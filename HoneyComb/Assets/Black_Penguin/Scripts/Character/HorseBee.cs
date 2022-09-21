using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseBee : Player
{
    protected override void Start()
    {
        base.Start();
        InGameManager.Instance.curObjectMoveSpeed = 7f;
    }
}
