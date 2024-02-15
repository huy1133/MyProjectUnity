using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Screens
{
    Main,
    Menu,
    StatrGame,
    EndGame
}
public class ScreensManager : MonoBehaviour
{
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject StatrGame;
    [SerializeField] GameObject EndGame;
    [SerializeField] GameObject Main;

    [SerializeField] GameObject Story;
    [SerializeField] GameObject Practicle;
    [SerializeField] GameObject Listening;

    GameObject currentScreen;
    GameObject currentScreenOfMenu;

    private void Awake()
    {
        currentScreen = Main;
        currentScreen.SetActive(true);
        currentScreenOfMenu = null;
    }

    public void changeScreens(Screens screens)
    {
        currentScreen.SetActive(false);
        if(currentScreenOfMenu != null)
        {
            currentScreenOfMenu.SetActive(false);
            currentScreenOfMenu = null;
        }
        switch (screens)
        {
            case Screens.Menu:
                currentScreen = Menu;
                break;
            case Screens.Main:
                currentScreen = Main;
                break;
            case Screens.StatrGame:
                currentScreen = StatrGame;
                break;
            case Screens.EndGame:
                currentScreen = EndGame;
                break;
        }
        currentScreen.SetActive(true);
    }
    public void changeScreensOfMenu(int n)
    {
        switch (n)
        {
            case 0:
                currentScreenOfMenu = Story;
                break;
            case 1:
                currentScreenOfMenu = Practicle;
                break;
            case 2:
                currentScreenOfMenu = Listening;
                break;
        }
        currentScreenOfMenu.SetActive(true);
    }
    public void backScreen()
    {
        if (currentScreen == Menu) 
        {
            if(currentScreenOfMenu!=null)
            {
                currentScreenOfMenu.SetActive(false);
                currentScreenOfMenu = null;
            }
            else
            {
                changeScreens(Screens.Main);
            }
        }
        else if(currentScreen == StatrGame)
        {
            SceneManager.LoadScene(0);
        }
        else if(currentScreen == EndGame)
        {
            SceneManager.LoadScene(0);
        }
       
    }
}
