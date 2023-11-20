using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCars : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    private int currentCar;
    private bool canChangeCar;
    private bool isChangeLeft;
    void Start()
    {
        currentCar = PlayerPrefs.GetInt("CurrentCar");
        cars[PlayerPrefs.GetInt("CurrentCar")].gameObject.transform.position=Vector3.zero;
        canChangeCar = false;
        isChangeLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        changeCar();
    }
    void changeCar()
    {
        if (canChangeCar && currentCar != PlayerPrefs.GetInt("CurrentCar"))
        {
            Vector3 vectorMove=isChangeLeft?Vector3.right:Vector3.left;
            if (cars[PlayerPrefs.GetInt("CurrentCar")].transform.position.x != 0)
            {
                cars[PlayerPrefs.GetInt("CurrentCar")].transform.position += vectorMove;
                cars[currentCar].transform.position += vectorMove;
            }
            else
            {
                canChangeCar = false;
                currentCar = PlayerPrefs.GetInt("CurrentCar");
            }
        }
        
    }
    public void changeCarRightButton()
    {
        if (!canChangeCar)
        {
            Vector3 newPos = new Vector3(17, 0, 0);
            int maxCountCar = 10;
            int temp = (PlayerPrefs.GetInt("CurrentCar") + 1) % maxCountCar;
            PlayerPrefs.SetInt("CurrentCar", temp);
            PlayerPrefs.Save();
            cars[PlayerPrefs.GetInt("CurrentCar")].transform.position = newPos;
            canChangeCar = true;
            isChangeLeft = false;
        }
    }
    public void changeCarLeftButon()
    {
        if (!canChangeCar)
        {
            Vector3 newPos = new Vector3(-17, 0, 0);
            int maxCountCar = cars.Length;
            int temp = (PlayerPrefs.GetInt("CurrentCar") - 1);
            if(temp <0)
            {
                temp = maxCountCar - 1;
            }
            PlayerPrefs.SetInt("CurrentCar", temp);
            PlayerPrefs.Save();
            cars[PlayerPrefs.GetInt("CurrentCar")].transform.position = newPos;
            canChangeCar = true;
            isChangeLeft = true;
        }
    }
}
