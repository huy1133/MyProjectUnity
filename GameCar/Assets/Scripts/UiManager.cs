using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject changeCar;
    [SerializeField] GameObject changemap;
    [SerializeField] GameObject next;
    [SerializeField] GameObject back;
    [SerializeField] GameObject play;
    [SerializeField] GameObject islock;

    enum Page
    {
        ChangeCar, 
        ChangeMap
    }
    Page page;
    void Start()
    {
        page = Page.ChangeCar;
        PlayerPrefs.SetInt("UnlockedCar0", 1);
        PlayerPrefs.SetInt("UnlockedMap0", 1);
        PlayerPrefs.Save(); 
    }

    // Update is called once per frame
    void Update()
    {
        showPage();
    }

    bool checkUnlocked(string check)
    {
        if (PlayerPrefs.GetInt(check) == 1)
        {
            return true;
        }
        return false;
    }

    void showPage()
    {
        if(page == Page.ChangeCar)
        {
            changeCar.SetActive(true);
            changemap.SetActive(false);
            back.SetActive(false);
            play.SetActive(false);
            if (checkUnlocked("UnlockedCar"+PlayerPrefs.GetInt("CurrentCar").ToString()))
            {
                islock.SetActive(false);
                next.SetActive(true);
            }
            else
            {
                islock.SetActive(true) ;
                next.SetActive(false);
            }
        }
        else
        {
            changeCar.SetActive(false);
            changemap.SetActive(true);
            next.SetActive(false);
            back.SetActive(true);
            if (checkUnlocked("UnlockedMap" + PlayerPrefs.GetInt("CurrentMap").ToString()))
            {
                islock.SetActive(false) ;
                play.SetActive(true);
            }
            else
            {
                islock.SetActive(true);
                play.SetActive(false);
            }
        }
    }
    public void nextPageButton()
    {
        page = Page.ChangeMap;
    }
    public void backPageButton()
    {
        page = Page.ChangeCar;
    }
}
