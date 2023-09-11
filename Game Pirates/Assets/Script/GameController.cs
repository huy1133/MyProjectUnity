using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject[] typeObstacle;
    [SerializeField] GameObject[] typeCoin;
    [SerializeField] GameObject mainMenu;

    Vector3 pointReSetMast;
    GameObject lastObstacle;
    int minDistance, maxDistance;

    float[] face = { -0.5f, 0.5f, -1, 1 };
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mast")
        {
            resetMast(collision.gameObject);
        }
        if(collision.gameObject.tag == "Ship")
        {
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Obstacle")
        {
            Destroy(collision.gameObject);
            resetObstacle();
        }
        if(collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
        }
    }
    void Start()
    {
        minDistance = 4;
        maxDistance = 10;
        pointReSetMast = new Vector3(0,5,0);
        createObstacleFirstTime(5);
        mainMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void createObstacleFirstTime(int number)
    {
        Vector3 currentPoint = new Vector3(0,10,0);
        for (int i = 0; i < number; i++){
            currentPoint += new Vector3(0, Random.Range(minDistance, maxDistance));
            int f = Random.Range(0, 2);
            GameObject tempGameObject = Instantiate(typeObstacle[Random.Range(0,typeObstacle.Length)]);
            tempGameObject.transform.position = currentPoint + Vector3.right * face[f];
            tempGameObject.transform.localScale = new Vector3(face[f + 2], 1, 1);
            lastObstacle = tempGameObject;
        }
        
    }
    void resetObstacle()
    {
        //create obstacle
        int distanceOf2Obscle = Random.Range(minDistance, maxDistance);//distance bettwen 2 obstacle
        int f = Random.Range(0, 2);//face of obstacle
        Vector3 newPointObstacle = new Vector3(0,lastObstacle.transform.position.y+distanceOf2Obscle,0);
        GameObject tempGameObject = Instantiate(typeObstacle[Random.Range(0, typeObstacle.Length)]);
        tempGameObject.transform.position = newPointObstacle + Vector3.right * face[f];
        tempGameObject.transform.localScale = new Vector3(face[f + 2], 1, 1);
        
        float numberCoin = Random.Range(0,distanceOf2Obscle);
        createCoin(numberCoin, f);

        lastObstacle = tempGameObject;
    }
    void createCoin(float n, int f)
    {
        for (float i = 1; i < n-1; i++)
        {
            GameObject tempCoin = Instantiate(typeCoin[0]);
            tempCoin.transform.position = new Vector3(face[f], lastObstacle.transform.position.y + i + 1, 0);
        }
    }
    void resetMast(GameObject gameObject)
    {
        gameObject.transform.position = pointReSetMast;
    }

    public void playButtonClick()
    {
        mainMenu.SetActive(false);
        setGame.setIsGame(true);
    }
}
