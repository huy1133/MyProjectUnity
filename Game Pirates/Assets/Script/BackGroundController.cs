using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        float bgrHeight = spriteRenderer.sprite.bounds.size.y;
        float bgrWidth = spriteRenderer.sprite.bounds.size.x;

        transform.localScale = new Vector3(cameraWidth/bgrWidth,cameraHeight/bgrHeight,1f);
    }
}
