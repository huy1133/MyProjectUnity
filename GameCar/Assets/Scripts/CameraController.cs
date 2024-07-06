using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform car;
    [SerializeField] float speedMove;
    Vector3 offset;
    bool isFollow;
    
    // Start is called before the first frame update
    void Start()
    {
        isFollow = false;
    }
    

    void LateUpdate()
    {

        if (isFollow)
        {
            Vector3 vectorto = offset + car.position;
            transform.position = vectorto;
        }
        else
        {
            float newX = Mathf.Lerp(transform.position.x, 0, 3*Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            if (transform.position.x < 1)
            {
                starfollow();
            }
        }

        transform.LookAt(car);
    }
    void starfollow()
    {
        offset = transform.position - car.position;
        isFollow = true;
    }
}
