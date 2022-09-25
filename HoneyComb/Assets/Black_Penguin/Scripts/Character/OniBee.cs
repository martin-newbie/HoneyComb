using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniBee : Player
{
    [SerializeField] private int transformCount = 40;
    [SerializeField] private int curTransformCount = 0;
    [SerializeField] private bool isTransform;

    private Sprite[] transformingSprites;
    protected override void Start()
    {
        base.Start();
        transformingSprites = ResourcesManager.Instance.Player9Angry.ToArray();
    }
    public override void GetHoney()
    {
        base.GetHoney();
        if (isTransform) return;

        curTransformCount++;
        if (curTransformCount >= transformCount)
        {
            SoundManager.Instance.PlaySound("oniBeeTransform", SoundType.SE, 1, 1.5f);
            curTransformCount = 0;
            isTransform = true;
            InGameManager.Instance.curObjectMoveSpeed += 3;

            TryGetComponent(out PlayerAnimation anim);

            Sprite[] sprites = anim.animationSprites;
            anim.animationSprites = transformingSprites;
            transformingSprites = sprites;
        }
    }
    protected override IEnumerator OnDamage()
    {
        if (isTransform)
        {
            TryGetComponent(out PlayerAnimation anim);
            InGameManager.Instance.curObjectMoveSpeed -= 3;

            SoundManager.Instance.PlaySound("Hit2", SoundType.SE);
            Sprite[] sprites = anim.animationSprites;
            anim.animationSprites = transformingSprites;
            transformingSprites = sprites;

            isTransform = false;
            yield break;
        }
        else
            yield return base.OnDamage();
    }
}