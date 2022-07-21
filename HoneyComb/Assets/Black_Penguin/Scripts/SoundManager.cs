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

        //���ҽ� ������ Sounds�����ȿ� �����Ŭ������ ������
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/");
        foreach (AudioClip clip in clips)
            audioClips[clip.name] = clip;

        //����� �ҽ� ������ ����Ŵ����� ���
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
        //���� �ҷ������� ����̳Ŀ� ���� ��������� ��ȯ�Ѵ�
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
