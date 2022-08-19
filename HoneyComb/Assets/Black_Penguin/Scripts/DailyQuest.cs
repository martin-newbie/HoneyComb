using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private string receiveDailyQuestTimeString = "DaillyQuest DataString";

    public int distance;
    public int getHoneyCount;
    public int hitCount;
    public int moveCount;
    public int playCount;

    public int makingWaxCount;
    public int makingBeeCount;
    private void Start()
    {
        CheckTimeToReset();
        List<BaseDailyQuest> baseDailyQuests = new List<BaseDailyQuest>();
    }

    /// <summary>
    /// 일일퀘스트는 새벽 4시마다 갱신된다
    /// </summary>
    void CheckTimeToReset()
    {
        string lastTime = PlayerPrefs.GetString(receiveDailyQuestTimeString, "null");
        if (lastTime != "null")
        {
            string[] nowtime = DateTime.Now.ToString("yyyy-MM-dd-HH").Split('-');
            string dataString = JsonUtility.ToJson(nowtime);
            PlayerPrefs.SetString(receiveDailyQuestTimeString, dataString);
        }
        else
        {
            QuestReset();
        }

    }
    void QuestReset()
    {
        distance = 0;
        hitCount = 0;
        moveCount = 0;
        playCount = 0;
        makingWaxCount = 0;
        makingBeeCount = 0;
    }
}
