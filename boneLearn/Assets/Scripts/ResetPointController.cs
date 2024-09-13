using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPointController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D rayhit = Physics2D.Raycast(transform.position, Vector3.down, 10f, groundLayer);
        if (rayhit.collider != null)
        {
            Vector3 point = rayhit.point;
            point.y += 0.1f;
            transform.position = point;
        }
    }
}
