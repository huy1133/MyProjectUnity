using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    MeshRenderer meshRenderer;
    float scroll;
    public bool isGame;
    float widthCamera;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        scroll = 0;
        isGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGame) 
        {
            widthCamera = 2f * Camera.main.orthographicSize * Camera.main.aspect;
            transform.localScale = new Vector3(widthCamera, 1, 1);
            scroll -= Time.deltaTime;
            meshRenderer.material.mainTextureOffset = new Vector2(scroll, 0);
        }
        
    }
}
