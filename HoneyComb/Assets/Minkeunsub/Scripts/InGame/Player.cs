using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float invincibleDelay;
    public bool isInvincible;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstruction") && !isInvincible)
        {
            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        isInvincible = true;
        InGameManager.Instance.CameraShake(0.3f);
        yield return new WaitForSeconds(invincibleDelay);
        isInvincible = false;
    }
}
