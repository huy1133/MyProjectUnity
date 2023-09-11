using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (setGame.gameOver)
        {
            saveGame();
        }
        if(!setGame.gameOver&&!setGame.gameStar)
        {
            loadGame();
        }
    }
    void loadGame()
    {
        setGame.coin = PlayerPrefs.GetInt("Coin");
        setGame.bestDistance = PlayerPrefs.GetInt("Distance");
    }
    void saveGame()
    {
        PlayerPrefs.SetInt("Coin",setGame.coin);
        PlayerPrefs.SetInt("Distance", setGame.bestDistance);
        PlayerPrefs.Save();
    }
}
