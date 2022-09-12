using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruction : Hostile
{

    [SerializeField] List<SpriteRenderer> Obj = new List<SpriteRenderer>();

    public override void DestroyItem()
    {
        Destroy(gameObject);
    }

    public override void Init()
    {
        Obj.ForEach(item => item.gameObject.SetActive(false));
        int rand = Random.Range(0, Obj.Count);
        Obj[rand].gameObject.SetActive(true);
    }
}
