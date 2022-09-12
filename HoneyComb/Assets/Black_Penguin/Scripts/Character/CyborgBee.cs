using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgBee : Player
{
    private float abilityColldown = 30;
    private float curAbilityColldown;
    protected override void Update()
    {
        base.Update();
        curAbilityColldown += Time.deltaTime;
        if (curAbilityColldown > abilityColldown)
        {
            curAbilityColldown = 0;
            AbilityUse();
        }
    }
    void AbilityUse()
    {
        foreach (Obstruction obstruction in FindObjectsOfType<Obstruction>())
        {
            obstruction.DestroyItem();
        }
    }
}
