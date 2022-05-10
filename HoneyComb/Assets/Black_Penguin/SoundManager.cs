using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundType
{
    BGM,
    SE
}

public class SoundManager : Singleton<SoundManager>
{
    Dictionary<SoundType, AudioSource> audioSources = new Dictionary<SoundType, AudioSource>();
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private Dictionary<SoundType, float> audioVolume = new Dictionary<SoundType, float>();
    private void Awake()
    {
        SceneManager.sceneLoaded += whenSceneLoad;
        DontDestroyOnLoad(this.gameObject);
        
        //리소스 폴더안 Sounds폴더안에 오디오클립들을 모은다
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/");
        foreach (AudioClip clip in clips)
            audioClips[clip.name] = clip;

        //오디오 소스 생성후 사운드매니저에 상속
        GameObject BgmObj = new GameObject();
        audioSources[SoundType.BGM] = BgmObj.AddComponent<AudioSource>();
        BgmObj.transform.parent = transform;
        audioSources[SoundType.BGM].loop = true;
        audioVolume[SoundType.BGM] = 0.5f;

        GameObject SeObj = new GameObject();
        audioSources[SoundType.SE] = SeObj.AddComponent<AudioSource>();
        SeObj.transform.parent = transform;
        audioVolume[SoundType.SE] = 0.5f;
    }
    public void PlaySound(string ClipName, SoundType type = SoundType.SE, float Volume = 1, float Pitch = 1)
    {
        switch (type)
        {
            case SoundType.BGM:
                audioSources[type].clip = audioClips[ClipName];
                audioSources[type].Play();
                audioSources[type].volume = Volume * audioVolume[type];
                audioSources[type].pitch = Pitch;
                break;
            case SoundType.SE:
                audioSources[type].PlayOneShot(audioClips[ClipName], Volume * audioVolume[type]);
                audioSources[type].pitch = Pitch;
                break;
        }
    }
    private void whenSceneLoad(Scene level,LoadSceneMode mode)
    {
        //씬이 불러와질때 어떤씬이냐에 따라 배경음악을 전환한다
        switch (level.name)
        {
            case "InGameScene":
                PlaySound("Ingame", SoundType.BGM);
                break;
            default:
                if (audioSources[SoundType.BGM].clip != audioClips["Title"])
                    PlaySound("Title", SoundType.BGM);
                break;
        }
    }
}
