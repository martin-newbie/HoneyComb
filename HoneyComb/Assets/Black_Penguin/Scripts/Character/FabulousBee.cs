using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabulousBee : Player
{
    protected override void Start()
    {
        base.Start();
        InGameManager.Instance.moveSpeed *= 1.4f;
    }
}
