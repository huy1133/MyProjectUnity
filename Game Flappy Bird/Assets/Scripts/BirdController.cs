using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float force;
    Rigidbody2D rigidbody;
    bool isGame;

    float moveUpDown = -0.02f;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        isGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isGame)
        {
            if (transform.position.y > 0.5) moveUpDown = -0.02f;
            if (transform.position.y < -0.5) moveUpDown = 0.02f;
            transform.Translate(Vector3.up * moveUpDown);
        }
        if (Input.GetMouseButtonDown(0))
        {
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            isGame = true;

            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * force, ForceMode2D.Force);

        }
    }
}
