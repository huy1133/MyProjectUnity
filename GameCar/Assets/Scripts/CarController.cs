using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

enum Turn
{
    turnLeft,
    turnRight,
    straight
}
public class CarController : MonoBehaviour
{
    [SerializeField] float speed;
    Turn turn;
    // Start is called before the first frame update
    void Start()
    {
        turn = Turn.straight;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        transform.Rotate(Vector3.up * horizontal * 100 * Time.deltaTime);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Debug.Log(transform.forward);
    }
}
