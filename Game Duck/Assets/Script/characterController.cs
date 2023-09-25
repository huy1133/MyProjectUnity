
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class characterController : MonoBehaviour
{
    [SerializeField] float speedMove;

    Rigidbody2D rb;
    Animator animator;
    Vector2 moves;
    bool canSetFirstAgain;
    Vector3 firstPoint;
    Vector3 nowPoint;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canSetFirstAgain = true;
    }

    // Update is called once per frame
   
    void Update()
    {
        move();
    }
    void move()
    {
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
            int face = vectorMove.x > 0 ? -1 : 1;
            Vector3 scale = transform.localScale;
            scale.x = face*Mathf.Abs(transform.localScale.x);
            transform.localScale = scale;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            canSetFirstAgain = true;
            animator.SetBool("Run", false);
        }
        Debug.Log(vectorMove);
    }
}
