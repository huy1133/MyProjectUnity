using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] map;
    [SerializeField] Text nameMap;
    [SerializeField] Image[] star;
    [SerializeField] Sprite[] spriteStar;
    string[] nameMaps = { "Desert" , "Midnight City" };
    void Start()
    {
        PlayerPrefs.SetInt("LeverMap0", 0);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        showMap();
        showInforMap();
    }
    void showInforMap()
    {
        nameMap.text = nameMaps[PlayerPrefs.GetInt("CurrentMap")];
        string leverstr = "LeverMap" + PlayerPrefs.GetInt("CurrentMap").ToString();
        for(int i = 0; i < star.Length; i++) 
        { 
            if(i<PlayerPrefs.GetInt(leverstr))
            {
                star[i].sprite = spriteStar[1];
            }
            else
            {
                star[i].sprite = spriteStar[0];
            }
        }
    }
    void showMap()
    {
        for(int i = 0; i < map.Length; i++)
        {
            if (i != PlayerPrefs.GetInt("CurrentMap"))
            {
                map[i].SetActive(false);
            }
            else
            {
                map[i].SetActive(true);
               
            }
        }
    }
    public void ChangeLeft()
    {
        int temp = PlayerPrefs.GetInt("CurrentMap") - 1;
        if(temp < 0)
            temp = 1;
        PlayerPrefs.SetInt("CurrentMap", temp);
        PlayerPrefs.Save();
    }
    public void ChangeRight()
    {
        int temp = PlayerPrefs.GetInt("CurrentMap") + 1;
        int maxCountMap = map.Length;
        temp %= maxCountMap;
        PlayerPrefs.SetInt("CurrentMap", temp);
        PlayerPrefs.Save();
    }
}
