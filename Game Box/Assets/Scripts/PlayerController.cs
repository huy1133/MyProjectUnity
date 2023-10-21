using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    List<Vector3> pointBox;
    
    void Start()
    {
        pointBox = new List<Vector3>();
    }
    
    public void create(List<Vector3> box)
    {
        clear();
        pointBox = new List<Vector3>(box);
        createBox();
    }
    void clear()
    {
        foreach(Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if(t!= gameObject.transform)
            {
                GameObject.Find("OblectPooling").GetComponent<ObjectBooling>().returnBox(t.gameObject);
            }
        }
    }
    void createBox()
    {
        int i = 0;
        foreach (Vector3 v in pointBox)
        {
            GameObject tempBox = GameObject.Find("OblectPooling").GetComponent<ObjectBooling>().getPlayerBox();
            tempBox.transform.SetParent(transform, false);
            tempBox.transform.localPosition = v;
            tempBox.name = i.ToString();
            i++;
        }
    }
}
