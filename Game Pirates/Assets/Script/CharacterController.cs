using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    Animator animator;
    AudioSource audio;
    [SerializeField] AudioClip sideSound;
    [SerializeField] AudioClip coinSound;
    [SerializeField] AudioClip dieSound;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            audio.PlayOneShot(dieSound);
            setGame.gameStar = false;
            setGame.gameOver = true;
            setGame.coin += setGame.tempCoin;
            if (setGame.distance > setGame.bestDistance)
            {
                setGame.bestDistance = (int)Math.Round(setGame.distance);
            }
        }
        if (collision.gameObject.tag == "Coin")
        {
            audio.PlayOneShot(coinSound);
            Destroy(collision.gameObject);
            setGame.tempCoin++;
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        setGame.tempCoin = 0;
        audio = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (setGame.gameStar)
        {
            
            setGame.distance+=setGame.speed*Time.deltaTime;
            if (transform.position.y < 0)
            {
                transform.Translate(Vector3.up * 1 * Time.deltaTime);
            }
            animator.SetTrigger("IsClim");
            if (Input.GetMouseButtonDown(0))
            {
                audio.PlayOneShot(sideSound);
                Vector3 pos = transform.position;
                Vector3 sca = transform.localScale;
                transform.position = new Vector3(pos.x * -1, pos.y, pos.z);
                transform.localScale = new Vector3(sca.x * -1, sca.y, sca.z);
            }
        }
        
       
        if (!setGame.gameStar && setGame.gameOver)
        {
            if (transform.position.y > -10)
            {
                transform.position += Vector3.down*3*Time.deltaTime;
            }
        }
        if (setGame.isSound)
        {
            audio.mute = false;
        }
        else
        {
            audio.mute = true;
        }
        //textCoin.text = "" + coin;
        //textDistance.text = "" + setGame.distance+"m";
        //Debug.Log(setGame.coin+" "+setGame.bestDistance);
    }
}
