using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform character;
    Vector3 target;
    [SerializeField]float smooth;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = character.position;
        target.z = -10;

        transform.position = Vector3.MoveTowards(transform.position, target, smooth );
    }
}
