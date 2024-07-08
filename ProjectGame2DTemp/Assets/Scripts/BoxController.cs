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
}

