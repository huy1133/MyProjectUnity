using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] Text textQuez, textTime, enemyTalk;
    [SerializeField] Text[] textOpition;
    [SerializeField] Slider blood;
    [SerializeField] GameObject[] levels;
    List<List<EnglishWord>> vocabularys =  new List<List<EnglishWord>>();
    List<EnglishWord> currentVocabulary = new List<EnglishWord>();
    bool isStar;
    bool isEnd;
    bool isNext;
    float timeCountdown;
    bool isCountDown;
    int currentAnswer;
    int correct;
    int valuesBloos;
    bool canChoose;
    int currentLevel;
    private void Start()
    {
        isStar = false;
        isEnd = false;
        isNext = true;
        isCountDown = false;
        canChoose = true;
        currentAnswer = 0;
        createVocabulary();
        updateLevel();
    }
    private void Update()
    {
        
        if (isStar)
        {
            isGame();
        }
        if (isEnd)
        {
            isEnd = false;
            GameObject.Find("ScreensManager").GetComponent<ScreensManager>().changeScreens(Screens.EndGame);
        }
        
    }
    void updateLevel()
    {
        //PlayerPrefs.SetInt("" + currentLevel, 0);
        //PlayerPrefs.Save();
        for (int i = 0; i < levels.Length; i++)
        {
            if(PlayerPrefs.GetInt(""+i) == 1)
            {

                levels[i].GetComponent<Image>().color = Color.yellow;
            }
        }
        
      
    }
    void winGame()
    {
        enemyTalk.text = "You Win";
        isStar = false;
        isEnd = true;
        PlayerPrefs.SetInt(""+currentLevel, 1);
        PlayerPrefs.Save();
    }
    void isGame()
    {
        if (isCountDown)
        {
            timeCountdown -= Time.deltaTime;
        }
        textTime.text = "" + (int)(timeCountdown) + "s";
        if (timeCountdown > 0) 
        {
            if (isNext)
            {
                canChoose = true;
                isNext = false;
                timeCountdown = 15;
                isCountDown=true;
                int[] t = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    t[i] = UnityEngine.Random.Range(0, currentVocabulary.Count);
                    for(int j = 0; j<i; j++)
                    {
                        if (t[i] == t[j])
                        {
                            i -= 1;
                            break;
                        }
                    }
                    i = t[i] == currentAnswer ? i - 1 : i;
                }
                correct = UnityEngine.Random.Range(0, 4);
                t[correct] = currentAnswer;
                textQuez.text = currentVocabulary[currentAnswer].getWord()+" "+ currentVocabulary[currentAnswer].getPronunciation()+" "+ currentVocabulary[currentAnswer].getWordType();
                for (int i = 0; i < 4; i++)
                {
                    textOpition[i].text = "" + currentVocabulary[t[i]].getVietnameseTranslation();
                }
                currentAnswer++;
            }
        }
        else
        {
            isStar = false;
            isEnd = true;
            enemyTalk.text = "You Loss";
        }
        
    }
    void waitNext()
    {
        isNext = true;
        
    }
    void waitCharacterAttact()
    {
        
        blood.value -= (float)1 / valuesBloos;
        if (currentAnswer == currentVocabulary.Count - 1)
        {
            winGame();
        }
        else
        {
            Invoke("waitNext", 3f);
        }
    }
    void waitEnemyAttact()
    {
        isEnd = true;
    }
    void correctAnswer()
    {
        
        float timeBombExplore = 1.5f;
        GameObject.Find("Player").GetComponent<LayerController>().PlayerAttack(timeBombExplore);
        Invoke("waitCharacterAttact", timeBombExplore);
    }
    void notCorrectAnswer()
    {
        enemyTalk.text = "You Loss";
        float timeBombExplore = 2.7f;
        GameObject.Find("Player").GetComponent<LayerController>().PlayerAttack(timeBombExplore);
        GameObject.Find("Enemy").GetComponent<Enemy>().enemyAttack();
        Invoke("waitEnemyAttact", timeBombExplore+1);
    }
    public void chooseAnswer(int n)
    {
        isCountDown = false;
        if (canChoose)
        {
            if (n == correct)
            {
                correctAnswer();
            }
            else
            {
                notCorrectAnswer();
            }
            canChoose = false;
        }
        
    }
    public void Option(int option)
    {
        switch (option)
        {
            case -1:
                GameObject.Find("ScreensManager").GetComponent<ScreensManager>().backScreen();
                break;
            case 0:
                GameObject.Find("ScreensManager").GetComponent<ScreensManager>().changeScreens(Screens.Menu);
                break;
            case 1:
                GameObject.Find("ScreensManager").GetComponent<ScreensManager>().changeScreensOfMenu(0);
                break;
            case 2:
                GameObject.Find("ScreensManager").GetComponent<ScreensManager>().changeScreensOfMenu(1);
                break;
            case 3:
                GameObject.Find("ScreensManager").GetComponent<ScreensManager>().changeScreensOfMenu(2);
                break;
        }
    }
    public void optionScreensPlay(int option)
    {
        currentLevel = option;
        GameObject.Find("ScreensManager").GetComponent<ScreensManager>().changeScreens(Screens.StatrGame);
        currentVocabulary = vocabularys[option];
        timeCountdown = currentVocabulary.Count;
        isStar = true;
        valuesBloos = currentVocabulary.Count;
    }
    void createVocabulary()
    {
        List<EnglishWord> voca = new List<EnglishWord>
        {
            new EnglishWord("undergo", "trải qua", "/ˌʌndərˈɡoʊ/", "(v)"),
            new EnglishWord("renovation", "sự tu sửa", "/ˌrɛnəˈveɪʃən/", "(n)"),
            new EnglishWord("amend", "sửa đổi", "/əˈmɛnd/", "(v)"),
            new EnglishWord("maintenance", "bảo dưỡng", "/ˈmeɪntənəns/", "(n)"),
            new EnglishWord("method", "phương pháp", "/ˈmɛθəd/", "(n)"),
            new EnglishWord("propose", "đề xuất", "/prəˈpoʊz/", "(v)"),
            new EnglishWord("technique", "kỹ thuật", "/tɛkˈnik/", "(n)"),
            new EnglishWord("approach", "tiếp cận", "/əˈproʊtʃ/", "(v)"),
            new EnglishWord("suggest", "gợi ý", "/səˈdʒɛst/", "(v)"),
            new EnglishWord("several", "một số", "/ˈsɛvrəl/", "(det)"),
            new EnglishWord("manufacturer", "nhà sản xuất", "/ˌmænjuˈfæktʃərər/", "(n)"),
            new EnglishWord("sculptures", "điêu khắc", "/ˈskʌlptʃərz/", "(n)"),
            new EnglishWord("accomplish", "đạt được", "/əˈkɑmplɪʃ/", "(v)"),
            new EnglishWord("achieve", "đạt được", "/əˈtʃiv/", "(v)"),
            new EnglishWord("reach", "đạt được", "/riːʧ/", "(v)"),
            new EnglishWord("exhibition", "triển lãm", "/ˌɛksəˈbɪʃən/", "(n)"),
            new EnglishWord("quality", "chất lượng", "/ˈkwɑləti/", "(n)"),
            new EnglishWord("requirement", "yêu cầu", "/rɪˈkwaɪrmənt/", "(n)"),
            new EnglishWord("comply", "tuân theo", "/kəmˈplaɪ/", "(v)"),
            new EnglishWord("result", "kết quả", "/rɪˈzʌlt/", "(n)"),
            new EnglishWord("compensate", "bồi thường", "/ˈkɑmpənˌseɪt/", "(v)"),
            new EnglishWord("offset", "bù đắp", "/ˈɔfˌsɛt/", "(v)"),
            new EnglishWord("ease", "giảm", "/iz/", "(v)"),
            new EnglishWord("congestion", "tắc nghẽn", "/kənˈdʒɛstʃən/", "(n)"),
            new EnglishWord("delegate", "ủy quyền", "/ˈdɛləˌɡeɪt/", "(v)"),
            new EnglishWord("present", "trình bày", "/prɪˈzɛnt/", "(v)"),
            new EnglishWord("charge", "tính phí", "/ʧɑrdʒ/", "(v)"),
            new EnglishWord("versatile", "đa năng", "/ˈvɜrsətɪl/", "(adj)"),
            new EnglishWord("opposition", "sự phản đối", "/ˌɑpəˈzɪʃən/", "(n)"),
            new EnglishWord("guarantee", "đảm bảo", "/ˌɡærənˈti/", "(v)"),
            new EnglishWord("certificate", "giấy chứng nhận", "/sərˈtɪfɪkət/", "(n)"),
        };
        vocabularys.Add(voca);
        voca = new List<EnglishWord>
        {
            new EnglishWord("increase", "tăng", "/ˈɪnˌkris/", "(v)"),
            new EnglishWord("profits", "lợi nhuận", "/ˈprɒfɪts/", "(n)"),
            new EnglishWord("profitable", "có lợi nhuận", "/ˈprɒfɪtəbəl/", "(adj)"),
            new EnglishWord("lucrative", "sinh lợi", "/ˈluːkrətɪv/", "(adj)"),
            new EnglishWord("reduce", "giảm", "/rɪˈdus/", "(v)"),
            new EnglishWord("cost", "chi phí", "/kɒst/", "(n)"),
            new EnglishWord("staff", "nhân viên", "/stæf/", "(n)"),
            new EnglishWord("productivity", "năng suất", "/ˌprɒdʌkˈtɪvɪti/", "(n)"),
            new EnglishWord("staff productivity", "năng suất nhân viên", "/stæf ˌprɒdʌkˈtɪvɪti/", "(n)"),
            new EnglishWord("proposal", "đề xuất", "/prəˈpəʊzl/", "(n)"),
            new EnglishWord("stimulate", "kích thích", "/ˈstɪmjʊˌleɪt/", "(v)"),
            new EnglishWord("encourage", "khuyến khích", "/ɪnˈkʌrɪʤ/", "(v)"),
            new EnglishWord("supervisor", "giám sát viên", "/ˈsuːpəˌvaɪzər/", "(n)"),
            new EnglishWord("supervise", "giám sát", "/ˈsuːpəˌvaɪz/", "(v)"),
            new EnglishWord("oversee", "trông nom", "/ˌoʊvərˈsi/", "(v)"),
            new EnglishWord("feature", "đặc điểm", "/ˈfiːʧər/", "(n)"),
            new EnglishWord("procedure", "thủ tục", "/prəˈsiːʤər/", "(n)"),
            new EnglishWord("construction", "xây dựng", "/kənˈstrʌkʃən/", "(n)"),
            new EnglishWord("resident", "cư dân", "/ˈrɛzɪdənt/", "(n)"),
            new EnglishWord("representative", "đại diện", "/ˌrɛprɪˈzɛntətɪv/", "(n)"),
            new EnglishWord("patient", "bệnh nhân", "/ˈpeɪʃənt/", "(n)"),
            new EnglishWord("consultation", "tư vấn", "/ˌkɒnsəlˈteɪʃən/", "(n)"),
            new EnglishWord("consult", "tư vấn", "/kənˈsʌlt/", "(v)"),
            new EnglishWord("purpose", "mục đích", "/ˈpɜːpəs/", "(n)"),
            new EnglishWord("operation", "hoạt động", "/ˌɒpəˈreɪʃən/", "(n)"),
            new EnglishWord("operate", "vận hành", "/ˈɒpəreɪt/", "(v)"),
            new EnglishWord("deal with", "đối phó với", "/diːl wɪð/", "(phr)"),
            new EnglishWord("cope with", "đối phó với", "/kəʊp wɪð/", "(phr)"),
            new EnglishWord("solve", "giải quyết", "/sɒlv/", "(v)"),
            new EnglishWord("distribute", "phân phát", "/dɪˈstrɪbjuːt/", "(v)"),
            new EnglishWord("distribution", "phân phối", "/ˌdɪstrɪˈbjuːʃən/", "(n)"),
            new EnglishWord("distributor", "nhà phân phối", "/dɪˈstrɪbjətər/", "(n)"),
            new EnglishWord("approve", "chấp thuận", "/əˈpruːv/", "(v)"),
            new EnglishWord("approval", "sự chấp thuận", "/əˈpruːvəl/", "(n)"),
            new EnglishWord("physician", "bác sĩ", "/fɪˈzɪʃən/", "(n)"),
        };
        vocabularys.Add(voca);
    }

}
