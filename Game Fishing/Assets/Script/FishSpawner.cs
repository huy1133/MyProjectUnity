using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] FishContreller fish;
    [SerializeField] FishContreller.FishType[] fishType;

    private void Awake()
    {
        // Đặt dung lượng tweens theo cách thủ công
        DOTween.SetTweensCapacity(300, 50);
    }
    private void Start()
    {
       for(int i=0; i<fishType.Length; i++)
        {
            for(int j =0; j < fishType[i].fishCount; j++)
            {
                FishContreller newFish = Instantiate(fish);
                newFish.Type = fishType[i];
            }
        }
    }
}
