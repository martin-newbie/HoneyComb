using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerNPC : NpcBase
{
    protected override void FirstMeetScript()
    {
        TextAsset asset = Resources.Load("Texts/FirstMeet/" + path) as TextAsset;
        string[] message = asset.text.Split('\n');

        SpeechOn(message);
    }

    protected override void GetQuestAction()
    {
        StatusManager.Instance.gamePlayAble = true;
    }

    protected override void GetRewardAction()
    {
        
    }
}
