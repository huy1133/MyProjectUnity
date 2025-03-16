using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveonRope : MonoBehaviour
{
    public List<Transform> points;

    float speedClimb;
    GameObject a;
    GameObject b;
    float percentage;
    int total;
    Transform currentPoint;
    float timeSwing;
    bool isSwing;

    private void Start()
    {
        total = points.Count;
        percentage = 0.5f;
        a = null;
        speedClimb = 0.2f;
        timeSwing = 0;
        isSwing = false;
    }
    void Update()
    {
        if (a != null)
        {
            for (int i = 1; i < total - 1; i++)
            {
                if (percentage >= (float)i / total)
                {
                    float lerpFactor = (percentage - ((float)i / total)) * total;
                    a.transform.position = Vector2.Lerp(points[i].position, points[i + 1].position, lerpFactor);
                    Vector3 vectorOffset = Vector3.forward * -270;
                    a.transform.eulerAngles = Vector3.Lerp(points[i].eulerAngles, points[i + 1].transform.eulerAngles, lerpFactor) + vectorOffset;
                    if (b != null)
                    {
                        b.transform.eulerAngles = a.transform.eulerAngles;
                    }
                }
                else
                {
                    currentPoint = points[i];
                    break;
                }
            }
        }
        if (timeSwing > 0)
        {
            timeSwing -= Time.deltaTime;
            if (!isSwing)
            {
               foreach(var i in points)
               {
                    Rigidbody2D trb = i.gameObject.GetComponent<Rigidbody2D>();
                    trb.drag = Mathf.Lerp(trb.drag,0,1);
               }
            }
            isSwing = true;
        }
        else 
        {
            if (isSwing)
            {
                foreach (var i in points)
                {
                    i.gameObject.GetComponent<Rigidbody2D>().drag = 1;
                }
                isSwing = false;
            }
        }
    }
    public void Climb(float value)
    {
        percentage -= Time.deltaTime * value * speedClimb;
        percentage = Mathf.Clamp(percentage, 0.98f/total,(float) (total-2)/total );
    }
    public void swing(float value)
    {
        Vector3 character = b.transform.position;
        character.y = points[0].position.y;
        float distance = Vector3.Distance(points[0].position, character);
        if(distance < 2)
        {
            currentPoint.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * value * (7-Mathf.Pow(distance,4)));
        }
        timeSwing = 1;
    }
    public void ChangeIndexAnchor(Transform target, GameObject pointJoinRope)
    {
        int index = points.IndexOf(target);
        float temp = index / (float)total;
        percentage = temp;
        if (a == null)
        {
            a = pointJoinRope;
            a.transform.parent = null;
        }
    }
    public void DetachAnchor()
    {
        if (a != null)
        {
            a = null;
        }
    }
    public void joinCharacter(GameObject characterJoint)
    {
        if(b == null)
        {
            b = characterJoint;
        }
    }
    public void DetachCharacter()
    {
        if (b != null)
        {
            b=null;
        }
    }
}
