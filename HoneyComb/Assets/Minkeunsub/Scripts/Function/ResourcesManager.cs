using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : Singleton<ResourcesManager>
{
    public Sprite[] sprites;
    public List<Sprite> Player9Angry;
    public List<Sprite[]> PlayerSprites = new List<Sprite[]>();

    public Image fillImg;
    public Text fillTxt;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadSprite();
        SceneLoadManager.Instance.LoadScene("TitleScene");
    }

    void LoadSprite()
    {
        sprites = Resources.LoadAll<Sprite>("PlayableCharacter/Animation/");

        int cycleCount = sprites.Length / 60;
        for (int i = 0; i < cycleCount; i++)
        {
            List<Sprite> temp = new List<Sprite>();
            for (int j = i * 60; j < i * 60 + 60; j++)
            {
                temp.Add(sprites[j]);
            }

            PlayerSprites.Add(temp.ToArray());
        }

    }
}

