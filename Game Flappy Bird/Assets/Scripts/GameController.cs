
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [SerializeField] GameObject enemyType;
    [SerializeField] GameObject[] Score;
    [SerializeField] Sprite[] number;
    [SerializeField] GameObject[] birdType;
    GameObject lastEnemy;
    int score;
    
    // Start is called before the first frame update
    void Start()
    {
        SetGame.gameStar = false;
        SetGame.gameOver = false;
        createFirstEnemy(5,10);
        score = 0;
        if (PlayerPrefs.GetInt("Skin1") == 0) PlayerPrefs.SetInt("Skin1", 1);
        GameObject bird = Instantiate(birdType[PlayerPrefs.GetInt("NowSkin")]);
        SetGame.speed = 0.025f;
    }

    // Update is called once per frame
    void Update()
    {
        showNumber(score.ToString().Length);

        Debug.Log(SetGame.speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "score")
        {
            createNewEnemy();
        }
        Destroy(collision.gameObject);
    }
    void createNewEnemy()
    {
        Vector3 tempPosition = lastEnemy.transform.position;
        Vector3 NewPositionEnemy = new Vector3(tempPosition.x + Random.Range(5, 10), (float)Random.Range(-2.0f, 2.1f), 0);
        GameObject tempGameObject = Instantiate(enemyType, NewPositionEnemy, Quaternion.identity);
        lastEnemy = tempGameObject;
    }
    void createFirstEnemy(int number, int firstPointX)
    {
        for(int i = 0; i < number; i++)
        {
            GameObject temp = Instantiate(enemyType, new Vector3(firstPointX,(float)Random.Range(-2.0f,2.1f),0),Quaternion.identity);
            firstPointX += Random.Range(5, 10);
            lastEnemy = temp;
        }
    }
    void showNumber(int n)
    {
        for(int i=0; i < n; i++)
        {
            int temp = (int)(score% Mathf.Pow(10, n-i) / Mathf.Pow(10, n-i-1));
            Score[i].SetActive(true);
            Score[i].GetComponent<SpriteRenderer>().sprite = number[temp];
        }
    }
    public void plusScore()
    {
        score++;
        if (score % 5 == 0)
        {
            SetGame.speed += 0.005f;
        }
        SetGame.score = score;
    }
   
}
