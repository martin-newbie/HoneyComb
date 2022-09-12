using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public int animationIndex = 0;
    public float frameSpeed = 0.01f;
    Sprite[] animationSprites;
    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animationSprites = ResourcesManager.Instance.PlayerSprites[animationIndex];
        StartCoroutine(AnimationCoroutine());
    }

    IEnumerator AnimationCoroutine()
    {
        int idx = 0;
        WaitForSeconds waitSeconds = new WaitForSeconds(frameSpeed);

        while (true)
        {
            sprite.sprite = animationSprites[idx];


            if (idx < animationSprites.Length - 1) idx++;
            else idx = 0;

            yield return waitSeconds;
        }
    }
}
