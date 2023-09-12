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
    [SerializeField] Text coin2;
    [SerializeField] Image overBar;
    [SerializeField] Sprite notSound;
    [SerializeField] Sprite sound;
    [SerializeField] Image buttonSound;
    [SerializeField] GameObject skinMenu;
    [SerializeField] GameObject[] buttonChooseSkin;
    [SerializeField] Sprite[] imgChooseBuy;

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
        coin2.text = "" + setGame.coin;
        for (int i=0; i < 12; i++)
        {
            buttonChooseSkin[i].GetComponent<Image>().sprite = imgChooseBuy[setGame.unlockSkin[i]];
            
        }
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
    public void skinButtonClick()
    {
        mainMenu.SetActive(false);
        skinMenu.SetActive(true);
    }
    public void homeButtonClickOfSkin()
    {
        SceneManager.LoadScene(0);
    }
    public void chooseSkinButton0()
    {
        if (setGame.unlockSkin[0] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 0);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin0", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    public void chooseSkinButton1()
    {
        if (setGame.unlockSkin[1] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 1);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin1", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    public void chooseSkinButton2()
    {
        if (setGame.unlockSkin[2] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 2);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin2", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    public void chooseSkinButton3()
    {
        if (setGame.unlockSkin[3] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 3);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin3", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    public void chooseSkinButton4()
    {
        if (setGame.unlockSkin[4] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 4);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin4", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    public void chooseSkinButton5()
    {
        if (setGame.unlockSkin[5] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 5);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin5", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    public void chooseSkinButton6()
    {
        if (setGame.unlockSkin[6] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 6);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin6", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    public void chooseSkinButton7()
    {
        if (setGame.unlockSkin[7] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 7);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin7", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    public void chooseSkinButton8()
    {
        if (setGame.unlockSkin[8] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 8);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin8", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    public void chooseSkinButton9()
    {
        if (setGame.unlockSkin[9] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 9);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin9", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    public void chooseSkinButton10()
    {
        if (setGame.unlockSkin[10] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 10);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin10", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    public void chooseSkinButton11()
    {
        if (setGame.unlockSkin[11] == 1)
        {
            PlayerPrefs.SetInt("IsSkin", 11);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (setGame.coin > 100)
            {
                PlayerPrefs.SetInt("skin11", 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 100);
            }
        }
    }
    
}
