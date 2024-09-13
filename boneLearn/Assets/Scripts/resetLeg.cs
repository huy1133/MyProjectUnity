using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetLeg : MonoBehaviour
{
    [SerializeField] Transform targetPoint;
    [SerializeField] Transform targetlegMax;
    [SerializeField] Transform targetlegMin;
    float disMin;
    float disMax;
    float distance;
    Vector3 vectorTo;
    private void Start()
    {
        disMax = 8f;
        disMin = 2f;
        vectorTo = transform.position;
    }
    void Update()
    {
        distance = Vector3.Distance(transform.position, targetPoint.position);
        if (distance > disMax)
        {
            //transform.position = Vector3.Lerp(transform.position, targetlegMax.position, 10*Time.deltaTime);
            vectorTo = targetlegMax.position;
        }
        else if (distance < disMin)
        {
            //transform.position = Vector3.Lerp(transform.position, targetlegMin.position, 10f * Time.deltaTime);
            vectorTo = targetlegMin.position;
        }
        transform.position = Vector3.Lerp(transform.position, vectorTo, 5 * Time.deltaTime);
    }
}
