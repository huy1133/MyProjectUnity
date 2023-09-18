using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxY, minY;

    // Attack
    [SerializeField] private GameObject bulletPre;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float waitAttack = 0.35f;
    [SerializeField] private float timer;
    private bool canAttack;

    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip destroySound;
    AudioSource sound;

    private void Start()
    {
        sound = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sound.PlayOneShot(destroySound);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Attack();
    }

    void MovePlayer()
    {
        float v = Input.GetAxis("Vertical");

        if (v > 0)
        {
            Vector3 temp = transform.position;
            temp.y += speed * Time.deltaTime;

            if (temp.y > maxY)
            {
                temp.y = maxY;
            }

            transform.position = temp;
        }
        else if (v < 0)
        {
            Vector3 temp = transform.position;
            temp.y -= speed * Time.deltaTime;
            if (temp.y < minY)
            {
                temp.y = minY;
            }

            transform.position = temp;
        }
    }

    void Attack()
    {
        timer += Time.deltaTime;

        if (timer > waitAttack)
        {
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canAttack)
            {
                canAttack = false;
                timer = 0;
                GameObject bullet = Instantiate(bulletPre, attackPoint.position, Quaternion.identity);
                // Destroy(bullet, 10f);
                sound.PlayOneShot(attackSound);
            }
        }
    }
}
