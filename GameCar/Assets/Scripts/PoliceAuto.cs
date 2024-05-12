using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoliceAuto : MonoBehaviour
{
    [SerializeField]float speed, steerAngle;
    [SerializeField] TrailRenderer[] trail;
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] ParticleSystem explosion;


    public Transform playerPointTarget;
   

    Rigidbody rb;

    Vector3 moveForce;
    bool isDie;
    Turn turn;
    float timeCanDrift,currentTimeCanDrift;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Ground")
        {
            explosion.gameObject.SetActive(true);
            isDie = true;
            trail[0].emitting = false;
            trail[1].emitting = false;
            particles[0].gameObject.SetActive(false);
            particles[1].gameObject.SetActive(false);
            Invoke("disable",3);
            if(collision.gameObject.tag!="Police"&& collision.gameObject.tag != "Player" && collision.gameObject.tag != "water")
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        turn = Turn.none;

        trail[0].emitting = false;
        trail[1].emitting = false;
        particles[0].gameObject.SetActive(false);
        particles[1].gameObject.SetActive(false);
        timeCanDrift = 0.2f;
        currentTimeCanDrift = 0;
        speed = 40f;
        isDie = false;
        explosion.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isDie)
        {
            controlCar();
        }
    }
    void Update()
    {
        
        AutoSteering();
    }
    void AutoSteering()
    {
        Vector3 vector1 = transform.forward.normalized,
                vector2 = (playerPointTarget.position - transform.position).normalized;
        float angle = Vector3.Angle(vector1,vector2);
        float leftAngle = Vector3.Angle
                (
                    Quaternion.Euler(0f, -90f, 0f) * vector1,
                    vector2
                );
        float rightAngle = Vector3.Angle
                (
                    Quaternion.Euler(0f, 90f, 0f) * vector1,
                    vector2
                );
        if (angle >= 20)
        {
            if (leftAngle > rightAngle)
            {
                turn = Turn.right;
            }
            else if(leftAngle < rightAngle) 
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
    void disable()
    {
        explosion.gameObject.SetActive(false);
        isDie = false;
        gameObject.SetActive(false);
    }
}
