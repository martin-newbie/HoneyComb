using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : Hostile
{
    public override float objectMoveSpeed => InGameManager.Instance.objectMoveSpeed + 3f;

    [Header("Objects")]
    public SpriteRenderer snowBall;
    public SpriteRenderer snowSplash;

    [Header("Sprites")]
    public Sprite[] ballSprites;
    public Sprite[] splashSprites;

    [Header("Value")]
    public float frameSpeed = 0.1f;
    public int spawnIdx = 0;

    int moveIdx = 1; // 1, -1
    Transform[] poses;


    private void Start()
    {
        StartCoroutine(SpriteAnimation(snowBall, ballSprites, frameSpeed));
        StartCoroutine(SpriteAnimation(snowSplash, splashSprites, frameSpeed));

        StartCoroutine(MoveLogic());
    }

    protected void Init(int idx)
    {
        spawnIdx = idx;
        poses = InGameManager.Instance.PlayerPoses;

        if (spawnIdx == 0) moveIdx = 1;
        else if (spawnIdx == 2) moveIdx = -1;
        else moveIdx = Random.Range(0, 2) == 0 ? 1 : -1;
    }

    IEnumerator MoveLogic()
    {
        int dir = moveIdx;
        float t = 0f;

        while (true)
        {
            if (t > 1f && dir == 1) dir = -1;
            else if (t < 0f && dir == -1) dir = 1;

            t += Time.deltaTime * dir;

            Vector3 pos = Vector3.Lerp(poses[spawnIdx].position, poses[spawnIdx + moveIdx].position, t);
            pos.y = transform.position.y;
            transform.position = pos;
            yield return null;
        }
    }

    protected override void Update()
    {
        base.Update();
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
        Init(Random.Range(0, 3));
    }
}
