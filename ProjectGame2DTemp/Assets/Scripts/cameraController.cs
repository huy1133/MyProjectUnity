using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] GameObject target;
    Vector3 vectorTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vectorTarget.x = target.transform.position.x;
        vectorTarget.y = target.transform.position.y;
        vectorTarget.z = transform.position.z;
        transform.position = vectorTarget;
    }
}
