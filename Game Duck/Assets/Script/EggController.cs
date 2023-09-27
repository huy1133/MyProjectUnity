using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EggController : MonoBehaviour
{
    GameObject character;
    bool isSucked;
    Vector3 target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            GameObject.Find("GameController").GetComponent<GameController>().eggate++;
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        isSucked = false;
        character = GameObject.Find("Player");
    }
    private void Update()
    {
        float Distance = (character.transform.position - transform.position).magnitude;
        if(Distance <=1.5f)
        {
            isSucked=true;
        }
        sucked();
    }
    void sucked()
    {
        if(isSucked)
        {
            target = character.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, target, 6 * Time.deltaTime);
        }
    }
}
