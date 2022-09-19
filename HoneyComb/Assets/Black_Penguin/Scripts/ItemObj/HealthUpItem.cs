using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpItem : ItemBase
{
    public override void DestroyItem()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<Player>().Hp += 5;
            SoundManager.Instance.PlaySound("Button_Click2");
            Destroy(gameObject);
        }
    }
}
