using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject logo;
    [SerializeField] GameObject score1;
    [SerializeField] GameObject score2;
    [SerializeField] Text score;
    [SerializeField] Text bestScore;
    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(false);
       
    }
   
    // Update is called once per frame
    public void showMainMenu()
    {
        mainMenu.SetActive(true);
        score.text = "Score: " + SetGame.score;
        bestScore.text = "Best Score: "+ PlayerPrefs.GetInt("BestScore");
        score1.SetActive(false);
        score2.SetActive(false);
        logo.SetActive(false);
        showLogo();
        Invoke("showScore", 1);
    }
    public void RestStart() 
    {
        SceneManager.LoadScene(0);
    }
    void showLogo()
    {
        logo.SetActive(true);
    }
    void showScore()
    {
        score1.SetActive(true);
        score2.SetActive(true);
    }
}
