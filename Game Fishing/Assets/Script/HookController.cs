using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    Camera mainCamera;
    Vector3 oldHookTransform;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        oldHookTransform = transform.position;
    }

    public void starFishing()
    {
        mainCamera.transform.DOMoveY(-20f, 5f, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y <= -10)
            {
                transform.SetParent(mainCamera.transform);
            }
        }).OnComplete(delegate{
            mainCamera.transform.DOMoveY(0f, 5f, false).OnUpdate(delegate
            {
                if(mainCamera.transform.position.y >= -10)
                {
                    transform.SetParent(null);
                    transform.position = oldHookTransform;
                }
            });
        });
        ScreensManager.instance.ChangeScreen(Screens.InGame);
    }
}
