using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource audio;
    [SerializeField] AudioClip backGround, inGame;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = backGround;
        audio.Play();
    }
    public void changedAudio(int n)
    {
        if(n == 1)
        {
            audio.clip = inGame;
            audio.Play();
        }
    }
}
