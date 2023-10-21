using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum gameStatic
{
    gameMenu,
    gameStar,
    gameOver
}

public class GameController : MonoBehaviour
{
    [SerializeField]GameObject Player;
    [SerializeField] GameObject obstacle;
    [SerializeField] Text textLerver;
    [SerializeField] Text textScore;
    [SerializeField] GameObject play;
    [SerializeField] GameObject playAgain;

    AudioSource audioSource;

    public gameStatic isGame;

    float width;
    float height;
    public int numberBoxCanBroken;
    List<Vector3> gatePoint = new List<Vector3>();
    List<Vector3> boxPoint = new List<Vector3>();
    List<Vector2> lerverGate = new List<Vector2>();
    int lerver;
    int difficult;
    int score;
    private void OnTriggerEnter(Collider other)
    {
        nextLerver();
        score++;
    }
    private void Start()
    {
        lerver = 0;
        score = 0;
        difficult = 2;
        createLerverGate();
        width = lerverGate[lerver].x;
        height = lerverGate[lerver].y;
        isGame = gameStatic.gameMenu;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        textLerver.text = "Lerver: " + (int)(lerver + 1);
        textScore.text = "" + score;
        if (isGame == gameStatic.gameOver)
        {
            playAgain.SetActive(true);
            Camera.main.transform.LookAt(new Vector3(0, 2, -11));
            Vector3 toMove = new Vector3(-4, 7, -17);
            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, toMove,Time.deltaTime);
        }
        else if(isGame == gameStatic.gameMenu)
        {
            playAgain.SetActive(false);
            textLerver.enabled = false;
            textScore.enabled = false;
        }
        else
        {
            playAgain.SetActive(false);
            textLerver.enabled = true;
            textScore.enabled = true;
            play.SetActive(false);
        }
        if(lerverGate.Count == lerver)
        {
            Debug.Log("het");
        }
    }
    void nextLerver()
    {
        if(difficult > (int)(width * height) / 1.5f)
        {
            lerver++;
            width = lerverGate[lerver].x;
            height = lerverGate[lerver].y;
            difficult = (int)(width * height) / 2;
        }

        numberBoxCanBroken = difficult;
        difficult++;

        createPoint();
        Player.GetComponent<PlayerController>().create(boxPoint);
        obstacle.GetComponent<ObstacleController>().Create(boxPoint, gatePoint);

        

    }
    void createLerverGate()
    {
        for(int i = 2; i<=5; i++)
        {
            for(int j = 2; j<=5; j++)
            {
                lerverGate.Add(new Vector2(j,i));
            }
        }
    }
    void createPoint()
    {
        gatePoint.Clear();
        boxPoint.Clear();
        for (float i = -((width + 1) / 2); i <= (width + 1) / 2; i++)
        {
            for (float j = 0; j <= height; j++)
            {
                if (i == -((width + 1) / 2) || i == (width + 1) / 2 || j == height)
                {
                    gatePoint.Add(new Vector3(i, j, 0));
                }
                else
                {
                    boxPoint.Add(new Vector3(i, j, 0));
                }
            }
        }
    }
    public void playButton()
    {
        isGame = gameStatic.gameStar;
        nextLerver();
    }
    public void playAgainButton() 
    {
        SceneManager.LoadScene(0);
    }
    public void playAudioDestroy()
    {
        audioSource.Play();
    }
}
