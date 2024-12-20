using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject changeCar;
    [SerializeField] GameObject changemap;
    [SerializeField] GameObject next;
    [SerializeField] GameObject back;
    [SerializeField] GameObject play;
    [SerializeField] GameObject islock;
    [SerializeField] GameObject loadingGamePanner;
    [SerializeField] GameObject iconLoading;
    [SerializeField] Text TextKey;

    enum Page
    {
        ChangeCar, 
        ChangeMap
    }
    Page page;
    void Start()
    {
        page = Page.ChangeCar;
        PlayerPrefs.SetInt("UnlockedCar0", 1);
        PlayerPrefs.SetInt("UnlockedMap0", 1);
        PlayerPrefs.Save();
        
    }

    // Update is called once per frame
    void Update()
    {
        showPage();
    }

    bool checkUnlocked(string check)
    {
        if (PlayerPrefs.GetInt(check) == 1)
        {
            return true;
        }
        return false;
    }

    void showPage()
    {
        if(page == Page.ChangeCar)
        {
            changeCar.SetActive(true);
            changemap.SetActive(false);
            back.SetActive(false);
            play.SetActive(false);
            if (checkUnlocked("UnlockedCar"+PlayerPrefs.GetInt("CurrentCar").ToString()))
            {
                islock.SetActive(false);
                next.SetActive(true);
            }
            else
            {
                islock.SetActive(true) ;
                next.SetActive(false);
            }
            TextKey.text = PlayerPrefs.GetInt("key") + " key";
        }
        else
        {
            changeCar.SetActive(false);
            changemap.SetActive(true);
            next.SetActive(false);
            back.SetActive(true);
            if (checkUnlocked("UnlockedMap" + PlayerPrefs.GetInt("CurrentMap").ToString()))
            {
                islock.SetActive(false) ;
                play.SetActive(true);
            }
            else
            {
                islock.SetActive(true);
                play.SetActive(false);
            }
        }
        
    }
    public void unlockCarButton()
    {
        if (PlayerPrefs.GetInt("key") >= 1 && PlayerPrefs.GetInt("UnlockedCar" + PlayerPrefs.GetInt("CurrentCar").ToString())==0)
        {
            PlayerPrefs.SetInt("UnlockedCar" + PlayerPrefs.GetInt("CurrentCar").ToString(), 1);
            PlayerPrefs.SetInt("key", PlayerPrefs.GetInt("key") - 1);
            PlayerPrefs.Save();
        }
    }
    public void nextPageButton()
    {
        page = Page.ChangeMap;
    }
    public void backPageButton()
    {
        page = Page.ChangeCar;
    }
    public void PlayButton()
    {
        StartCoroutine(LoadingGame());
    }
    IEnumerator LoadingGame()
    {
        loadingGamePanner.SetActive(true);
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        while (!async.isDone)
        {
            iconLoading.GetComponent<RectTransform>().Rotate(new Vector3(0,0,-180)*Time.deltaTime);
            yield return null;
        }
    }
}
