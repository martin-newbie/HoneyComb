using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniBee : Player
{
    [SerializeField] private int transformCount = 10;
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
            curTransformCount = 0;
            isTransform = true;

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