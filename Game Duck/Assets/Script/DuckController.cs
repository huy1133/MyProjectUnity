
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    GameObject character;
    float speed;
    int blood;
    int damage;
    [SerializeField] GameObject egg;
    Vector3 target;
    float currentTime;
    float timeDamaged;

    // Start is called before the first frame update

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Character")
        {
            if (timeDamaged <= currentTime)
            {

                GameObject.Find("GameController").GetComponent<GameController>().damaged(damage);
                currentTime = 0;
            }
            
        }
    }
    private void Start()
    {
        speed = 1f;
        blood = 1;
        damage = 1;

        timeDamaged = 1;
        currentTime = 0;

        character = GameObject.Find("Player");
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        Move();
    }
    // Update is called once per frame
    
    void Move()
    {
        target = character.transform.position;
        //transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, target, speed*Time.deltaTime);
    }
    public void damaged()
    {
        blood-=PlayerPrefs.GetInt("Damage");
        die();
    }
    public void die()
    {
        if(blood <= 0)
        {
            GameObject tempegg = Instantiate(egg,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
    public void lever(int lv)
    {
        damage*=lv;
        blood*=lv;
    }
    
}
