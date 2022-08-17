using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum QuestType
{
    GO_FAST,

}

public abstract class BaseDailyQuest
{
    public int index;
    public abstract bool isClear();
    public int ReturnLevelIndex()
    {
        int value = 0;
        value += StatusManager.Instance.playableCharacterInfos.FindAll((x) => x.level >= 1).Count;
        value += StatusManager.Instance.stageInfos.FindAll((x) => x == true).Count;

        return value / 2;
    }
}
public class GoFaraway : BaseDailyQuest
{
    public override bool isClear()
    {
        return ReturnLevelIndex() * 5000 <= index;
    }
}
public class DailyQuest : MonoBehaviour
{

}
