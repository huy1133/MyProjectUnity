
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject bomb;
    Animator animator;
    Vector3 starPoint;
    bool isGround;
    bool canAttack;
  
    private void Start()
    {
        starPoint = transform.position;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        updateStatus(); 
    }
    
    void updateStatus()
    {
        if (transform.position.y > starPoint.y)
        {
            isGround = false;
        }
        else
        {
            isGround = true;
        }
        if (isGround)
        {
            if (transform.position.x > starPoint.x)
            {
                Vector3 pointToMove = starPoint;
                pointToMove.x = starPoint.x - 0.5f;
                transform.position = Vector3.Slerp(transform.position, pointToMove, Time.deltaTime);
                animator.SetBool("isRun", true);
            }
            else
            {
                animator.SetBool("isRun", false);
            }
        }
        
    }
    public void enemyAttack()
    {
        Invoke("startAttackAnimation", 1.5f);
        canAttack = true;
    }
    void startAttackAnimation()
    {
        animator.SetBool("isAttack", true);
        Invoke("startAttackForce", 0.2f);
    }
    void startAttackForce()
    {
        float exploreRadius = 1.5f;
        foreach (Collider2D cld in Physics2D.OverlapCircleAll(transform.position, exploreRadius))
        {
            if (cld.gameObject.tag == "Bomb")
            {
                bomb = cld.gameObject;
                if (canAttack)
                {
                    canAttack = false;
                    float force = 140;
                    bomb.GetComponent<Rigidbody2D>().velocity = (Vector2.zero);
                    bomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 2) * force);
                }
                break;
            }
        }
        animator.SetBool("isAttack", false);
    }
}
