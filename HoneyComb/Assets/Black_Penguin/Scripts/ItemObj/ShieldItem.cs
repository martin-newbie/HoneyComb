using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShieldItem : ItemBase
{
    public override void DestroyItem()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = FindObjectOfType<Player>();
            player.shieldDuration = 5;

            SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();
            sprite.DOFade(0.3f, 0);
            sprite.DOFade(1, 5);

            SoundManager.Instance.PlaySound("Button_Click2");
            Destroy(gameObject);
        }
    }
}
