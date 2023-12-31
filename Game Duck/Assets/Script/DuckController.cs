
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    [SerializeField] GameObject character;
    float speed;
    int blood;
    int damage;
    [SerializeField] GameObject egg;
    Vector3 target;
    float currentTime;
    float timeDamaged;
    SpriteRenderer spriteRenderer;

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

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

          
    }
    private void Update()
    {
        if(currentTime <= timeDamaged)
        {
            currentTime += Time.deltaTime;
        }
        Move();
    }
    // Update is called once per frame
    
    void Move()
    {
        target = character.transform.position;
        //transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, target, speed*Time.deltaTime);
        bool flip =target.x>transform.position.x?false:true;
        spriteRenderer.flipX = flip;
    }
    public void damaged()
    {
        blood -= PlayerPrefs.GetInt("Damage");
        spriteRenderer.color = Color.red;
        Invoke("chanceColor", 0.1f);
        die();
    }
    public void die()
    {
        if(blood <= 0)
        {
            int SLEgg = UnityEngine.Random.Range(1, 4);
            for (int i=0; i<SLEgg; i++)
            {
                Vector3 tempPoint = transform.position;
                tempPoint.x += UnityEngine.Random.Range(0.1f, 0.6f);
                tempPoint.y += UnityEngine.Random.Range(0.1f, 0.6f);
                GameObject tempegg = Instantiate(egg, tempPoint, Quaternion.identity);
            }
            Destroy(gameObject);
        }
       
        
    }
    void chanceColor()
    {
        spriteRenderer.color = Color.white;
    }
    public void lever(int lv)
    {
        damage*=lv;
        blood*=lv;
    }
    
}
