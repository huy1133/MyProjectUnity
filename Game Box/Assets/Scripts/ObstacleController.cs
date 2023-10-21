using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleController : MonoBehaviour
{
    
    List<Vector3> obstaclePoint;
    List<Vector3> gatePoint;
    bool canMove;
    float speed;
    Vector3 startPos;
    private void Start()
    {
        obstaclePoint = new List<Vector3>();
        gatePoint = new List<Vector3>();
        startPos = transform.position;
        canMove = false;
    }
    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().numberBoxCanBroken <= 0)
        {
            speed = 30;
        }
        if(canMove)
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
    }
    public void Create(List<Vector3> box, List<Vector3> gate)
    {
        canMove = true;
        transform.position = startPos;
        speed = 10;
        clear();
        obstaclePoint = new List<Vector3>(box);
        gatePoint = new List<Vector3>(gate);
        createObstacle();
        createGate();
    }
    void clear()
    {
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
           
            if (t != gameObject.transform && t.gameObject.name != "Check")
            {
                GameObject.Find("OblectPooling").GetComponent<ObjectBooling>().returnBox(t.gameObject);
            }
        }
    }
    void createObstacle()
    {
        int number = obstaclePoint.Count - GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().numberBoxCanBroken;
        for (int i=0; i<number; i++)
        {
            obstaclePoint.RemoveAt(Random.Range(0, obstaclePoint.Count));
        }
        foreach (Vector3 v in obstaclePoint)
        {
            GameObject tempBox = GameObject.Find("OblectPooling").GetComponent<ObjectBooling>().getObstacleBox();
            tempBox.transform.SetParent(transform, false);
            tempBox.transform.localPosition = v;
        }
    } 
    void createGate()
    {
        foreach(Vector3 v in gatePoint)
        {
            GameObject tempBox = GameObject.Find("OblectPooling").GetComponent<ObjectBooling>().getGateBox();
            tempBox.transform.SetParent(transform, false);
            tempBox.transform.localPosition = v;
        }
    }
    public void gameOver()
    {
        canMove = false;
        Vector3 tempPos = transform.position;
        tempPos.z = -11;
        transform.position = tempPos;
        Time.timeScale = 1;
    }
}
