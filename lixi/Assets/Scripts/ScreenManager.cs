using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Status
{
    screens1,
    screens2
}
public class ScreenManager : MonoBehaviour
{
    [SerializeField] GameObject screens1, screens2;
    [SerializeField] Text money;
  
    GameObject currentScreens;
    // Start is called before the first frame update
    void Start()
    {
        currentScreens = screens1;
        currentScreens.SetActive(true);
    }

    void changerScreens(Status st)
    {
        currentScreens.SetActive(false);
        if(st == Status.screens1)
        {
            currentScreens = screens1;
        }
        else
        {
            currentScreens = screens2;
        }
        currentScreens.SetActive(true);
    }
    public void randomMoney()
    {
        changerScreens(Status.screens2);
        Debug.Log("sdfsdfsd");
        int n = Random.Range(0, 100);
        if (n <= 50)
        {
            money.text = "10.000 vnd";
        }
        else if (n <= 80)
        {
            money.text = "20.000 vnd";
        }
        else if(n <= 98)
        {
            money.text = "50.000 vnd";
        }
        else
        {
            money.text = "100.000 vnd";
        }
    }
    public void resetRandom()
    {
        changerScreens(Status.screens1);
    }
}
