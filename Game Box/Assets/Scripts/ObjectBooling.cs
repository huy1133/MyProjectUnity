using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBooling : MonoBehaviour
{
    [SerializeField] GameObject playerBox;
    [SerializeField] GameObject obstacleBox;
    [SerializeField] GameObject gateBox;
    List<GameObject> obstacles;
    List<GameObject> gates;
    List<GameObject> players;

    private void Start()
    {
        obstacles = new List<GameObject>();
        gates = new List<GameObject>();
        players = new List<GameObject>();
        createPlayerBox();
        createGateBox();
        createObstacleBox();
    }
    

    void createPlayerBox()
    {
        for(int i=0; i<25; i++)
        {
            GameObject temp = Instantiate(playerBox);
            temp.SetActive(false);
            temp.transform.SetParent(transform,false);
            players.Add(temp);
        }
    }
    void createObstacleBox()
    {
        for (int i = 0; i < 25; i++)
        {
            GameObject temp = Instantiate(obstacleBox);
            temp.SetActive(false);
            temp.transform.SetParent(transform, false);
            obstacles.Add(temp);
        }
    }
    void createGateBox()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject temp = Instantiate(gateBox);
            temp.SetActive(false);
            temp.transform.SetParent(transform, false);
            gates.Add(temp);
        }
    }
    public GameObject getPlayerBox()
    {
        foreach(GameObject temp in players)
        {
            if (!temp.activeInHierarchy)
            {
                temp.SetActive(true);
                return temp;
            }
        }
        return null;
    }
    public GameObject getObstacleBox()
    {
        foreach (GameObject temp in obstacles)
        {
            if (!temp.activeInHierarchy)
            {
                temp.SetActive(true);

                return temp;
            }
        }
        return null;
    }
    public GameObject getGateBox()
    {
        foreach(GameObject temp in gates)
        {
            if (!temp.activeInHierarchy)
            {
                temp.SetActive(true);
                return temp;
            }
        }
        return null;
    }
    public void returnBox(GameObject temp)
    {
        temp.transform.SetParent(transform, false );
        temp.SetActive(false);
    }

}
