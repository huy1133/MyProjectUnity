using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject,3);
    }
    void Update()
    {
        transform.Translate(Vector2.right * PlayerPrefs.GetInt("SpeedShoot") * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "enemy")
        {
            collision.gameObject.GetComponent<DuckController>().damaged();
            Destroy(gameObject);
        }
    }
}
