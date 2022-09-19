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
        int dir = moveIdx;
        float t = 0f;

        while (true)
        {
            if(t > 1f || t < 0f)
            {
                int d;
                if (spawnIdx == 2) d = -1;
                else if (spawnIdx == 0) d = 1;
                else d = Random.Range(0, 2) == 0 ? 1 : -1;

                dir *= d;
            }

            t += Time.deltaTime * dir;

            weed.transform.Rotate(Vector3.forward * dir * Time.deltaTime * objectMoveSpeed * 100f);

            Vector3 pos = Vector3.Lerp(poses[spawnIdx].position, poses[spawnIdx + moveIdx].position, t);
            pos.y = transform.position.y;
            transform.position = pos;
            yield return null;
        }
    }

    public override void DestroyItem()
    {
        Destroy(gameObject);
    }

    void Init(int idx)
    {
        spawnIdx = idx;

        poses = InGameManager.Instance.PlayerPoses;
        moveIdx = Random.Range(0, 2) == 0 ? 1 : -1;
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
