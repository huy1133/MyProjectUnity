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
        if(setGame.gameStar) 
        {
            transform.Translate(Vector3.down * setGame.speed * Time.deltaTime);
        }
        
    }
    
}
