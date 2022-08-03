using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatBee : Player
{
    protected override void Start()
    {
        MaxHp *= 1.4f;
        base.Start();
    }
}
