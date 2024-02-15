
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Animator animator;
    List<GameObject> list;
    float exploreForce = 150;
    float exploreRadius = 1.5f;
    private void Start()
    {
        list = new List<GameObject>();
        animator = GetComponent<Animator>();
    }
    
    public void explore()
    {
        animator.SetBool("isExplore", true);
        foreach (Collider2D cld in Physics2D.OverlapCircleAll(transform.position,exploreRadius))
        {
            if(cld.GetComponent<Rigidbody2D>()&&cld.gameObject!=gameObject)
            {
                float torqueForce;
                float distance = Vector2.Distance(cld.gameObject.transform.position , transform.position);
                Vector3 bombTransform = transform.position;
                bombTransform.y=transform.position.y-1;
                Vector2 vetorForce = (cld.gameObject.transform.position - bombTransform).normalized;
                cld.gameObject.GetComponent<Rigidbody2D>().AddForce(vetorForce * (exploreForce-distance/exploreRadius*exploreForce));
                if (vetorForce.x < 0)
                    torqueForce = 8;
                else
                    torqueForce = -8;
                cld.gameObject.GetComponent<Rigidbody2D>().AddTorque(torqueForce, ForceMode2D.Impulse);
            }
        }
        Invoke("disableBomb", 0.5f);
    }
    public void disableBomb()
    {
        animator.SetBool("isExplore", false);
        gameObject.SetActive(false);
    }
}
