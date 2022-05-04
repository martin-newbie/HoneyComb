using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGM,
    SE
}

public class SoundManager : Singleton<SoundManager>
{
    Dictionary<SoundType, AudioSource> audioSources;
    Dictionary<string, AudioClip> audioClips;
    private Dictionary<SoundType, float> audioVolume;

    public Dictionary<SoundType, float> _audioVolume
    {
        get { return audioVolume; }
        set
        {
            audioVolume = value;
        }
    }

    private void Awake()
    {
        audioVolume[SoundType.SE] = 0.5f;
        //���ҽ� ������ Sounds�����ȿ� �����Ŭ������ ������
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/");
        foreach (AudioClip clip in clips)
            audioClips[clip.name] = clip;

        //����� �ҽ� ������ ����Ŵ����� ���
        GameObject BgmObj = new GameObject();
        audioSources[SoundType.BGM] = BgmObj.AddComponent<AudioSource>();
        BgmObj.transform.parent = this.gameObject.transform;
        audioSources[SoundType.BGM].loop = true;
        audioVolume[SoundType.BGM] = 0.5f;

        GameObject SeObj = new GameObject();
        audioSources[SoundType.SE] = SeObj.AddComponent<AudioSource>();
        SeObj.transform.parent = this.gameObject.transform;
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

}
