using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float force;
    [SerializeField] AudioClip swing;
    [SerializeField] AudioClip die;
    [SerializeField] AudioClip plusScore;
    AudioSource audio;
    Rigidbody2D rigidbody;
    Vector3 rotate = new Vector3(0, 0, 35);

    float moveUpDown = -0.02f;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!SetGame.gameStar)
        {
            if(!SetGame.gameOver)
            {
                if (transform.position.y > 0.5) moveUpDown = -0.02f;
                if (transform.position.y < -0.5) moveUpDown = 0.02f;
                transform.Translate(Vector3.up * moveUpDown);
            }
        }
        else
        {
            move();
        }
        
        if (Input.GetMouseButtonDown(0)&& !SetGame.gameStar &&!SetGame.gameOver)
        {
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            SetGame.gameStar = true;
            move();
            
        }
    }
    void move()
    {
        if (rigidbody.velocity.y >= 0) transform.localRotation = Quaternion.Euler(rotate);
        else transform.localRotation = Quaternion.Euler(rotate * -1);

        if (Input.GetMouseButtonDown(0)&&SetGame.gameStar)
        {
            audio.PlayOneShot(swing);
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * force, ForceMode2D.Force);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            if(SetGame.gameStar)audio.PlayOneShot(die);
            SetGame.gameStar = false;
            SetGame.gameOver = true;
           
            GameObject.Find("GameController").GetComponent<UIController>().showMainMenu();
        }
        if(collision.gameObject.tag == "score")
        {
            audio.PlayOneShot(plusScore);
            GameObject.Find("GameController").GetComponent<GameController>().plusScore();
            
        }
        
    }
}
