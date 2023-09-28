using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    GameObject gameObjectTarget;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform pointShoot;
    float timeShoot;
    float timeCount;
    bool canShoot;

    private void Start()
    {
        timeShoot = 5;
        timeCount = 0;
        canShoot = true;
        gameObjectTarget = null; 
    }
    private void Update()
    {
        gameObjectTarget = findTarget();
        forWardGun();
        shoot();
    }
    void shoot()
    {
        timeCount += Time.deltaTime;
        if (timeCount > timeShoot)
        {
            canShoot = true;
        }
        if(gameObjectTarget != gameObject && canShoot)
        {

            canShoot = false;
            timeCount = 0;
            GameObject tempBullet = Instantiate(bullet,pointShoot.position, Quaternion.identity);
            tempBullet.transform.rotation = transform.rotation;
            gun.GetComponent<Animator>().SetBool("Fire", true);
            Invoke("fireOff", 0.1f);
        }
    }
    void fireOff()
    {
        gun.GetComponent<Animator>().SetBool("Fire", false);
    }
    void scaleGun()
    {
        Vector3 scale = transform.localScale;
        if(gameObjectTarget!=gameObject) 
        {
            scale.x = Mathf.Abs(scale.x);
            if (gameObjectTarget.transform.position.x > transform.position.x)
            {
                scale.y = Mathf.Abs(scale.y);
            }
            else
            {
                scale.y = Mathf.Abs(scale.y)*-1;
            }
        }
        else
        {
            scale.y = Mathf.Abs(scale.y);
            scale.x = Mathf.Abs(scale.x)*-1;
            
        }
        transform.localScale = scale;
    }
    void forWardGun()
    {
        if (gameObjectTarget!=null) 
        {
            Vector3 targetDirection = gameObjectTarget.transform.position - transform.position;
            //tinh goc tu gun den enemy
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
            scaleGun();
        }
        
        
    }
    GameObject findTarget()
    {
        GameObject targetNearest = gameObjectTarget;
        float distance = 20;
        if (targetNearest == null)
        {
            GameObject[] tempTarget = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject target in tempTarget)
            {
                float distanceTemp = Vector3.Distance(gameObject.transform.position, target.transform.position);
                if (distance > distanceTemp)
                {
                    distance = distanceTemp;
                    targetNearest = target;
                }

            }
        }
        return targetNearest;
    }
}
