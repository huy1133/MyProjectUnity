using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarController : MonoBehaviour
{

    float moveSpeed = 50;
    float drag = 0.98f;
    float maxSpeed = 15;
    float steerAngle = 20;

    Vector3 moveForce;
    private void Update()
    {
        
        //moving
        moveForce += transform.forward* Input.GetAxis("Vertical") * moveSpeed*Time.deltaTime;
        transform.position += moveForce*Time.deltaTime;
        //Steering
        transform.Rotate(Vector3.up*Input.GetAxis("Horizontal") * moveForce.magnitude * steerAngle * Time.deltaTime);
        //drag and speed limit
        moveForce *= drag;
        moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed);

        Debug.DrawRay(transform.position, transform.forward.normalized*3, Color.yellow);
        Debug.DrawRay(transform.position, moveForce.normalized * 3, Color.red);

        moveForce = Vector3.Lerp(moveForce.normalized, transform.forward.normalized, 1)*moveForce.magnitude;
    }
}
