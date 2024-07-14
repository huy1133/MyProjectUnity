using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingChapter0 : MonoBehaviour
{
    [SerializeField] GameObject ground, tree1, tree2, grass, bush;

    List<GameObject> groundList = new List<GameObject>(); 
    List<GameObject> tree1List = new List<GameObject>();
    List<GameObject> tree2List = new List<GameObject>();
    List<GameObject> grassList = new List<GameObject>();
    List<GameObject> bushList = new List<GameObject>();
    List<List<GameObject>> listMain = new List<List<GameObject>>();
    int numberObject = 4;
    private void Awake()
    {
        SpawnObject();
        listMain.Add(groundList);
        listMain.Add(tree1List);
        listMain.Add(tree2List);
        listMain.Add(grassList);
        listMain.Add(bushList);
    }
    private void Start()
    {
       
    }
    void SpawnObject()
    {
        for (int i = 0; i < numberObject; i++) {
            GameObject 
            temp = (Instantiate(ground));
            temp.transform.SetParent(transform, false);
            temp.SetActive(false);
            groundList.Add(temp);

            temp = Instantiate(tree1);
            temp.transform.SetParent(transform, false);
            temp.SetActive(false);
            tree1List.Add(temp);

            temp = Instantiate(tree2);
            temp.transform.SetParent(transform, false);
            temp.SetActive(false);
            tree2List.Add(temp);

            temp = Instantiate(grass);
            temp.transform.SetParent(transform, false);
            temp.SetActive(false);
            grassList.Add(temp);

            temp = Instantiate(bush);
            temp.transform.SetParent(transform, false);
            temp.SetActive(false);
            bushList.Add(temp);
        }
    }
    public void returnOBJ(GameObject obj)
    {
        obj.transform.SetParent(transform, false);
        obj.SetActive(false);
    }
    public GameObject getOBJ(int numID)
    {
        foreach (GameObject obj in listMain[numID])
        {
            if (obj.transform.parent != null)
            {
                obj.transform.SetParent(null, false);
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }
    
}
