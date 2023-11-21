using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController1 : MonoBehaviour
{
    [SerializeField] GameObject[] police;
    [SerializeField] Transform target;
    [SerializeField] Text timePlay;
    [SerializeField] Image[] star;
    [SerializeField] Sprite[] imgStar;
    [SerializeField] Slider slider;
    [SerializeField] GameObject overGamePaner;
    [SerializeField] Text textTimeToReStart;
    [SerializeField] GameObject loadingPanner;
    [SerializeField] GameObject iconLoading;

    List<GameObject> policeList = new List<GameObject>();
    
    int Star;
    float time;
    int numberPolice;
    float timeBorn;
    float timeCanBorn;
    bool isGame;
    float timeStartCount;
    float timeIncreaseStar;
    float timeToReStar;
    void Start()
    {
        createPolice();
        timeToReStar = 5;
        Star = 0;
        time = 0;
        numberPolice = 5;
        timeBorn = 0;
        timeCanBorn = 0.5f;
        
        timeStartCount = 0;
        timeIncreaseStar = 30;
    }

    private void Update()
    {
        isGame = GameObject.FindWithTag("Player").GetComponent<CarController1>().canMove;
        bornPolice();
        showUI();
        
        if(isGame)
        {
            time += Time.deltaTime;
            timeStartCount += Time.deltaTime;
        }
        if (timeBorn < timeCanBorn)
        {
            timeBorn += Time.deltaTime;
        }
    }
    
    void bornPolice()
    {
       
        int timeReducePolice = 10;
        int temp = numberPolice + (int)time/timeReducePolice ;
        temp = Mathf.Clamp(temp, 0, 20);
        for(int i=0; i<temp; i++)
        {
            if (!policeList[i].activeSelf&&timeBorn>=timeCanBorn)
            {
                rebornPolice(policeList[i]);
                timeBorn = 0;
                break;
            }
        }
    }
    void rebornPolice(GameObject tempGameObject)
    {
        Vector3 point =target.forward.normalized * 150;
        point.y = 0;
        tempGameObject.transform.position = point;
        tempGameObject.transform.forward = target.position - point;
        tempGameObject.SetActive(true);
    }
    void createPolice()
    {
        int numberPolice = 7;
        for (int i = 0; i < police.Length; i++)
        {
            for (int j = 0; j < numberPolice; j++)
            {
                GameObject temp;
                if (i == 0)
                {
                    temp = Instantiate(police[i], new Vector3(Random.Range(-1f,1)*10, 0, (7+10*j)*-1), Quaternion.identity);
                    temp.SetActive(true);
                }
                else
                {
                    temp = Instantiate(police[i], new Vector3(0, 0, 0), Quaternion.identity);
                    temp.SetActive(false);
                }
                temp.GetComponent<PoliceAuto>().playerPointTarget = target;
                
                policeList.Add(temp);
            }
        }
    }
    void showUI()
    {
        for(int i=0; i< star.Length; i++)
        {
            if (i < Star)
            {
                star[i].sprite = imgStar[1];
            }
            else
            {
                star[i].sprite = imgStar[0];
            }
        }
        
        
        timePlay.text = "" + (int)time+"s";
        if (timeStartCount / timeIncreaseStar >= 1)
        {
            Star++;
            timeStartCount = 0;
        }
        slider.value = timeStartCount / timeIncreaseStar;
        if(!isGame)
        {
            overGamePaner.SetActive(true);
            timeToReStar -= Time.deltaTime;
            string leverstr = "LeverMap" + PlayerPrefs.GetInt("CurrentMap");
            if (Star > PlayerPrefs.GetInt(leverstr))
            {
                PlayerPrefs.SetInt(leverstr, Star);
                PlayerPrefs.Save();
            }
            textTimeToReStart.text = "Start Again In " + (int)timeToReStar + "s";
            if (timeToReStar < 0)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
    public void exitToMenuButton()
    {
        StartCoroutine(LoadingGame());
    }
    IEnumerator LoadingGame()
    {
        loadingPanner.SetActive(true);
        AsyncOperation async = SceneManager.LoadSceneAsync(0);
        while (async.isDone) 
        {
            iconLoading.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 90) * Time.deltaTime);
            yield return null;
        }
    }
}
