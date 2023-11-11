using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    ParticleSystem particle;
    LineRenderer lineRenderer;

    Ray shootRay;
    RaycastHit shootHit;

    float timeBetweenBulet;
    float countTime;
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        lineRenderer = GetComponent<LineRenderer>();   
        lineRenderer.enabled = false;
        timeBetweenBulet = 1;
        countTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)&&countTime>=timeBetweenBulet)
        {
            Shooting();
        }
        if(countTime<=timeBetweenBulet)
        {
            countTime += Time.deltaTime;
        }
    }
    void Shooting()
    {
        countTime = 0;
        particle.Play();

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);

        if (Physics.Raycast(shootRay, out shootHit, 100))
        {
            lineRenderer.SetPosition(1, shootHit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position );
        }
        Invoke("disableEffect", 0.2f);
    }
    private void disableEffect()
    {
        lineRenderer.enabled=false;
    }
}
