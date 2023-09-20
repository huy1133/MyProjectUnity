using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject logo;
    [SerializeField] GameObject score1;
    [SerializeField] GameObject score2;
    [SerializeField] GameObject restar;
    [SerializeField] Text score;
    [SerializeField] Text bestScore;
    // Start is called before the first frame update
    void Start()
    {
        score1.SetActive(false);
        score2.SetActive(false);
        logo.SetActive(false);
        restar.SetActive(false);
    }
   
    // Update is called once per frame
    public void showMainMenu()
    {
        logo.SetActive(true);
        Invoke("showScore", 1);
        score.text = "Score: " + SetGame.score;
        bestScore.text = "Best Score: "+ PlayerPrefs.GetInt("BestScore");
        
    }
    public void RestStart() 
    {
        SceneManager.LoadScene(0);
    }
    void showScore()
    {
        score1.SetActive(true);
        score2.SetActive(true);
        restar.SetActive(true);
    }
}
