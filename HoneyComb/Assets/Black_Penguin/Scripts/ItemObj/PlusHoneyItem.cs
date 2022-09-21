using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusHoneyItem : ItemBase
{
    public override void DestroyItem()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound("EarnItems");

            InGameManager.Instance.roundHoney += 25;
            SoundManager.Instance.PlaySound("Button_Click2");
            Destroy(gameObject);
        }
    }
}
