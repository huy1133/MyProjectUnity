
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    [SerializeField] GameObject character;
    [SerializeField] float speed;
    Vector3 target;
    // Start is called before the first frame update
   
    private void Update()
    {
        target = character.transform.position;
        Move();
    }
    // Update is called once per frame
    void Move()
    {
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
    }
}
