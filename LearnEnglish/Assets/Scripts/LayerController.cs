
using UnityEngine;


public class LayerController : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] Transform bombTransform;
    Rigidbody2D rigidbody2D;
    public Animator PlayerAnimator;
    bool check;
    public int heart;
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        heart = 2;
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
        if (heart == 1)
        {
            rigidbody2D.constraints = ~RigidbodyConstraints2D.FreezeAll;
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
            if(timeExplore == 2.7f)
            {
                Invoke("hit", timeExplore);
            }
       }
    }
    private void hit()
    {
        PlayerAnimator.SetBool("hit", true);
        Invoke("stopHit", 0.5f);
    }
    private void stopHit()
    {
        PlayerAnimator.SetBool("hit", false);
    }
}
