using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimStateManager : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask mask;
    [SerializeField] MultiAimConstraint RhandIK;
    Coroutine aimLerpCoroutine;
    //[SerializeField] MovementStateManager movementStateManager;
    void Start()
    {
        
    }

    void Update()
    {
        ShootRaycast();
    }

    public void SetAimRigActive(bool isActive)
    {
        if (aimLerpCoroutine != null)
            StopCoroutine(aimLerpCoroutine);

        aimLerpCoroutine = StartCoroutine(LerpRigWeight(isActive ? 1f : 0f));
    }

    private IEnumerator LerpRigWeight(float targetWeight)
    {
        float duration = 0.3f; 
        float elapsed = 0f;
        float startWeight = RhandIK.weight;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            RhandIK.weight = Mathf.Lerp(startWeight, targetWeight, elapsed / duration);
            yield return null;
        }

        RhandIK.weight = targetWeight; 
    }

    public void ShootRaycast()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, mask))
        {
            firePoint.transform.position = hitInfo.point;
        }
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
    }
}
