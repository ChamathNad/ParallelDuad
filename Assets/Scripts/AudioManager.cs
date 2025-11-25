using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip flipClip, matchClip, mismatchClip, gameoverClip;
    private AudioSource src;
    public static AudioManager Instance { get; private set; }

    private bool isMute = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        src = gameObject.AddComponent<AudioSource>();
    }

    public void Mute(bool val)
    {
        isMute = val;
    }


    public void PlayFlip() 
    {
        if(isMute)
            return;
        src.PlayOneShot(flipClip); 
    }
    public void PlayMatch()
    {
        if (isMute)
            return;
        src.PlayOneShot(matchClip);
    }
    public void PlayMismatch()
    {
        if (isMute)
            return;
        src.PlayOneShot(mismatchClip); 
    }
    public void PlayGameOver()
    {
        if (isMute)
            return;
        src.PlayOneShot(gameoverClip); 
    }

}
