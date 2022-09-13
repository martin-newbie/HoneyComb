using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookItem : ItemBase
{
    public override void DestroyItem()
    {
        Destroy(gameObject);
    }

    protected override void Update()
    {
        if (transform.position.y <= -6f)
        {
            DestroyItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InGameManager.Instance.BookGet();
            Destroy(gameObject);
        }
    }
}
