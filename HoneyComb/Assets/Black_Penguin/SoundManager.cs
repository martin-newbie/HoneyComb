using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SoundType
{
    BGM,
    SE
}

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource BgmAudioSource;
    public AudioSource SeAudioSource;
    private void Awake()
    {
        GameObject BgmObj = new GameObject();
        
        
        GameObject SeObj = new GameObject();


    }
    public void Start()
    {

    }
}
