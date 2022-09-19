using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyItem : ItemBase
{
    public int value;
    [SerializeField] List<SpriteRenderer> Flowers = new List<SpriteRenderer>();
    [SerializeField] GameObject Honey;
    bool collectAble = true;

    void Start()
    {
        int rand = Random.Range(0, Flowers.Count);
        for (int i = 0; i < Flowers.Count; i++)
        {
            if (i == rand) Flowers[i].gameObject.SetActive(true);
            else Flowers[i].gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collectAble)
        {
            SoundManager.Instance.PlaySound("Button_Click2");
            Honey.SetActive(false);
            InGameManager.Instance.roundHoney += value;
            InGameManager.Instance.Player.GetHoney();
            collectAble = false;
        }
    }

    public override void DestroyItem()
    {
        Destroy(gameObject);
    }
}
