using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;

    bool isDie;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        isDie = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDie)
        {
            agent.SetDestination(player.position);
        }
    }
}
