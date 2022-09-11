using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IChracterHaveSpecialSkill
{
    public void SpecialSkill();
}
public class Player : MonoBehaviour
{
    public float MaxHp;
    public float Hp;
    public bool isGameOver;

    public float invincibleDelay;
    public bool isInvincible;

    public EPlayableCharacter characterType;

    public Vector2 checkBox;
    public Transform hitPos;

    protected Animator animator;
    private StatusManager statusManager;
    private ParticleSystem HoneyParticle;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        HoneyParticle = transform.GetComponentInChildren<ParticleSystem>();
        statusManager = StatusManager.Instance;

        int level = statusManager.playableCharacterInfos.Find((x) => x.character == statusManager.nowCharacter).level;
        if (level > 1)
        {
            MaxHp += (level - 1) * 15;
        }

        Hp = MaxHp;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(hitPos.position, checkBox);
    }

    protected virtual void Update()
    {
        HpLogic();
    }

    void HpLogic()
    {
        InGameUI.Instance.SetPlayerHp(Hp / MaxHp);

        if (!isGameOver)
        {
            Hp -= Time.deltaTime;
            CheckGameOver();
        }
    }

    public virtual void GetHoney()
    {
        HoneyParticle.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstruction") && !isInvincible)
        {
            if (Physics2D.OverlapBox(hitPos.position, checkBox, 0f, LayerMask.GetMask("Hostile")))
            {
                StartCoroutine(OnDamage());
                DailyQuest.Instance.hitCount++;
            }
        }
    }

    void CheckGameOver()
    {
        if (Hp < 0)
        {
            Hp = 0;

            if (!isGameOver)
            {
                isGameOver = true;
                InGameManager.Instance.GameOver();
            }
        }
    }

    protected virtual IEnumerator OnDamage()
    {
        isInvincible = true;

        Hp -= InGameManager.Instance.damage;
        CheckGameOver();

        SoundManager.Instance.PlaySound("Hit", SoundType.SE, 0.2f);
        SoundManager.Instance.PlaySound("Hit2", SoundType.SE);
        InGameManager.Instance.CameraShake(0.3f);
        yield return new WaitForSeconds(invincibleDelay);
        isInvincible = false;
    }
}
