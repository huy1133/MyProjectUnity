using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject overMenu;
    [SerializeField] Text distance;
    [SerializeField] Text bestDistance;
    [SerializeField] Text coin;
    [SerializeField] Image overBar;
    [SerializeField] Sprite notSound;
    [SerializeField] Sprite sound;
    [SerializeField] Image buttonSound;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        overMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!setGame.isSound)
        {
            buttonSound.sprite = notSound;
        }
        else
        {
            buttonSound.sprite = sound;
        }
        if (setGame.gameOver)
        {
            overMenu.SetActive(true);
            if (overBar.rectTransform.anchoredPosition.y > 0)
            {
                float posy = overBar.rectTransform.anchoredPosition.y - 10;
                overBar.rectTransform.anchoredPosition = new Vector3(0,posy,0);
            }
            
        }
        else
        {
            overMenu.SetActive(false);
        }
        if (!setGame.gameOver && !setGame.gameStar)
        {
            mainMenu.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(false);
        }
        distance.text = "" + Math.Round(setGame.distance) + "m";
        bestDistance.text = "BEST DISTANCE: " + setGame.bestDistance;
        coin.text = ""+setGame.coin;
    }
    public void playButtonClick()
    {
        setGame.gameStar = true;
        setGame.gameOver = false;
    }
    public void homeButtonClick()
    {
        setGame.gameOver = false;
        setGame.gameStar = false;
        setGame.distance = 0;
        setGame.speed = 5;
        SceneManager.LoadScene(0);
    }
    public void soundButtonClick()
    {
        if (setGame.isSound)
        {
            setGame.isSound = false;
        }
        else
        {
            setGame.isSound=true;
        }
    }
}
