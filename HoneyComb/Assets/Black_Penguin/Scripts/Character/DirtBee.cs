using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DirtBee : Player
{
    protected override IEnumerator OnDamage()
    {
        isInvincible = true;
        Hp -= InGameManager.Instance.damage / 3;
        SoundManager.Instance.PlaySound("Hit", SoundType.SE, 0.2f);
        SoundManager.Instance.PlaySound("Hit2", SoundType.SE);
        InGameManager.Instance.CameraShake(0.3f);
        yield return new WaitForSeconds(invincibleDelay);
        isInvincible = false;
    }


}
