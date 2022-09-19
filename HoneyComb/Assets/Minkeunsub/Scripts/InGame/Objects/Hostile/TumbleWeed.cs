using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleWeed : Hostile
{
    public override float objectMoveSpeed => InGameManager.Instance.objectMoveSpeed + 3f;

    [Header("Objects")]
    public SpriteRenderer weed;

    [Header("Sprites")]
    public Sprite[] weedSprites;

    [Header("Value")]
    public float frameSpeed = 0.1f;
    public int spawnIdx = 0;

    int moveIdx = 1; // 1, -1
    Transform[] poses;

    private void Start()
    {
        StartCoroutine(SpriteAnimation(weed, weedSprites, frameSpeed));

        StartCoroutine(MoveLogic());
    }

    IEnumerator MoveLogic()
    {
        float t = 0f;
        int prevIdx = spawnIdx;
        int nextIdx = prevIdx + GetDir(prevIdx);

        while (true)
        {
            if (t > 1f)
            {
                prevIdx = nextIdx;
                int dir = GetDir(prevIdx);
                nextIdx += dir;
                moveIdx = dir;

                t = 0f;
            }

            t += Time.deltaTime * 3f;
            weed.transform.Rotate(Vector3.forward * moveIdx * Time.deltaTime * objectMoveSpeed * 100f);


            Vector3 pos = Vector3.Lerp(poses[prevIdx].position, poses[nextIdx].position, t);
            pos.y = transform.position.y;
            transform.position = pos;
            yield return null;
        }
    }

    int GetDir(int idx)
    {
        int d;
        if (idx == 2) d = -1;
        else if (idx == 0) d = 1;
        else d = Random.Range(0, 2) == 0 ? 1 : -1;
        return d;
    }

    public override void DestroyItem()
    {
        Destroy(gameObject);
    }

    void Init(int idx)
    {
        spawnIdx = idx;

        poses = InGameManager.Instance.PlayerPoses;
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

    public override void Init()
    {
        Init(Random.Range(0, 3));
    }
}
