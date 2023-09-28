using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class GameController : MonoBehaviour
{
    [SerializeField] Text eggText;
    [SerializeField] Text eggTextGameOver;
    [SerializeField] Slider BloddSlider;
    [SerializeField] GameObject overGamePanel;
   
    public int eggate;
    float blood;

    void Start()
    {
        eggate = 0;
        blood = PlayerPrefs.GetInt("Blood");
    }

    // Update is called once per frame
    void Update()
    {
        UI();
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
            overGamePanel.SetActive(true);
            eggTextGameOver.text = "" + eggate;
        }
    }
    private void UI()
    {
        eggText.text = "" + eggate;
        BloddSlider.value = (float)blood / PlayerPrefs.GetInt("Blood");
    }
    public void clamButton()
    {
        PlayerPrefs.SetInt("Egg", PlayerPrefs.GetInt("Egg") + eggate);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
   
}
