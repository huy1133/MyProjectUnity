using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    Camera mainCamera;
    Vector3 oldHookTransform;
    bool isMove;
    float maxX, minX;
    Tween tweenCamera;
    CircleCollider2D circleCollider;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        minX = mainCamera.ViewportToWorldPoint(Vector3.zero).x;
        maxX = mainCamera.ViewportToWorldPoint(Vector3.right).x;
        oldHookTransform = transform.position;
        isMove=false;
        circleCollider = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        if(isMove && Input.GetMouseButton(0))
        {
            Vector3 vector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            if(vector.x>position.x)
            {
                transform.DOScaleX(1, 0);
            }
            else if(vector.x<position.x)
            {
                transform.DOScaleX(-1, 0);
            }
            //position.x = vector.x;
            position.x = Mathf.Clamp(vector.x, minX, maxX);
            transform.position = position;
        }
    }
    public void starFishing()
    {
        tweenCamera = mainCamera.transform.DOMoveY(-50f, 5f, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y <= -10)
            {
                transform.SetParent(mainCamera.transform);
            }
        }).OnComplete(delegate{
            tweenCamera = mainCamera.transform.DOMoveY(0f, 7f, false).OnUpdate(delegate
            {
                circleCollider.enabled = true;
                stopFishing();
            });
        });
        ScreensManager.instance.ChangeScreen(Screens.InGame);
        isMove = true;
    }
    void stopFishing()
    {
        if (mainCamera.transform.position.y >= -25)
        {
            tweenCamera.Kill();
            tweenCamera = mainCamera.transform.DOMoveY(0, 2f, false).OnUpdate(delegate
            {
                if (mainCamera.transform.position.y >= -10)
                {
                    transform.SetParent(null);
                    transform.position = oldHookTransform;
                }
                circleCollider.enabled = false;
            });
        }
    }
}
