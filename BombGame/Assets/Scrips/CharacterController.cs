using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 trucNhay;
    public float lucNhay;
    // Start is called before the first frame update
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("cham mat dat");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("cham mat dat triigger");
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trucNhay = new Vector2(0, 1);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(trucNhay*lucNhay);
        }
    }
}
