using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn
{
    left,
    right,
    none
}
public class CarController1 : MonoBehaviour
{
    [SerializeField] float speed, steerAngle;
    [SerializeField] TrailRenderer[] trail;
    [SerializeField] ParticleSystem[] particles;

    Rigidbody rb;

    Vector3 moveForce;
    Turn turn;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        turn = Turn.none;
       
        trail[0].emitting = false;
        trail[1].emitting = false;
        particles[0].gameObject.SetActive(false);
        particles[1].gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        controlCar();
    }
    void Update()
    {
        input();
    }
    void input()
    {
        Vector3 screenPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        if (Input.GetMouseButton(0)) 
        {
            if (screenPoint.x > 0.5f)
            {
                turn = Turn.right;
            }
            else
            {
                turn = Turn.left;
            }
        }
        else
        {
            turn = Turn.none;
        }
    }
    void controlCar()
    {
        moveForce = transform.forward*speed;
        rb.AddForce(moveForce);
        if(turn == Turn.left)
        {
            transform.Rotate(Vector3.up*steerAngle*-1);
            trail[0].emitting=true;
            trail[1].emitting = true;
            particles[0].gameObject.SetActive(true);
            particles[1].gameObject.SetActive(true);
        }
        else if(turn == Turn.right)
        {
            transform.Rotate(Vector3.up * steerAngle);
            trail[0].emitting = true;
            trail[1].emitting = true;
            particles[0].gameObject.SetActive(true);
            particles[1].gameObject.SetActive(true);
        }
        else
        {
            trail[0].emitting = false;
            trail[1].emitting = false;
            particles[0].gameObject.SetActive(false);
            particles[1].gameObject.SetActive(false);
        }
    }
}
