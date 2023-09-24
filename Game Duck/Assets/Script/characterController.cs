using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class characterController : MonoBehaviour
{
    [SerializeField] FixedJoystick joystick;
    [SerializeField] float speedMove;
    Rigidbody2D rb;
    Vector2 move;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        move.x = joystick.Horizontal;
        move.y = joystick.Vertical;
        rb.MovePosition(rb.position + move*speedMove*Time.deltaTime);
    }
}
