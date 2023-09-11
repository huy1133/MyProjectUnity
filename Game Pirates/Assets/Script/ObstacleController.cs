using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private void Start()
    {
        
    }
    void Update()
    {
        if(setGame.getIsGame()) 
        {
            transform.Translate(Vector3.down * setGame.getSpeed() * Time.deltaTime);
        }
        
    }
    
}
