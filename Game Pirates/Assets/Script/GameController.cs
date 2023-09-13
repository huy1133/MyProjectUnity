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
        if (PlayerPrefs.GetInt("IsSkin") < 0 && PlayerPrefs.GetInt("IsSkin") > 12)
            PlayerPrefs.SetInt("IsSkin", 0);
        Instantiate(skinCharacter[PlayerPrefs.GetInt("IsSkin")]).transform.position=new Vector3(-0.5f,-0.6f,0);
        audio =gameObject.GetComponent<AudioSource>();
        PlayerPrefs.SetInt("skin" + 0, 1);
        setGame.distance = 0;
        setGame.speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Round(setGame.distance)>100)
        {
            setGame.speed += 0.0005f;
        }
        
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
        for(int i=0; i<12; i++)
        {
            setGame.unlockSkin[i] = PlayerPrefs.GetInt("skin" + i);
        }
        setGame.skin = PlayerPrefs.GetInt("IsSkin");
    }
    void saveGame()
    {
        for (int i = 0; i < 12; i++)
        {
           PlayerPrefs.SetInt("skin" + i, setGame.unlockSkin[i]) ;
        }
        PlayerPrefs.SetInt("Coin",setGame.coin);
        PlayerPrefs.SetInt("Distance", setGame.bestDistance);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("IsSkin", setGame.skin);
    }
}
