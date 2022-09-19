using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Hostile
{
    public override float objectMoveSpeed => InGameManager.Instance.objectMoveSpeed + 5f;

    [Header("Objects")]
    public SpriteRenderer log;
    public SpriteRenderer logSplash;

    [Header("Sprites")]
    public Sprite[] logSprites;
    public Sprite[] splashSprites;

    [Header("Value")]
    public float frameSpeed = 0.1f;

    private void Start()
    {
        StartCoroutine(SpriteAnimation(log, logSprites, frameSpeed));
        StartCoroutine(SpriteAnimation(logSplash, splashSprites, frameSpeed));

        StartCoroutine(MoveLogic());
    }

    IEnumerator MoveLogic()
    {
        while (true)
        {
            transform.Translate(Vector3.down * Time.deltaTime * objectMoveSpeed);
            yield return null;
        }
    }

    IEnumerator SpriteAnimation(SpriteRenderer renderer, Sprite[] frame, float frameSpeed)
    {
        int idx = 0;
        while (true)
        {
            renderer.sprite = frame[idx];
            yield return new WaitForSeconds(frameSpeed);

            if (idx < frame.Length - 1) idx++;
            else idx = 0;
        }
    }

    public override void DestroyItem()
    {
        Destroy(gameObject);
    }

    public override void Init()
    {
    }
}
