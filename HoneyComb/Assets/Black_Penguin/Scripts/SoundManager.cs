using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundType
{
    BGM,
    SE,
    END
}

public class SoundManager : Singleton<SoundManager>
{
    Dictionary<SoundType, AudioSource> audioSources = new Dictionary<SoundType, AudioSource>();
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private Dictionary<SoundType, float> audioVolume = new Dictionary<SoundType, float>();
    private void Awake()
    {
        SceneManager.sceneLoaded += whenSceneLoad;
        DontDestroyOnLoad(gameObject);

        //리소스 폴더안 Sounds폴더안에 오디오클립들을 모은다
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/");
        foreach (AudioClip clip in clips)
            audioClips[clip.name] = clip;

        //오디오 소스 생성후 사운드매니저에 상속
        for (SoundType i = 0; i < SoundType.END; i++)
        {
            GameObject SourceObj = new GameObject(i.ToString());
            audioSources[i] = SourceObj.AddComponent<AudioSource>();
            audioVolume[i] = 0.5f;
            SourceObj.transform.parent = transform;

            if (i == SoundType.BGM) audioSources[i].loop = true;
        }
    }
    public void PlaySound(string ClipName, SoundType type = SoundType.SE, float Volume = 1, float Pitch = 1)
    {
        switch (type)
        {
            case SoundType.BGM:
                audioSources[type].clip = audioClips[ClipName];
                audioSources[type].volume = Volume * audioVolume[type];
                audioSources[type].Play();
                break;
            default:
                audioSources[type].PlayOneShot(audioClips[ClipName], Volume * audioVolume[type]);
                break;
        }
        audioSources[type].pitch = Pitch;
    }
    private void whenSceneLoad(Scene level, LoadSceneMode mode)
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
