using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : ItemBase
{
    public override void DestroyItem()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InGameManager.Instance.curObjectMoveSpeed += 1;
            SoundManager.Instance.PlaySound("Button_Click2");
            Destroy(gameObject);
        }
    }
}
