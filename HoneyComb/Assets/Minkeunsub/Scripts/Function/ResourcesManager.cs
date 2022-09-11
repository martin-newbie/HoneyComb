using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourcesManager : Singleton<ResourcesManager>
{
    public Sprite[] sprites;
    public List<Sprite[]> PlayerSprites = new List<Sprite[]>();
    public Sprite[] Player9Angry;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        sprites = Addressables.LoadAssetsAsync<Sprite>("PlayerSprite", null).WaitForCompletion().ToArray();

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

