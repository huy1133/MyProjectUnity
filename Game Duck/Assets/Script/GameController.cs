using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class GameController : MonoBehaviour
{
    [SerializeField] Text eggText;
    [SerializeField] Slider BloddSlider;
    public int eggate;
    float blood;

    void Start()
    {
        if (PlayerPrefs.GetInt("firt") == 0)
        {
            loadingGameFirt();
            PlayerPrefs.SetInt("firt", 1);
            PlayerPrefs.Save();
        }
        eggate = 0;
        blood = PlayerPrefs.GetInt("Blood");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(eggate);
        UI();
    }
    void loadingGameFirt()
    {
        Debug.Log("yea");
        PlayerPrefs.SetInt("Damage", 1);
        PlayerPrefs.SetInt("SpeedShoot", 5);
        PlayerPrefs.SetInt("Blood", 5);
        PlayerPrefs.Save();
    }
    public void damaged(int D)
    {
        blood -= D;
        die();
    }
    void die()
    {
        if (blood <= 0)
        {
            Time.timeScale = 0;
        }
    }
    private void UI()
    {
        eggText.text = "" + eggate;
        BloddSlider.value = (float)blood / PlayerPrefs.GetInt("Blood");
    }
}