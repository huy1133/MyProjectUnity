using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
   
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if(SetGame.gameStar)
            transform.Translate(new Vector3(-1,0,0) * SetGame.speed);
        
    }
}
