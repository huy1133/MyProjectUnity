using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed, rotateSpeed;

    [SerializeField] private GameObject bulletPre;
    [SerializeField] private Transform attackPoint;
    float waitAttack = 0.35f;
    float timer;
    private bool canAttack;

    public bool canRotate;
    public bool isEnemyShip;

    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip destroySound;
    AudioSource sound;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //sound.PlayOneShot(destroySound);
        Destroy(gameObject);
        
    }
    void Start()
    {
        timer = 0;
        sound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
        RotateEnemy();
        Attack();
    }
    void Attack()
    {
        if (isEnemyShip)
        {
            timer += Time.deltaTime;

            if (timer > waitAttack)
            {
                canAttack = true;
            }
            if (canAttack)
            {
                waitAttack = Random.Range(1, 5);
                canAttack = false;
                timer = 0;
                GameObject bullet = Instantiate(bulletPre, attackPoint.position, Quaternion.identity);
                bullet.GetComponent<BulletController>().speed *= -1;
                sound.PlayOneShot(attackSound);
            }
        }
    }
    void MoveEnemy()
    {
        Vector3 temp = transform.position;
        temp.x -= speed * Time.deltaTime;
        transform.position = temp;
    }
    void RotateEnemy()
    {
        if (canRotate)
        {
            transform.Rotate(new Vector3(0f, 0f, rotateSpeed * Time.deltaTime));
        }
    }
}
