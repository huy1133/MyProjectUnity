using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;
using UnityEngine.SceneManagement;

public class GameControllerChapter0 : MonoBehaviour
{
    [SerializeField] GameObject dandelion;
    [SerializeField] Vector3 startLocateDandelion;
    [SerializeField] ParticleSystem IntroductionParticle;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip BlingClip;
    [SerializeField] GameObject character;
    [SerializeField] Transform currentPointCamera;
    [SerializeField] GameObject backGround;
    [SerializeField] Vector3[] posTree;
    [SerializeField] Vector3[] posBush;
    [SerializeField] GameObject[] diffirentOBJ;
    Vector3[] PointGround = new Vector3[3];
    List<GameObject> GOBJGround = new List<GameObject>();
    List<GameObject> GOBJGrass = new List<GameObject>();
    Dictionary<int,GameObject> GOJTree = new Dictionary<int,GameObject>();
    Dictionary<int, GameObject> GOJBush = new Dictionary<int, GameObject>();
    Vector3 currentPointGround;
    float stepGround;
    private void Awake()
    {
        character.SetActive(false);
        dandelion.transform.position = startLocateDandelion;
    }
    private void Start()
    {
        stepGround = 17.8f;
        currentPointGround = Vector3.zero;
        resetPointGround("");
        PlayNewGame();
    }
    private void Update()
    {
        updateGround();
        updateTreeAndBush();
        updateDiffirent();
    }
    void updateDiffirent()
    {
        float distance = 25f;
        foreach(GameObject @object in diffirentOBJ)
        {
            if (Vector3.Distance(@object.transform.position, currentPointCamera.transform.position) <= distance && !@object.activeSelf)
            {
                @object.SetActive(true);
            }
            else if (Vector3.Distance(@object.transform.position, currentPointCamera.transform.position) > distance && @object.activeSelf)
            {
                @object.SetActive(false);
            }
        }
    }
    void updateTreeAndBush()
    {
        float distance = 20f;
        for(int i=0; i<posTree.Length; i++)
        {
            if (Vector3.Distance(posTree[i], currentPointCamera.transform.position) <= distance)
            {
                if (!GOJTree.ContainsKey(i))
                {
                    GameObject temp = GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().getOBJ(2);
                    temp.transform.position = posTree[i];
                    GOJTree.Add(i, temp);
                }
            }
            else
            {
                if (GOJTree.ContainsKey(i))
                {
                    GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().returnOBJ(GOJTree[i]);
                    GOJTree.Remove(i);
                }
            }
        }
        for (int i = 0; i < posBush.Length; i++)
        {
            if (Vector3.Distance(posBush[i], currentPointCamera.transform.position) <= distance)
            {
                if (!GOJBush.ContainsKey(i))
                {
                    GameObject temp = GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().getOBJ(3);
                    temp.transform.position = posBush[i];
                    GOJBush.Add(i, temp);
                }
            }
            else
            {
                if (GOJBush.ContainsKey(i))
                {
                    GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().returnOBJ(GOJBush[i]);
                    GOJBush.Remove(i);
                }
            }
        }
    }
    
    void updateGround()
    {
        backGround.transform.position =  new Vector3(backGround.transform.position.x,0, backGround.transform.position.y);
        float disCamToP0 = Vector3.Distance(currentPointCamera.position, PointGround[0]);
        float disCamToP1 = Vector3.Distance(currentPointCamera.position, PointGround[1]);
        float disCamToP2 = Vector3.Distance(currentPointCamera.position, PointGround[2]);
        if(disCamToP1 >= disCamToP0)
        {
            currentPointGround = PointGround[0];
            resetPointGround("left");
        }
        else if (disCamToP1 >= disCamToP2)
        {
            currentPointGround = PointGround[2];
            resetPointGround("right");
        }
    }
    void resetPointGround(string Type)
    {
        PointGround[1] = currentPointGround;
        PointGround[2] = new Vector3(currentPointGround.x + stepGround,0,0);
        PointGround[0] = new Vector3(currentPointGround.x - stepGround, 0, 0);
        if(Type == "left")
        {
            GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().returnOBJ(GOBJGround[2]);
            GOBJGround.Remove(GOBJGround[2]);
            GameObject temp = GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().getOBJ(0);
            temp.transform.position = PointGround[0];
            GOBJGround.Insert(0, temp);

            GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().returnOBJ(GOBJGrass[2]);
            GOBJGrass.RemoveAt(2);
            GameObject temp1 = GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().getOBJ(1);
            temp1.transform.position = PointGround[0];
            GOBJGrass.Insert(0, temp1);
        }
        else if (Type == "right")
        {
            GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().returnOBJ(GOBJGround[0]);
            GOBJGround.Remove(GOBJGround[0]);
            GameObject temp = GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().getOBJ(0);
            temp.transform.position = PointGround[2];
            GOBJGround.Insert(2, temp);

            GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().returnOBJ(GOBJGrass[0]);
            GOBJGrass.RemoveAt(0);
            GameObject temp1 = GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().getOBJ(1);
            temp1.transform.position = PointGround[2];
            GOBJGrass.Insert(2, temp1);
        }
        else
        {
            for (int i = 0; i < PointGround.Length; i++)
            {
                GameObject temp = GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().getOBJ(0);
                temp.transform.position = PointGround[i];
                GOBJGround.Add(temp);

                GameObject temp1 = GameObject.Find("ObjectPooling").GetComponent<PoolingChapter0>().getOBJ(1);
                temp1.transform.position = PointGround[i];
                GOBJGrass.Add(temp1);
            }
        }
    }
    void PlayContinuedGame()
    { 
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
    }
    void PlayNewGame()
    {
        PlayerPrefs.SetInt("Level", 0);
        //startGame();    
        dandelionAction();
    }
    void startGame()
    {
        character.SetActive(true);
        character.transform.position = dandelion.transform.position;
    }
    void dandelionAction()
    {
        Sequence flowerSequence = DOTween.Sequence();
        Sequence action1 = DOTween.Sequence();
        action1.Append(dandelion.transform.DOMove(new Vector3(10, 0.5f, 0), 5f)).SetEase(Ease.Linear);
        action1.Join(dandelion.transform.DORotate(new Vector3(0, 0, -45), 5f)).SetEase(Ease.Linear);
        flowerSequence.Append(action1);

        Sequence action2 = DOTween.Sequence();
        action2.Append(dandelion.transform.DOMove(new Vector3(-5.8f, -1f, 0), 5f)).SetEase(Ease.Linear);
        action2.Join(dandelion.transform.DORotate(new Vector3(0, 0, 45), 5f)).SetEase(Ease.Linear);
        flowerSequence.Append(action2);

        Sequence action3 = DOTween.Sequence();
        action3.Append(dandelion.transform.DOMove(new Vector3(-1.78f, -2.55f, 0), 5f)).SetEase(Ease.Linear);
        action3.Join(dandelion.transform.DORotate(new Vector3(0, 0, -3.6f), 5f)).SetEase(Ease.Linear);
        flowerSequence.Append(action3);

        flowerSequence.OnComplete(() => {
            IntroductionParticle.Play();
            audioSource.clip = BlingClip;
            audioSource.Play();
            startGame();
            dandelion.SetActive(false);
            }
        );

    }
}
