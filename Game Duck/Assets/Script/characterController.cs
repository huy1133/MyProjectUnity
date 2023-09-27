
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class characterController : MonoBehaviour
{
    [SerializeField] float speedMove;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    bool canSetFirstAgain;
    Vector3 firstPoint;
    Vector3 nowPoint;
    bool canMove;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        canSetFirstAgain = true;
 
    }

    // Update is called once per frame
   
    void Update()
    {
        move();
        Vector3 pos = transform.position;
        canMove = pos.x>=-7&&pos.x<=7&&pos.y>=-7&&pos.y<=7?true:false;
    }
    void move()
    {
        float m1 = Mathf.Clamp(transform.position.x, -7, 7);
        float m2 = Mathf.Clamp(transform.position.y, -7, 7);
        transform.position = new Vector3(m1, m2, transform.position.z);
        Vector3 vectorMove = (nowPoint - firstPoint).normalized;
        if (Input.GetMouseButton(0))
        {
            if (canSetFirstAgain)
            {
                firstPoint=Camera.main.ScreenToViewportPoint(Input.mousePosition);
                canSetFirstAgain = false;
            }
            nowPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            
            transform.Translate(vectorMove * Time.deltaTime * speedMove);
            animator.SetBool("Run", true);
            bool face = vectorMove.x > 0 ? true : false;

            spriteRenderer.flipX = face;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            canSetFirstAgain = true;
            animator.SetBool("Run", false);
        }
    }
    
}
