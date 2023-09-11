using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Animator animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            setGame.setIsGame(false);
            
        }
        if (collision.gameObject.tag == "Coin")
        {
            Debug.Log("awn");
            Destroy(collision.gameObject);
            setGame.plusCoin();
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (setGame.getIsGame())
        {
            setGame.plusDistanceMove(setGame.getSpeed()*Time.deltaTime);
            if (transform.position.y < 0)
            {
                transform.Translate(Vector3.up * 1 * Time.deltaTime);
            }
            animator.SetTrigger("IsClim");
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = transform.position;
                Vector3 sca = transform.localScale;
                transform.position = new Vector3(pos.x * -1, pos.y, pos.z);
                transform.localScale = new Vector3(sca.x * -1, sca.y, sca.z);
            }
        }
       
        Debug.Log(setGame.getCoin()+" "+setGame.getDistanceMove());
    }
}
