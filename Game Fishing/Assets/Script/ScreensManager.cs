using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Screens
{
    MainMenu,
    InGame,
    EndGame,
    Return
}
public class ScreensManager : MonoBehaviour
{
    public static ScreensManager instance;
    GameObject currentScreen;
    [SerializeField] GameObject mainMenuScreen;
    [SerializeField] GameObject inGameScreen;
    //[SerializeField] GameObject endGameScreen;
    private void Awake()
    {
        instance = this;
        currentScreen = mainMenuScreen;
    }
    public void ChangeScreen(Screens screen)
    {
        currentScreen.SetActive(false);
        switch (screen)
        {
            case Screens.MainMenu:
                currentScreen = mainMenuScreen; 
                break;
            case Screens.InGame:
                currentScreen = inGameScreen;
                break;
        }
        currentScreen.SetActive(true);
    }
}
