using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool hideMouse;
    Animator animator;
    Rigidbody rb;

    float horizontal;
    float vertical;
    Vector3 loockPos;
    Vector3 moveMent;
    float forWardAmount;
    float turnAmount;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        loockPos = transform.forward;
        setupAnimator();
        if(hideMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        moveMent = new Vector3(horizontal, 0, vertical).normalized;
        rb.AddForce(moveMent*speed/Time.deltaTime);
    }
    private void Update()
    {
        rotatePlayer();
        convertMove();
    }

    void rotatePlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100))
        {
            loockPos = hit.point;
            loockPos.y = transform.position.y;
        }
        transform.LookAt( loockPos );
    }
    void convertMove()
    {
        if ((rb.velocity.magnitude) > 2)
        {
            float angle = Vector3.Angle(moveMent, loockPos.normalized);
            if(angle < 45)
            {
                if (forWardAmount > 0.05f || forWardAmount < -0.05f)
                {
                    forWardAmount += 0.05f * (forWardAmount > 0 ? -1 : 1);
                }
                else
                {
                    forWardAmount = 0.0f;
                }

                turnAmount += 0.05f;
            }
            else if(angle > 135)
            {
                if (forWardAmount > 0.05f || forWardAmount < -0.05f)
                {
                    forWardAmount += 0.05f * (forWardAmount > 0 ? -1 : 1);
                }
                else
                {
                    forWardAmount = 0.0f;
                }
                turnAmount -= 0.05f;
            }
            else
            {
                if (moveMent.x > 0 || moveMent.z > 0)
                {
                    forWardAmount += 0.05f;
                }
                else if (moveMent.x < 0 || moveMent.z < 0)
                {
                    forWardAmount -= 0.05f;
                }

                if (turnAmount > 0.05f || turnAmount < -0.05f)
                {
                    turnAmount += 0.05f * (turnAmount > 0 ? -1 : 1);
                }
                else
                {
                    turnAmount = 0.0f;
                }
            }
            
        }
        else
        {
            if (turnAmount > 0.05f || turnAmount < -0.05f)
            {
                turnAmount += 0.05f * (turnAmount > 0 ? -1f : 1f);
            }
            else
            {
                turnAmount = 0;
            }
            if (forWardAmount > 0.05f || forWardAmount < -0.05f)
            {
                forWardAmount += 0.05f * (forWardAmount > 0 ? -1f : 1f);
            }
            else
            {
                forWardAmount = 0;
            }
        }
        forWardAmount = Mathf.Clamp(forWardAmount, -1f, 1f);
        turnAmount = Mathf.Clamp(turnAmount, -1f, 1f);
        animator.SetFloat("SideWay", turnAmount);
        animator.SetFloat("ForWard", forWardAmount);
    }
    void setupAnimator()
    {
        animator = GetComponent<Animator>();
        foreach(var a in GetComponentsInChildren<Animator>())
        {
            if(a != animator)
            {
                animator.avatar = a.avatar;
                Destroy(a);
                break;
            }
        }
    }
}
