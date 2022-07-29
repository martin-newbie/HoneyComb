using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    public float MaxHp;
    public float Hp;
    public bool isGameOver;

    public float invincibleDelay;
    public bool isInvincible;

    private StatusManager statusManager;
    [SerializeField] ParticleSystem HoneyParticle;

    void Start()
    {
        statusManager = StatusManager.Instance;

        int level = statusManager.playableCharacterInfos.Find((x) => x.character == statusManager.nowCharacter).level;
        if (level > 1)
        {
            MaxHp += ( level - 1) * 15;
        }

        Hp = MaxHp;
    }

    void Update()
    {
        if (!isGameOver)
        {
            HpLogic();
        }
    }

    void HpLogic()
    {
        if (Hp > 0)
        {
            Hp -= Time.deltaTime;
            InGameUI.Instance.SetPlayerHp(Hp / MaxHp);
        }
        else if (Hp <= 0 && !isGameOver)
        {
            isGameOver = true;
            InGameManager.Instance.GameOver();
        }
    }

    public void GetHoney()
    {
        HoneyParticle.Play();
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
        Hp -= InGameManager.Instance.damage;
        SoundManager.Instance.PlaySound("Hit", SoundType.SE, 0.2f);
        SoundManager.Instance.PlaySound("Hit2", SoundType.SE);
        InGameManager.Instance.CameraShake(0.3f);
        yield return new WaitForSeconds(invincibleDelay);
        isInvincible = false;
    }
}
