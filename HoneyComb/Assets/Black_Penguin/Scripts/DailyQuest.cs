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
public class DailyQuestGoFaraway : BaseDailyQuest
{
    public override bool isClear() => ReturnLevelIndex() * 5000 <= index;
}
public class DailyQuestGetSomeHoney : BaseDailyQuest
{
    public override bool isClear() => ReturnLevelIndex() * 250 <= index;
}
public class DailyQuestHit : BaseDailyQuest
{
    public override bool isClear() => ReturnLevelIndex() * 50 <= index;
}
public class DailyQuestMove : BaseDailyQuest
{
    public override bool isClear() => ReturnLevelIndex() * 399 <= index;
}
public class DailyQuestPlayGame : BaseDailyQuest
{
    public override bool isClear() => ReturnLevelIndex() * 30 <= index;
}
public class DailyQuest : MonoBehaviour
{
    void Update()
    {

    }
    private void Start()
    {
        List<BaseDailyQuest> baseDailyQuests = new List<BaseDailyQuest>();
        baseDailyQuests.Add(new DailyQuestPlayGame());
        baseDailyQuests.Add(new DailyQuestGetSomeHoney());
        baseDailyQuests.Add(new DailyQuestGoFaraway());
        baseDailyQuests.Add(new DailyQuestMove());
        baseDailyQuests.Add(new DailyQuestHit());
    }
}
