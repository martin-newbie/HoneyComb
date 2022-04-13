using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessNPC : NpcBase
{
    protected override void FirstMeetScript()
    {
        TextAsset asset = Resources.Load("Texts/FirstMeet/" + path) as TextAsset;
        string[] messages = asset.text.Split('\n');
        SpeechOn(messages);
    }

    protected override void GetQuestAction()
    {
    }

    protected override void GetRewardAction()
    {
    }
}
