using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Text textEgg;
    [SerializeField] Text textDamege;
    [SerializeField] GameObject[] damageLV;
    [SerializeField] Text textSpeedShoot;
    [SerializeField] GameObject[] speedShootLV;
    [SerializeField] Text textBlood;
    [SerializeField] GameObject[] bloodLV;
    [SerializeField] Text costDamage;
    [SerializeField] Text costSpeedShoot;
    [SerializeField] Text costBlood;
    [SerializeField] GameObject loadingGame;
    [SerializeField] GameObject loadingIcon;
    int[] costDamegeUpdate;
    int[] costSpeedShootUpdate;
    int[] costBloodUpdate;
    
    private void Start()
    {
        costBloodUpdate = new int[] { 50, 100, 200, 300, 400, 700 };
        costDamegeUpdate =new int[] {50,150,350,650,950,1200};
        costSpeedShootUpdate = new int[] { 50, 250, 450, 750, 1000, 1350 };
      
        if (PlayerPrefs.GetInt("first") == 0)
        {
            loadingGameFirt();
        }
       
    }
    private void Update()
    {
        textEgg.text = ""+PlayerPrefs.GetInt("Egg");
        showDamage();
        showSpeedShoot();
        showBlood();
       
    }
    void loadingGameFirt()
    {
        PlayerPrefs.SetInt("Damage", 1);
        PlayerPrefs.SetInt("SpeedShoot", 5);
        PlayerPrefs.SetInt("Blood", 5);
        PlayerPrefs.SetInt("first", 1);
        PlayerPrefs.SetInt("Egg", 0);
        PlayerPrefs.Save();
    }
    void showDamage()
    {
        for(int i = 0; i< PlayerPrefs.GetInt("Damage"); i ++)
        {
            damageLV[i].SetActive(true);
        }
        textDamege.text = "" + PlayerPrefs.GetInt("Damage")+" Max";
        if (PlayerPrefs.GetInt("Damage")<6)
        {
            textDamege.text = "" + PlayerPrefs.GetInt("Damage");
        }
        costDamage.text = "" + costDamegeUpdate[PlayerPrefs.GetInt("Damage")-1];
        if (PlayerPrefs.GetInt("Egg") <= costDamegeUpdate[PlayerPrefs.GetInt("Damage") - 1])
        {
            costDamage.color = Color.red;
        }
        else
        {
            costDamage.color = Color.white;
        }

    }
    void showSpeedShoot()
    {
        for (int i = 0; i <6-PlayerPrefs.GetInt("SpeedShoot"); i++)
        {
            speedShootLV[i].SetActive(true);
        }
        textSpeedShoot.text = "" + PlayerPrefs.GetInt("SpeedShoot") + " Max";
        if (PlayerPrefs.GetInt("SpeedShoot") > 1)
        {
            textSpeedShoot.text = "" + PlayerPrefs.GetInt("SpeedShoot");
        }
        costSpeedShoot.text = "" + costSpeedShootUpdate[5 - PlayerPrefs.GetInt("SpeedShoot")];
        if (PlayerPrefs.GetInt("Egg") <= costSpeedShootUpdate[5-PlayerPrefs.GetInt("SpeedShoot")])
        {
            costSpeedShoot.color = Color.red;
        }
        else
        {
            costSpeedShoot.color = Color.white;
        }
    }
    void showBlood()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("Blood")/5; i ++)
        {
            bloodLV[i].SetActive(true);
        }
        textBlood.text = "" + PlayerPrefs.GetInt("Blood") + " Max";
        if (PlayerPrefs.GetInt("Blood")<30)
        {
            textBlood.text = "" + PlayerPrefs.GetInt("Blood");
        }
        costBlood.text = "" + costBloodUpdate[PlayerPrefs.GetInt("Blood")/5-1];
        if (PlayerPrefs.GetInt("Egg") <= costBloodUpdate[PlayerPrefs.GetInt("Blood") / 5 - 1])
        {
           costBlood.color = Color.red;
        }
        else
        {
            costBlood.color = Color.white;
        }
    }
    public void ButtonDamage()
    {
        if (PlayerPrefs.GetInt("Damage") < 6 && PlayerPrefs.GetInt("Egg") >= costDamegeUpdate[PlayerPrefs.GetInt("Damage")-1])
        {
            PlayerPrefs.SetInt("Egg", PlayerPrefs.GetInt("Egg") - costDamegeUpdate[PlayerPrefs.GetInt("Damage") - 1]);
            PlayerPrefs.SetInt("Damage", PlayerPrefs.GetInt("Damage") + 1);
            Debug.Log("Can Update");
            PlayerPrefs.Save();
        }
    }
    public void ButtonSpeed()
    {
        if (PlayerPrefs.GetInt("SpeedShoot") > 1 && PlayerPrefs.GetInt("Egg") >= costSpeedShootUpdate[5 - PlayerPrefs.GetInt("SpeedShoot")])
        {
            PlayerPrefs.SetInt("Egg", PlayerPrefs.GetInt("Egg") - costSpeedShootUpdate[5 - PlayerPrefs.GetInt("SpeedShoot")]);
            PlayerPrefs.SetInt("SpeedShoot", PlayerPrefs.GetInt("SpeedShoot") - 1);
            Debug.Log("Can Update");
            PlayerPrefs.Save();
        }
    }
    public void buttonBlood()
    {
        if (PlayerPrefs.GetInt("Blood") <30  && PlayerPrefs.GetInt("Egg") >= costBloodUpdate[PlayerPrefs.GetInt("Blood") / 5 - 1])
        {
            PlayerPrefs.SetInt("Egg", PlayerPrefs.GetInt("Egg") - costBloodUpdate[PlayerPrefs.GetInt("Blood") / 5 - 1]);
            PlayerPrefs.SetInt("Blood", PlayerPrefs.GetInt("Blood")+5);
            Debug.Log("Can Update");
        }
    }
    public void buttonPlay()
    {
        
        StartCoroutine(LoadGameScene());
    }
    IEnumerator LoadGameScene()
    {
        loadingGame.SetActive(true);
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        while (!async.isDone)
        {
            loadingIcon.GetComponent<RectTransform>().Rotate(new Vector3(0,0,90)*Time.deltaTime);
            yield return null;
        }
        if (async.isDone)
        {
            loadingGame.SetActive(false);
        }
    }
}
