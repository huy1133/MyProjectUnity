
using UnityEngine;


public class LayerController : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] Transform bombTransform;
    bool check;
    private void Start()
    {
        check = true;
        bomb.transform.position = bombTransform.position;
        bomb.SetActive(false);
    }
    private void Update()
    {
        if (!bomb.activeSelf) 
        {
            bomb.transform.position = bombTransform.position;
            bomb.SetActive(true);
        }
    }
    private void can()
    {
        check = true;
    }
    private void explore()
    {
        bomb.GetComponent<Bomb>().explore();
    }
    public void PlayerAttack(float timeExplore)
    {
       if(check)
       {
            bomb.SetActive(true);
            float force = 140;
            bomb.transform.position = bombTransform.position;
            bomb.GetComponent<Rigidbody2D>().velocity = (Vector2.zero);
            bomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 2) * force);
            check = false;
            Invoke("can", 0.5f);
            Invoke("explore", timeExplore);
       }
    }
    
}
