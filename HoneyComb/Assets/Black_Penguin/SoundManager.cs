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
        //���ҽ� ������ Sound�����ȿ� �����Ŭ������ ������
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sound/");
        foreach (AudioClip clip in clips)
        {
            audioClips[clip.name] = clip;
        }
        
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
    public void PlaySound(string ClipName, SoundType type = SoundType.SE, float Volume = 1,)
    {

    }

}
