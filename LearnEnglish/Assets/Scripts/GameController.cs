using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;

public class GameController : MonoBehaviour
{
    [SerializeField] Text textQuez, textTime, enemyTalk;
    [SerializeField] Text[] textOpition;
    [SerializeField] Slider blood;
    [SerializeField] GameObject[] levels;
    [SerializeField] GameObject[] questions;
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
    int timeShowAnswer;
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
                correct = -1;
                for (int i = 0; i < 4; i++)
                {
                    t[i] = UnityEngine.Random.Range(0, currentVocabulary.Count);
                    if (currentVocabulary[t[i]].getVietnameseTranslation() == currentVocabulary[currentAnswer].getVietnameseTranslation())
                    {
                        correct = i;
                    }
                    for(int j = 0; j<i; j++)
                    {
                        if (currentVocabulary[t[i]].getVietnameseTranslation() == currentVocabulary[t[j]].getVietnameseTranslation())
                        {
                            i -= 1;
                            break;
                        }
                    }
                }
                if (correct == -1)
                {
                    correct = UnityEngine.Random.Range(0, 4);
                    t[correct] = currentAnswer;
                }
                textQuez.text = currentVocabulary[currentAnswer].getWord()+" "+ currentVocabulary[currentAnswer].getPronunciation() + " "+ currentVocabulary[currentAnswer].getWordType();
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
        timeShowAnswer = 9;
        showCorrectAnswer();
    }
    void showCorrectAnswer()
    {
        if (timeShowAnswer>0)
        {
            bool sae = timeShowAnswer--%2==0 ? false : true;
            questions[correct].gameObject.SetActive(sae);
            Invoke("showCorrectAnswer", 0.1f);
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
        vocabularys.Add(
        new List<EnglishWord>
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
        });
        vocabularys.Add(
        new List<EnglishWord>
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
        });
        vocabularys.Add(
        new List<EnglishWord>
        {
            new EnglishWord("hire", "thuê", "/haɪər/", "(v)"),
            new EnglishWord("familiar with", "quen thuộc với", "/fəˈmɪliər wɪð/", "(adj)"),
            new EnglishWord("shift", "ca làm việc", "/ʃɪft/", "(n)"),
            new EnglishWord("remote", "xa, hẻo lánh", "/rɪˈmoʊt/", "(adj)"),
            new EnglishWord("strict", "nghiêm ngặt", "/strɪkt/", "(adj)"),
            new EnglishWord("judge", "đánh giá", "/ʤʌʤ/", "(v)"),
            new EnglishWord("evaluate", "đánh giá", "/ɪˈvæljueɪt/", "(v)"),
            new EnglishWord("process", "quy trình", "/ˈprɑsɛs/", "(n)"),
            new EnglishWord("sufficient", "đủ", "/səˈfɪʃənt/", "(adj)"),
            new EnglishWord("adequate", "đủ", "/ˈædɪkwət/", "(adj)"),
            new EnglishWord("entire", "toàn bộ", "/ɪnˈtaɪər/", "(adj)"),
            new EnglishWord("whole", "toàn bộ", "/hoʊl/", "(adj)"),
            new EnglishWord("assemble", "lắp ráp", "/əˈsɛmbəl/", "(v)"),
            new EnglishWord("potential", "tiềm năng", "/pəˈtɛnʃəl/", "(adj)"),
            new EnglishWord("purchase", "mua", "/ˈpɜrʧəs/", "(v)"),
        });
        vocabularys.Add(
        new List<EnglishWord>
        {
            new EnglishWord("substantially", "một cách đáng kể", "/səbˈstænʃəli/", "(adv)"),
            new EnglishWord("considerably", "một cách đáng kể", "/kənˈsɪdərəbli/", "(adv)"),
            new EnglishWord("during", "trong thời gian", "/ˈdjʊrɪŋ/", "(prep)"),
            new EnglishWord("the peak season", "mùa cao điểm", "/ðə piːk ˈsiːzən/", "(n)"),
            new EnglishWord("criticize", "phê phán", "/ˈkrɪtɪˌsaɪz/", "(v)"),
            new EnglishWord("short discussion", "cuộc thảo luận ngắn", "/ʃɔrt dɪˈskʌʃən/", "(n)"),
            new EnglishWord("effectively", "một cách hiệu quả", "/ɪˈfɛktɪvli/", "(adv)"),
            new EnglishWord("lengthen", "làm dài ra", "/ˈlɛŋθən/", "(v)"),
            new EnglishWord("individual", "cá nhân", "/ˌɪndɪˈvɪdʒuəl/", "(n)"),
            new EnglishWord("organization", "tổ chức", "/ˌɔːrɡənaɪˈzeɪʃən/", "(n)"),
            new EnglishWord("expense", "chi phí", "/ɪkˈspɛns/", "(n)"),
            new EnglishWord("be responsible for", "chịu trách nhiệm về", "/bi rɪˈspɒnsəbl fɔːr/", "(phr)"),
            new EnglishWord("technical support", "hỗ trợ kỹ thuật", "/ˈtɛknɪkəl səˈpɔːrt/", "(n)"),
            new EnglishWord("available", "có sẵn", "/əˈveɪləbl/", "(adj)"),
            new EnglishWord("aid", "viện trợ", "/eɪd/", "(n)"),
            new EnglishWord("assist", "hỗ trợ", "/əˈsɪst/", "(v)"),
            new EnglishWord("exclusively", "duy nhất", "/ɪkˈskluːsɪvli/", "(adv)"),
            new EnglishWord("solve the problem", "giải quyết vấn đề", "/sɒlv ðə ˈprɒbləm/", "(phr)"),
            new EnglishWord("enclosed manual", "hướng dẫn kèm theo", "/ɪnˈkləʊzd ˈmænjʊəl/", "(phr)"),
            new EnglishWord("implement", "thực hiện", "/ˈɪmplɪmənt/", "(v)"),
            new EnglishWord("carry out", "thực hiện", "/ˈkæri aʊt/", "(phr)"),
            new EnglishWord("conduct", "tiến hành", "/ˈkɒndʌkt/", "(v)"),
            new EnglishWord("thoroughly", "kỹ lưỡng", "/ˈθʌrəli/", "(adv)"),
        });
        vocabularys.Add(
        new List<EnglishWord>
        {
            new EnglishWord("decade", "thập kỷ", "/ˈdɛkeɪd/", "(n)"),
            new EnglishWord("concern", "quan tâm", "/kənˈsɜrn/", "(n)"),
            new EnglishWord("interest rates", "lãi suất", "/ˈɪntrəst reɪts/", "(n)"),
            new EnglishWord("industry", "ngành công nghiệp", "/ˈɪndəstri/", "(n)"),
            new EnglishWord("avoid", "tránh", "/əˈvɔɪd/", "(v)"),
            new EnglishWord("defective", "có lỗi", "/dɪˈfɛktɪv/", "(adj)"),
            new EnglishWord("merchandise", "hàng hoá", "/ˈmɜrtʃəndaɪz/", "(n)"),
            new EnglishWord("defective merchandise", "hàng hoá lỗi", "/dɪˈfɛktɪv ˈmɜrtʃəndaɪz/", "(n)"),
            new EnglishWord("take out a loan", "vay mượn", "/teɪk aʊt ə loʊn/", "(phr)"),
            new EnglishWord("temporary", "tạm thời", "/ˈtɛmpəˌrɛri/", "(adj)"),
            new EnglishWord("solution", "giải pháp", "/səˈluʃən/", "(n)"),
            new EnglishWord("financial problem", "vấn đề tài chính", "/faɪˈnænʃəl ˈprɒbləm/", "(n)"),
            new EnglishWord("finance", "tài chính", "/ˈfaɪˌnæns/", "(n)"),
            new EnglishWord("a revised facilities", "cơ sở đã được sửa đổi", "/ə rɪˈvaɪzd fəˈsɪlɪtiz/", "(n)"),
            new EnglishWord("examine", "kiểm tra", "/ɪɡˈzæmɪn/", "(v)"),
            new EnglishWord("retire", "nghỉ hưu", "/rɪˈtaɪər/", "(v)"),
            new EnglishWord("adapt", "thích nghi", "/əˈdæpt/", "(v)"),
            new EnglishWord("committed", "cam kết", "/kəˈmɪtɪd/", "(adj)"),
            new EnglishWord("ability", "khả năng", "/əˈbɪləti/", "(n)"),
            new EnglishWord("realize", "nhận ra", "/ˈriəˌlaɪz/", "(v)"),
            new EnglishWord("investment", "đầu tư", "/ɪnˈvɛstmənt/", "(n)"),
            new EnglishWord("bankrupt", "phá sản", "/ˈbæŋkrʌpt/", "(adj)"),
            new EnglishWord("inflation", "lạm phát", "/ɪnˈfleɪʃən/", "(n)"),
        });
        vocabularys.Add(
        new List<EnglishWord>
        {
            new EnglishWord("initial", "ban đầu", "/ɪˈnɪʃəl/", "(adj)"),
            new EnglishWord("allocate", "phân bổ", "/ˈæləˌkeɪt/", "(v)"),
            new EnglishWord("anticipate", "dự đoán", "/ænˈtɪsəˌpeɪt/", "(v)"),
            new EnglishWord("express gratitude", "bày tỏ lòng biết ơn", "/ɪkˈsprɛs ˈɡrætɪˌtud/", "(phr)"),
            new EnglishWord("valid", "hợp lệ", "/ˈvælɪd/", "(adj)"),
            new EnglishWord("negotiation", "đàm phán", "/nɪˌɡoʊʃiˈeɪʃən/", "(n)"),
            new EnglishWord("consistently", "một cách nhất quán", "/kənˈsɪstəntli/", "(adv)"),
            new EnglishWord("high profile", "nổi bật", "/haɪ ˈproʊˌfaɪl/", "(adj)"),
            new EnglishWord("throughout", "xuyên suốt", "/θruˈaʊt/", "(prep)"),
            new EnglishWord("conduct", "tiến hành", "/ˈkɒndʌkt/", "(v)"),
            new EnglishWord("diagnose", "chẩn đoán", "/ˈdaɪəɡˌnoʊs/", "(v)"),
            new EnglishWord("poll", "cuộc thăm dò", "/poʊl/", "(n)"),
            new EnglishWord("exceed expectation", "vượt qua mong đợi", "/ɪkˈsid ˌɛkspɛkˈteɪʃən/", "(phr)"),
            new EnglishWord("function", "chức năng", "/ˈfʌŋkʃən/", "(n)"),
            new EnglishWord("permit", "cho phép", "/pərˈmɪt/", "(v)"),
            new EnglishWord("precise", "chính xác", "/prɪˈsaɪs/", "(adj)"),
            new EnglishWord("extend", "mở rộng", "/ɪkˈstɛnd/", "(v)"),
            new EnglishWord("expand", "mở rộng", "/ɪkˈspænd/", "(v)"),
            new EnglishWord("widen", "mở rộng", "/ˈwaɪdən/", "(v)"),
            new EnglishWord("identify", "nhận dạng", "/aɪˈdɛntɪˌfaɪ/", "(v)"),
        });
        vocabularys.Add(
        new List<EnglishWord>
        {
            new EnglishWord("award", "giải thưởng", "/əˈwɔrd/", "(n)"),
            new EnglishWord("grant", "phong", "/ɡrænt/", "(v)"),
            new EnglishWord("non-profit", "phi lợi nhuận", "/nɒn ˈprɒfɪt/", "(adj)"),
            new EnglishWord("organization", "tổ chức", "/ˌɔːrɡənaɪˈzeɪʃən/", "(n)"),
            new EnglishWord("issue", "vấn đề", "/ˈɪʃuː/", "(n)"),
            new EnglishWord("satisfaction", "sự hài lòng", "/ˌsætɪsˈfækʃən/", "(n)"),
            new EnglishWord("satisfied", "hài lòng", "/ˈsætɪsfaɪd/", "(adj)"),
            new EnglishWord("related to", "liên quan đến", "/rɪˈleɪtɪd tuː/", "(phr)"),
            new EnglishWord("concerning", "liên quan đến", "/kənˈsɜrnɪŋ/", "(prep)"),
            new EnglishWord("regarding", "liên quan đến", "/rɪˈɡɑrdɪŋ/", "(prep)"),
            new EnglishWord("complete", "hoàn thành", "/kəmˈpliːt/", "(v)"),
            new EnglishWord("entire", "toàn bộ", "/ɪnˈtaɪər/", "(adj)"),
            new EnglishWord("rapidly", "một cách nhanh chóng", "/ˈræpɪdli/", "(adv)"),
            new EnglishWord("tend to", "có khuynh hướng", "/tɛnd tuː/", "(v)"),
            new EnglishWord("interior", "bên trong", "/ɪnˈtɪriər/", "(n)"),
            new EnglishWord("lobby", "sảnh lớn", "/ˈlɑːbi/", "(n)"),
            new EnglishWord("recipe", "công thức nấu ăn", "/ˈrɛsəpi/", "(n)"),
            new EnglishWord("login credentials", "thông tin đăng nhập", "/ˈlɔɡɪn krɪˈdɛnʃlz/", "(n)"),
            new EnglishWord("submission", "sự nộp", "/səbˈmɪʃən/", "(n)"),
            new EnglishWord("submit", "nộp", "/səbˈmɪt/", "(v)"),
            new EnglishWord("intact", "nguyên vẹn", "/ɪnˈtækt/", "(adj)"),
            new EnglishWord("inappropriate", "không thích hợp", "/ˌɪnəˈproʊpriɪət/", "(adj)"),
            new EnglishWord("appropriate", "thích hợp", "/əˈproʊpriət/", "(adj)"),
            new EnglishWord("handle", "xử lý", "/ˈhændl/", "(v)"),
        });
        vocabularys.Add(
        new List<EnglishWord>
        {
            new EnglishWord("encourage", "khuyến khích", "/ɪnˈkɜrɪdʒ/", "(v)"),
            new EnglishWord("stimulate", "kích thích", "/ˈstɪmjʊˌleɪt/", "(v)"),
            new EnglishWord("display", "trưng bày", "/dɪˈspleɪ/", "(v)"),
            new EnglishWord("initiative", "sáng kiến", "/ɪˈnɪʃətɪv/", "(n)"),
            new EnglishWord("evident", "rõ ràng", "/ˈɛvɪdənt/", "(adj)"),
            new EnglishWord("alternative", "lựa chọn", "/ɔlˈtɜrnətɪv/", "(n)"),
            new EnglishWord("option", "tùy chọn", "/ˈɒpʃən/", "(n)"),
            new EnglishWord("facility", "cơ sở vật chất", "/fəˈsɪlɪti/", "(n)"),
            new EnglishWord("additional", "bổ sung", "/əˈdɪʃənl/", "(adj)"),
            new EnglishWord("effect", "hiệu ứng", "/ɪˈfɛkt/", "(n)"),
            new EnglishWord("avoid", "tránh", "/əˈvɔɪd/", "(v)"),
            new EnglishWord("prevent", "ngăn chặn", "/prɪˈvɛnt/", "(v)"),
            new EnglishWord("postpone", "hoãn lại", "/poʊstˈpoʊn/", "(v)"),
            new EnglishWord("specialized", "chuyên môn", "/ˈspɛʃəˌlaɪzd/", "(adj)"),
            new EnglishWord("exceptional", "xuất sắc", "/ɪkˈsɛpʃənl/", "(adj)"),
            new EnglishWord("achieve", "đạt được", "/əˈtʃiv/", "(v)"),
            new EnglishWord("efficiency", "hiệu suất", "/ɪˈfɪʃənsi/", "(n)"),
            new EnglishWord("seek", "tìm kiếm", "/sik/", "(v)"),
            new EnglishWord("client", "khách hàng", "/ˈklaɪənt/", "(n)"),
            new EnglishWord("affordable", "phải chăng", "/əˈfɔrdəbl/", "(adj)"),
            new EnglishWord("remind", "nhắc nhở", "/rɪˈmaɪnd/", "(v)"),
            new EnglishWord("express", "diễn đạt", "/ɪkˈsprɛs/", "(v)"),
            new EnglishWord("vehicle", "phương tiện", "/ˈviːəkəl/", "(n)"),
            new EnglishWord("effective", "hiệu quả", "/ɪˈfɛktɪv/", "(adj)"),
            new EnglishWord("promotion", "khuyến mãi", "/prəˈmoʊʃən/", "(n)"),
            new EnglishWord("complicated", "phức tạp", "/ˈkɒmplɪˌkeɪtɪd/", "(adj)"),
            new EnglishWord("attend", "tham dự", "/əˈtɛnd/", "(v)"),
            new EnglishWord("compare", "so sánh", "/kəmˈpɛr/", "(v)"),
            new EnglishWord("attributable", "có thể quy cho", "/əˈtrɪbjʊtəbl/", "(adj)"),
            new EnglishWord("due to", "do", "/duː tuː/", "(phr)"),
            new EnglishWord("temporarily", "tạm thời", "/ˈtɛmpəˌrɛrɪli/", "(adv)"),
            new EnglishWord("experienced", "có kinh nghiệm", "/ɪkˈspɪriənst/", "(adj)"),
            new EnglishWord("take advantage", "tận dụng", "/teɪk ədˈvæntɪdʒ/", "(phr)"),
            new EnglishWord("annual", "hàng năm", "/ˈænjuəl/", "(adj)"),
            new EnglishWord("complete", "hoàn thành", "/kəmˈpliːt/", "(v)"),
            new EnglishWord("indicate", "chỉ ra", "/ˈɪndɪˌkeɪt/", "(v)"),
            new EnglishWord("associated with", "liên quan đến", "/əˈsoʊsiˌeɪtɪd wɪð/", "(phr)"),
            new EnglishWord("material", "vật liệu", "/məˈtɪriəl/", "(n)"),
            new EnglishWord("productivity", "năng suất lao động", "/ˌprɒdʌkˈtɪvɪti/", "(n)"),
            new EnglishWord("decline", "sự suy giảm", "/dɪˈklaɪn/", "(n)"),
            new EnglishWord("instead of", "thay vì", "/ɪnˈstɛd ʌv/", "(phr)"),
            new EnglishWord("extensive", "rộng lớn", "/ɪkˈstɛnsɪv/", "(adj)"),
            new EnglishWord("unintentionally", "không cố ý", "/ˌənɪnˈtɛnʃənəli/", "(adv)"),
            new EnglishWord("major", "chính", "/ˈmeɪdʒər/", "(adj)"),
        });
        vocabularys.Add(
        new List<EnglishWord>
        {
            new EnglishWord("candidate", "ứng viên", "/ˈkændɪˌdeɪt/", "(n)"),
            new EnglishWord("minor", "nhỏ", "/ˈmaɪnər/", "(adj)"),
            new EnglishWord("issue", "vấn đề", "/ˈɪʃuː/", "(n)"),
            new EnglishWord("ensure", "đảm bảo", "/ɪnˈʃʊr/", "(v)"),
            new EnglishWord("force", "lực lượng", "/fɔrs/", "(n)"),
            new EnglishWord("alternative", "phương án thay thế", "/ɔːlˈtɜrnətɪv/", "(n)"),
            new EnglishWord("succession", "liên tiếp", "/səkˈsɛʃən/", "(n)"),
            new EnglishWord("crisis", "khủng hoảng", "/ˈkraɪsɪs/", "(n)"),
            new EnglishWord("warranty", "bảo hành", "/ˈwɔrənti/", "(n)"),
            new EnglishWord("grant", "trợ cấp", "/ɡrænt/", "(n)"),
            new EnglishWord("complaint", "phàn nàn", "/kəmˈpleɪnt/", "(n)"),
            new EnglishWord("implement", "thực hiện", "/ˈɪmpləmənt/", "(v)"),
            new EnglishWord("modify", "sửa đổi", "/ˈmɒdɪfaɪ/", "(v)"),
            new EnglishWord("findings", "kết quả", "/ˈfaɪndɪŋz/", "(n)"),
            new EnglishWord("deny", "từ chối", "/dɪˈnaɪ/", "(v)"),
            new EnglishWord("secretary", "thư ký", "/ˈsɛkrəˌtɛri/", "(n)"),
            new EnglishWord("assistant", "trợ lý", "/əˈsɪstənt/", "(n)"),
            new EnglishWord("supplier", "nhà cung cấp", "/səˈplaɪər/", "(n)"),
            new EnglishWord("steady", "ổn định", "/ˈstɛdi/", "(adj)"),
            new EnglishWord("property", "tài sản", "/ˈprɒpərti/", "(n)"),
            new EnglishWord("merger", "sáp nhập", "/ˈmɜrdʒər/", "(n)"),
            new EnglishWord("acquisition", "sự thu mua", "/ˌækwɪˈzɪʃən/", "(n)"),
            new EnglishWord("expectation", "kỳ vọng", "/ˌɛkspɛkˈteɪʃən/", "(n)"),
            new EnglishWord("seminar", "hội thảo", "/ˈsɛmɪnɑr/", "(n)"),
            new EnglishWord("dismissal", "sự sa thải", "/dɪsˈmɪsəl/", "(n)"),
            new EnglishWord("budget", "ngân sách", "/ˈbʌdʒɪt/", "(n)"),
            new EnglishWord("recently", "gần đây", "/ˈriːsəntli/", "(adv)"),
            new EnglishWord("expert", "chuyên gia", "/ˈɛkspɜrt/", "(n)"),
            new EnglishWord("authority", "quyền lực", "/ɔːˈθɒrɪti/", "(n)"),
            new EnglishWord("declare", "tuyên bố", "/dɪˈklɛr/", "(v)"),
            new EnglishWord("solidarity", "tính đoàn kết", "/ˌsɒlɪˈdærəti/", "(n)"),
            new EnglishWord("banquet", "bữa tiệc", "/ˈbæŋkwɪt/", "(n)"),
            new EnglishWord("astimate", "ước lượng", "/ˈɛstɪˌmeɪt/", "(v)"),
            new EnglishWord("internship", "thực tập", "/ˈɪntɜːnʃɪp/", "(n)"),
        });
        vocabularys.Add(
        new List<EnglishWord>
        {
            new EnglishWord("architect", "kiến trúc sư", "/ˈɑrkɪˌtɛkt/", "(n)"),
            new EnglishWord("proposal", "đề xuất", "/prəˈpoʊzəl/", "(n)"),
            new EnglishWord("ultimately", "cuối cùng", "/ˈʌltɪmɪtli/", "(adv)"),
            new EnglishWord("turn down", "từ chối", "/tɜrn daʊn/", "(v)"),
            new EnglishWord("original", "gốc", "/əˈrɪdʒənəl/", "(adj)"),
            new EnglishWord("exist", "tồn tại", "/ɪɡˈzɪst/", "(v)"),
            new EnglishWord("interpret", "diễn giải", "/ɪnˈtɜrprɪt/", "(v)"),
            new EnglishWord("explain", "giải thích", "/ɪkˈspleɪn/", "(v)"),
            new EnglishWord("contest", "cuộc thi", "/ˈkɒntɛst/", "(n)"),
            new EnglishWord("packaging", "bao bì", "/ˈpækɪʤɪŋ/", "(n)"),
            new EnglishWord("pay attention to", "chú ý đến", "/peɪ əˈtɛnʃən tuː/", "(phr)"),
            new EnglishWord("negotiator", "người đàm phán", "/nɪˈɡoʊʃiˌeɪtər/", "(n)"),
            new EnglishWord("caterer", "đầu bếp", "/ˈkeɪtərər/", "(n)"),
            new EnglishWord("subject to", "dưới sự ảnh hưởng của", "/ˈsʌbdʒɪkt tuː/", "(phr)"),
            new EnglishWord("ceremony", "lễ nghi", "/ˈsɛrəˌmoʊni/", "(n)"),
            new EnglishWord("withdraw", "rút lui", "/wɪðˈdrɔ/", "(v)"),
            new EnglishWord("repeat", "lặp lại", "/rɪˈpiːt/", "(v)"),
            new EnglishWord("predict", "dự đoán", "/prɪˈdɪkt/", "(v)"),
            new EnglishWord("crop", "vụ mùa", "/krɒp/", "(n)"),
            new EnglishWord("harvest", "thu hoạch", "/ˈhɑrvɪst/", "(n)"),
            new EnglishWord("violent", "bạo lực", "/ˈvaɪələnt/", "(adj)"),
            new EnglishWord("struggle", "đấu tranh", "/ˈstrʌɡəl/", "(n)"),
            new EnglishWord("supposed", "giả định", "/səˈpoʊzd/", "(adj)"),
            new EnglishWord("submit", "nộp", "/səbˈmɪt/", "(v)"),
            new EnglishWord("spacious", "rộng rãi", "/ˈspeɪʃəs/", "(adj)"),
            new EnglishWord("announce", "công bố", "/əˈnaʊns/", "(v)"),
            new EnglishWord("refreshments", "đồ uống nhẹ", "/rɪˈfrɛʃmənts/", "(n)"),
            new EnglishWord("aware", "nhận thức", "/əˈwɛər/", "(adj)"),
            new EnglishWord("appliance", "thiết bị", "/əˈplaɪəns/", "(n)"),
            new EnglishWord("monitor", "theo dõi", "/ˈmɒnɪtər/", "(v)"),
            new EnglishWord("promptly", "ngay lập tức", "/ˈprɒmptli/", "(adv)"),
        });
        vocabularys.Add(
        new List<EnglishWord>
        {
            new EnglishWord("assign", "giao việc", "/əˈsaɪn/", "(v)"),
            new EnglishWord("division", "phân chia", "/dɪˈvɪʒən/", "(n)"),
            new EnglishWord("flaw", "nhược điểm", "/flɔ/", "(n)"),
            new EnglishWord("somewhat", "một phần", "/ˈsʌmˌhwɑt/", "(adv)"),
            new EnglishWord("adequate", "đủ", "/ˈædɪkwət/", "(adj)"),
            new EnglishWord("sufficient", "đủ", "/səˈfɪʃənt/", "(adj)"),
            new EnglishWord("customized", "tùy chỉnh", "/ˈkʌstəˌmaɪzd/", "(adj)"),
            new EnglishWord("evaluation", "đánh giá", "/ɪˌvæljuˈeɪʃən/", "(n)"),
            new EnglishWord("assessment", "đánh giá", "/əˈsɛsmənt/", "(n)"),
            new EnglishWord("assess", "đánh giá", "/əˈsɛs/", "(v)"),
            new EnglishWord("judge", "đánh giá", "/dʒʌdʒ/", "(v)"),
            new EnglishWord("appraisal", "đánh giá", "/əˈpreɪzəl/", "(n)"),
            new EnglishWord("enormous", "to lớn", "/ɪˈnɔrməs/", "(adj)"),
            new EnglishWord("campaign", "chiến dịch", "/kæmˈpeɪn/", "(n)"),
            new EnglishWord("enable", "cho phép", "/ɪˈneɪbl/", "(v)"),
            new EnglishWord("acquire", "thu được", "/əˈkwaɪər/", "(v)"),
            new EnglishWord("proprietor", "chủ sở hữu", "/prəˈpraɪətər/", "(n)"),
            new EnglishWord("curator", "bảo tàng trưởng", "/kjʊˈreɪtər/", "(n)"),
            new EnglishWord("recession", "suy thoái", "/rɪˈsɛʃən/", "(n)"),
            new EnglishWord("immediate", "ngay lập tức", "/ɪˈmidiət/", "(adj)"),
            new EnglishWord("criticism", "sự phê bình", "/ˈkrɪtɪˌsɪzəm/", "(n)"),
            new EnglishWord("intend to", "dự định", "/ɪnˈtɛnd tuː/", "(phr)"),
            new EnglishWord("thoroughly", "một cách kỹ lưỡng", "/ˈθɜrˌoʊli/", "(adv)"),
            new EnglishWord("significantly", "một cách đáng kể", "/sɪɡˈnɪfɪkəntli/", "(adv)"),
            new EnglishWord("fabulous", "tuyệt vời", "/ˈfæbjələs/", "(adj)"),
            new EnglishWord("amenity", "tiện nghi", "/əˈmiːnəti/", "(n)"),
            new EnglishWord("responsibility", "trách nhiệm", "/rɪˌspɑːnsəˈbɪləti/", "(n)"),
            new EnglishWord("access", "truy cập", "/ˈæksɛs/", "(n)"),
            new EnglishWord("analyst", "nhà phân tích", "/ˈænəlɪst/", "(n)"),
            new EnglishWord("retirement", "nghỉ hưu", "/rɪˈtaɪərmənt/", "(n)"),
            new EnglishWord("severe", "nghiêm trọng", "/səˈvɪr/", "(adj)"),
        });
        vocabularys.Add(
        new List<EnglishWord>
        {
            new EnglishWord("expansion", "sự mở rộng", "/ɪkˈspænʃən/", "(n)"),
            new EnglishWord("enable", "kích hoạt", "/ɪˈneɪbl/", "(v)"),
            new EnglishWord("properly", "đúng cách", "/ˈprɒpəli/", "(adv)"),
            new EnglishWord("stipulate", "quy định", "/ˈstɪpjʊleɪt/", "(v)"),
            new EnglishWord("contract", "hợp đồng", "/ˈkɒntrækt/", "(n)"),
            new EnglishWord("presentation", "bài thuyết trình", "/ˌprɛzənˈteɪʃən/", "(n)"),
            new EnglishWord("speech", "bài phát biểu", "/spiːʧ/", "(n)"),
            new EnglishWord("job opening", "cơ hội việc làm", "/ʤɒb ˈoʊpənɪŋ/", "(n)"),
            new EnglishWord("lead", "dẫn dắt", "/liːd/", "(v)"),
            new EnglishWord("determine", "xác định", "/dɪˈtɜrmɪn/", "(v)"),
            new EnglishWord("track", "theo dõi", "/træk/", "(v)"),
            new EnglishWord("strategic", "chiến lược", "/strəˈtiːdʒɪk/", "(adj)"),
            new EnglishWord("boost", "tăng cường", "/buːst/", "(v)"),
            new EnglishWord("qualified", "đủ năng lực", "/ˈkwɒlɪˌfaɪd/", "(adj)"),
            new EnglishWord("respond", "phản hồi", "/rɪˈspɒnd/", "(v)"),
            new EnglishWord("free of charge", "miễn phí", "/fri əv ʧɑrdʒ/", "(phr)"),
            new EnglishWord("charge", "phí", "/ʧɑrdʒ/", "(n)"),
            new EnglishWord("press", "báo chí", "/prɛs/", "(n)"),
            new EnglishWord("revenues", "doanh thu", "/ˈrɛvəˌnjuːz/", "(n)"),
            new EnglishWord("distinguished", "nổi bật", "/dɪˈstɪŋɡwɪʃt/", "(adj)"),
            new EnglishWord("career", "sự nghiệp", "/kəˈrɪər/", "(n)"),
            new EnglishWord("personalized", "tùy chỉnh", "/ˈpɜrsənəˌlaɪzd/", "(adj)"),
            new EnglishWord("condense", "rút gọn", "/kənˈdɛns/", "(v)"),
            new EnglishWord("concise", "ngắn gọn", "/kənˈsaɪs/", "(adj)"),
            new EnglishWord("priority", "ưu tiên", "/praɪˈɒrɪti/", "(n)"),
            new EnglishWord("injury", "chấn thương", "/ˈɪnʤəri/", "(n)"),
        });
        vocabularys.Add(
        new List<EnglishWord>
        {
            new EnglishWord("appoint", "bổ nhiệm", "/əˈpɔɪnt/", "(v)"),
            new EnglishWord("expertise", "chuyên môn", "/ˌɛkspɜrˈtiːz/", "(n)"),
            new EnglishWord("praise", "khen ngợi", "/preɪz/", "(v)"),
            new EnglishWord("specialist", "chuyên gia", "/ˈspɛʃəlɪst/", "(n)"),
            new EnglishWord("treat", "điều trị", "/triːt/", "(v)"),
            new EnglishWord("encounter", "gặp gỡ", "/ɪnˈkaʊntər/", "(v)"),
            new EnglishWord("performance", "hiệu suất", "/pərˈfɔrməns/", "(n)"),
            new EnglishWord("effort", "nỗ lực", "/ˈɛfərt/", "(n)"),
            new EnglishWord("accommodate", "điều chỉnh", "/əˈkɒmədeɪt/", "(v)"),
            new EnglishWord("recall", "nhớ lại", "/rɪˈkɔːl/", "(v)"),
            new EnglishWord("the minutes", "biên bản", "/ðə ˈmɪnɪts/", "(n)"),
            new EnglishWord("omit", "bỏ qua", "/ˈoʊmɪt/", "(v)"),
            new EnglishWord("recruit", "tuyển dụng", "/rɪˈkruːt/", "(v)"),
            new EnglishWord("suffer", "chịu đựng", "/ˈsʌfər/", "(v)"),
            new EnglishWord("loss", "mất mát", "/lɒs/", "(n)"),
            new EnglishWord("council", "hội đồng", "/ˈkaʊnsl/", "(n)"),
            new EnglishWord("sponsor", "tài trợ", "/ˈspɒnsər/", "(v)"),
            new EnglishWord("launch", "phóng", "/lɔːnʧ/", "(v)"),
            new EnglishWord("release", "phát hành", "/rɪˈliːs/", "(v)"),
            new EnglishWord("stimulate", "kích thích", "/ˈstɪmjəˌleɪt/", "(v)"),
            new EnglishWord("saucepan", "nồi nướng", "/ˈsɔːspən/", "(n)"),
            new EnglishWord("related to", "liên quan đến", "/rɪˈleɪtɪd tuː/", "(phr)"),
            new EnglishWord("renew", "gia hạn", "/rɪˈnjuː/", "(v)"),
            new EnglishWord("banquet", "tiệc", "/ˈbæŋkwɪt/", "(n)"),
        });
    }

}
