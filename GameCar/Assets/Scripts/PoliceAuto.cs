using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoliceAuto : MonoBehaviour
{
    [SerializeField] float speed, steerAngle;
    [SerializeField] TrailRenderer[] trail;
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] Transform playerPointTarget;

    Rigidbody rb;

    Vector3 moveForce;

    Turn turn, currentTurn;
    float currentAngle;
    float timeCanDrift,currentTimeCanDrift;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        turn = Turn.none;
        currentTurn = Turn.left; currentAngle = 0;

        trail[0].emitting = false;
        trail[1].emitting = false;
        particles[0].gameObject.SetActive(false);
        particles[1].gameObject.SetActive(false);
        timeCanDrift = 0.5f;
        currentTimeCanDrift = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        controlCar();
    }
    void Update()
    {
        AutoSteering();
    }
    void AutoSteering()
    {
        Vector3 vector1 = transform.forward.normalized, vector2 = (playerPointTarget.position - transform.position).normalized;
        float angle = Vector3.Angle(vector1,vector2);
        if (angle >= 10)
        {
            if (angle > currentAngle)
            {
                if(currentTurn == Turn.left)
                {
                    currentTurn = Turn.right;
                }
                else if(currentTurn == Turn.right)
                {
                    currentTurn = Turn.left;
                }
                
            }
            turn = currentTurn;
        }
        else
        {
            turn = Turn.none;
        }
       
        currentAngle = angle;
    }
    void controlCar()
    {
        moveForce = transform.forward * speed;
        rb.AddForce(moveForce);
        
        if(turn == Turn.none) 
        {
            trail[0].emitting = false;
            trail[1].emitting = false;
            particles[0].gameObject.SetActive(false);
            particles[1].gameObject.SetActive(false);
            currentTimeCanDrift = 0;
        }
        else
        {
            if (turn == Turn.left)
            {
                transform.Rotate(Vector3.up * steerAngle * -1);
            }
            else
            {
                transform.Rotate(Vector3.up * steerAngle);
            }
            currentTimeCanDrift += Time.deltaTime;
            if (currentTimeCanDrift >= timeCanDrift)
            {
                trail[0].emitting = true;
                trail[1].emitting = true;
                particles[0].gameObject.SetActive(true);
                particles[1].gameObject.SetActive(true);
            }
        }
    }
}
