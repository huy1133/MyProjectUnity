using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [SerializeField] GameObject DestroyCube;
    

    private void OnTriggerEnter(Collider other)
    {
        Time.timeScale = 0;
        GameObject.Find("Obstacle").GetComponent<ObstacleController>().gameOver();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().isGame = gameStatic.gameOver;
        
    }
    private void OnMouseDown()
    {
        float temp = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().numberBoxCanBroken;
        if (temp > 0)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().numberBoxCanBroken--;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().playAudioDestroy();
            Destroy(Instantiate(DestroyCube, gameObject.transform.position, Quaternion.identity), 1);
            GameObject.Find("OblectPooling").GetComponent<ObjectBooling>().returnBox(gameObject);
        }

    }
}
