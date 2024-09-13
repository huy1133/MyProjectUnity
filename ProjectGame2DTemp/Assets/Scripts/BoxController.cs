using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    [SerializeField] AudioClip boxMoveClip;
    [SerializeField] AudioSource sound;
    Rigidbody2D rb;
    Animator animator;
    bool canPlaySound;
    float timeResetAnim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sound.clip = boxMoveClip;
        canPlaySound = true;
        timeResetAnim = 2f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Character" || collision.gameObject.tag == "Box")
        {
            rb.gravityScale = 35;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Character" || collision.gameObject.tag == "Box")
        {
            rb.gravityScale = 35;
        }
        else
        {
            rb.gravityScale = 5;
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Box")
    //    {
    //        push(collision.gameObject);
    //    }
    //}
    private void Update()
    {
        timeResetAnim = timeResetAnim<=0 ? timeResetAnim : timeResetAnim - Time.deltaTime;    
        if(timeResetAnim <= 0)
        {
            timeResetAnim = 2f;
            animator.SetFloat("type", Random.Range(1, 3));
        }
        if (rb.velocity.x >0.1f || rb.velocity.x < -0.1f )
        {
            if (canPlaySound)
            {
                sound.Play();
                canPlaySound=false;
            }
        }
        else
        {
            sound.Stop();
            canPlaySound = true;
        }
    }
    void push(GameObject gameObj)
    {
        float force = 1000f;
        Rigidbody2D tempRB = gameObj.GetComponent<Rigidbody2D>();
        if (gameObj.transform.position.x < transform.position.x)
        {
            rb.AddForce(Vector2.right * force);
        }
        else
        {
            rb.AddForce(Vector2.left * force);
        }
    }
}

