using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckManager : MonoBehaviour
{
    [SerializeField] GameObject[] ducks;
    int leverDuck;
    float currentTime;
    int Difficulty;
    bool canCreateDuck;
    private void Start()
    {
        currentTime = 11;
        Difficulty = 5;
        leverDuck = 1;
        canCreateDuck = true;
    }
    private void Update()
    {
        if(currentTime >= 10 && canCreateDuck ) 
        {
            Debug.Log("tao vi");
            createDuck();
            Difficulty += 5;
            leverDuck++;
            currentTime=0;
        }
        currentTime += Time.deltaTime;
        check();
    }
    void check()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("enemy");
        canCreateDuck = false;
        if (temp.Length <= 15)
            canCreateDuck = true;
    }
    void createDuck()
    {
        for(int i=0; i< Difficulty; i++)
        {
            int index = Random.Range(0, 2);
            Duck(index);
        }
    }
    void Duck(int index)
    {
        Vector3 pont;
        Vector3 target = GameObject.Find("Player").transform.position;
        while (true)
        {
            pont = new Vector3(Random.Range(-7, 7), Random.Range(-7, 7), 0);
            if ((target - pont).magnitude > 3) break;
        }
        GameObject duck = Instantiate(ducks[index],pont,Quaternion.identity);
        duck.GetComponent<DuckController>().lever(leverDuck);
    }
}
