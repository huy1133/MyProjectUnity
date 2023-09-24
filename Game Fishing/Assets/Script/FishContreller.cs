using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FishContreller : MonoBehaviour
{
    [Serializable]
    public class FishType
    {
        public int price;
        public int fishCount;
        public Sprite spriteFish;
        public int minBorn;
        public int maxBorn;
        public int speed;
    }
    public FishType type;
    public FishType Type
    {
        get { return type; }
        set { type = value; }
    }
    Tween fishTween;
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        starMove();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = type.spriteFish;
    }
    public void starMove()
    {
        Vector3 born = transform.position;
        born.y = UnityEngine.Random.Range(type.minBorn, type.maxBorn);
        transform.position = born;
        float delay = UnityEngine.Random.Range(0, 5);
        float speedMove = UnityEngine.Random.Range(type.speed-3>1?type.speed-3:1,type.speed);
        fishTween = transform.DOMoveX(2.6f,type.speed , false)
        .SetDelay(delay)
        .SetLoops(-1, LoopType.Yoyo)
        .OnStepComplete(delegate
        {
            transform.DOScaleX(transform.localScale.x * -1, 0);
        });
    }
    
}
