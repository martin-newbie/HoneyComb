using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    DailyQuestGoFaraway,
    DailyQuestGetSomeHoney,
    DailyQuestHit,
    DailyQuestMove,
    DailyQuestPlayGame,
    END
}

public abstract class BaseDailyQuest
{
    public QuestType type;
    public int index;
    public bool isClear()
    {
        switch (type)
        {
            case QuestType.DailyQuestGoFaraway:
                return ReturnLevelIndex() * 5000 <= index;
            case QuestType.DailyQuestGetSomeHoney:
                return ReturnLevelIndex() * 250 <= index;
            case QuestType.DailyQuestHit:
                return ReturnLevelIndex() * 50 <= index;
            case QuestType.DailyQuestMove:
                return ReturnLevelIndex() * 300 <= index;
            case QuestType.DailyQuestPlayGame:
                return ReturnLevelIndex() * 30 <= index;
            default:
                Debug.Log("DailyQuestLog Eror");
                break;
        }
        return false;
    }
    public int ReturnLevelIndex()
    {
        int value = 0;
        value += StatusManager.Instance.playableCharacterInfos.FindAll((x) => x.level >= 1).Count;
        value += StatusManager.Instance.stageInfos.FindAll((x) => x == true).Count;

        return value / 2;
    }
}
public class DailyQuest : MonoBehaviour
{
    public int distance;
    public int getHoneyCount;
    public int hitCount;
    void Update()
    {

    }
    private void Start()
    {
        List<BaseDailyQuest> baseDailyQuests = new List<BaseDailyQuest>();
    }
}
