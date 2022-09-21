using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBee : Player
{
    public GameObject knightSword;
    private Animator animator;

    private int HoneyCount;
    protected override void Start()
    {
        base.Start();
        animator = knightSword.GetComponent<Animator>();
    }
    protected override void Update()
    {
        base.Update();
        if (HoneyCount >= 10)
        {
            SoundManager.Instance.PlaySound("SwordSwing");
            knightSword.SetActive(true);
            HoneyCount = 0;
        }
    }
    public override void GetHoney()
    {
        base.GetHoney();
        HoneyCount++;
    }

}
