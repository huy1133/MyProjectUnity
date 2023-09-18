using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private float destroyTime;
    [SerializeField] AudioClip destroySound;
    AudioSource audio;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        audio.PlayOneShot(destroySound);
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        // Gọi hàm DestroyObject sau 5f
        Invoke("DestroyObject", destroyTime);
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 temp = transform.position;
        temp.x += speed * Time.deltaTime;
        transform.position = temp;
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
