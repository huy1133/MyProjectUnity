using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform car;
    [SerializeField] float speedMove;
    Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        distance = transform.position - car.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorto = distance+car.position;
        vectorto.y=distance.y;
        transform.position =Vector3.Slerp(transform.position,vectorto,speedMove);
       
    }
}
