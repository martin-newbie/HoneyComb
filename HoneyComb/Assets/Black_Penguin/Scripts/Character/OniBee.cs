using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniBee : Player
{
    [SerializeField] private int transformCount = 10;
    [SerializeField] private int curTransformCount = 10;
    [SerializeField] private bool isTransform;
    protected override void Start()
    {
        base.Start();
    }
    public override void GetHoney()
    {
        base.GetHoney();
        if (isTransform) return;

        curTransformCount++;
        if (transformCount >= curTransformCount)
        {
            curTransformCount = 0;
            isTransform = true;

        }
    }
}