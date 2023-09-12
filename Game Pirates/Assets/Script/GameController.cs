using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject[] skinCharacter;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(skinCharacter[setGame.skin]).transform.position=new Vector3(-0.5f,-0.6f,0);
        audio =gameObject.GetComponent<AudioSource>();
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
        if (setGame.isSound)
        {
            audio.mute = false;
        }
        else
        {
            audio.mute = true;
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
