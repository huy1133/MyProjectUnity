using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckManager : MonoBehaviour
{
    [SerializeField] GameObject[] ducks;
    int leverDuck;
    int exDuck;
    float currentTime;
    int Difficulty;
    private void Start()
    {
        currentTime = 21;
        Difficulty = 5;
        leverDuck = 1;
        exDuck = 0;
    }
    private void Update()
    {
        if(currentTime >= 15) 
        {
            if (exDuck == 2)
            {
                Difficulty += 1;
                leverDuck++;
                exDuck = 0;
            }
            if (canCreateDuck())
            {
                createDuck();
                exDuck++;
                currentTime = 0;

            }
        }
        else
        {
            currentTime += Time.deltaTime;
        }
        
       
    }
    bool canCreateDuck()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("enemy");
        if (temp.Length <= 15)
            return true;
        return false;
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
