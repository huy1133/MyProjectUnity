using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    [SerializeField] GameObject character;
    Vector3 target;
    // Start is called before the first frame update
   
    private void Update()
    {
        target = character.transform.position;
        starTarget();
    }
    // Update is called once per frame
    void starTarget()
   {
        transform.DOMove(target, 5f, false);
   }
}
