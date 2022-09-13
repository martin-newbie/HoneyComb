using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingBee : Player
{
    public override void GetHoney()
    {
        base.GetHoney();
        InGameManager.Instance.roundHoney++;
    }
    protected override IEnumerator OnDamage()
    {
        Hp -= InGameManager.Instance.damage;
        return base.OnDamage();
    }
}
