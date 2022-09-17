using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : StageBase
{
    [Header("Stage 2")]
    public ShadowBackground shadow;

    public override Background SpawnBackground(int idx, float height, InGameManager manager)
    {
        ShadowBackground temp = Instantiate(shadow);
        temp.Init(height * 4, height * -2, manager);
        temp.transform.position = new Vector3(0, idx * height * 2, 0);

        InGameManager.Instance.SetSpriteCameraSize(temp.GetComponent<SpriteRenderer>());

        return base.SpawnBackground(idx, height, manager);
    }
}
