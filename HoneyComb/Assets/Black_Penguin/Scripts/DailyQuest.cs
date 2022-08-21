using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DateTime = System.DateTime;

public enum QuestType
{
    DailyQuestGoFaraway,
    DailyQuestGetSomeHoney,
    DailyQuestHit,
    DailyQuestMove,
    DailyQuestPlayGame,
    DailyQuestCollectWax = 101,
    DailyQuestCollectBee,
    END
}

public class BaseDailyQuest
{
    public QuestType type;
    public EPlayableCharacter characterType;
    public EStageType stageType;
    public bool isCompleted;
    private int index;
    public int _index
    {
        get { return index; }
        set
        {
            if ((int)type < 100)
            {
                if (StatusManager.Instance.nowStage == stageType && StatusManager.Instance.nowCharacter == characterType)
                {
                    index = value;
                }
            }
            else
            {
                index = value;
            }
        }
    }
    public bool isClear()
    {
        switch (type)
        {
            case QuestType.DailyQuestGoFaraway:
                return ReturnLevelIndex() * 3000 <= index;
            case QuestType.DailyQuestGetSomeHoney:
                return ReturnLevelIndex() * 250 <= index;
            case QuestType.DailyQuestHit:
                return ReturnLevelIndex() * 20 <= index;
            case QuestType.DailyQuestMove:
                return ReturnLevelIndex() * 200 <= index;
            case QuestType.DailyQuestPlayGame:
                return ReturnLevelIndex() * 5 <= index;
            case QuestType.DailyQuestCollectWax:
                return 10 <= index;
            case QuestType.DailyQuestCollectBee:
                return 10 <= index;
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
public class DailyQuest : Singleton<DailyQuest>
{
    private string dailyQuestTimeSavePath = "DaillyQuestTime DataPath";
    private string dailyQuestSavePath = "DaillyQuest DataPath";

    public int distance;
    public int getHoneyCount;
    public int hitCount;
    public int moveCount;
    public int playCount;

    public int makingWaxCount;
    public int makingBeeCount;

    List<BaseDailyQuest> dailyQuests = new List<BaseDailyQuest>(3);
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        CheckTimeToReset();
    }

    /// <summary>
    /// 일일퀘스트는 새벽 4시마다 갱신된다
    /// </summary>
    private void CheckTimeToReset()
    {
        string lastTimeData = PlayerPrefs.GetString(dailyQuestTimeSavePath, "null");
        if (lastTimeData != "null")
        {
            DateTime nowtime = DateTime.Now;
            DateTime lastTime = JsonUtility.FromJson<DateTime>(lastTimeData);

            if (new DateTime(lastTime.Year, lastTime.Month, lastTime.Day + 1, 4, 0, 0) <= nowtime)
            {
                QuestReset(nowtime);
            }
            else
            {
                string dataLoadString = PlayerPrefs.GetString(dailyQuestSavePath, "null");
                if (dataLoadString != "null")
                {
                    dailyQuests = JsonUtility.FromJson<List<BaseDailyQuest>>(dataLoadString);
                }
                else
                {
                    Debug.Log("QUEST SAVE LOAD ERROR");
                }
            }
        }
        else
        {
            QuestReset(DateTime.Now);
        }
    }
    private void QuestInfoSave()
    {
        string dailyQuestDataString = JsonUtility.ToJson(dailyQuests);
        PlayerPrefs.SetString(dailyQuestSavePath, dailyQuestDataString);
    }
    private void QuestReset(DateTime nowTime)
    {
        distance = 0;
        hitCount = 0;
        moveCount = 0;
        playCount = 0;
        makingWaxCount = 0;
        makingBeeCount = 0;

        dailyQuests.Clear();
        for (int i = 0; i < 3; i++)
        {
            List<PlayableCharacterInfo> infos = StatusManager.Instance.playableCharacterInfos.FindAll((x) => x.level >= 1);
            EPlayableCharacter CharacterType = infos[Random.Range(0, infos.Count)].character;
            EStageType StageType;
            while (true)
            {
                int index = Random.Range(0, StatusManager.Instance.stageInfos.Count);
                if (StatusManager.Instance.stageInfos[index] == true)
                {
                    StageType = (EStageType)index;
                    break;
                }
            }

            while (dailyQuests.Count < 3)
            {
                QuestType questType = (QuestType)Random.Range(0, (int)QuestType.END);
                if (dailyQuests.Find((x) => x.type == questType) != null) continue;

                dailyQuests.Add(new BaseDailyQuest()
                {
                    type = questType,
                    stageType = StageType,
                    characterType = CharacterType
                });
            }
        }

        string dataString = JsonUtility.ToJson(nowTime);
        PlayerPrefs.SetString(dailyQuestTimeSavePath, dataString);

        QuestInfoSave();
    }
    private void OnApplicationQuit()
    {
        QuestInfoSave();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            QuestInfoSave();
        }
    }
}
[CreateAssetMenu(fileName = "DailyQuestInfo", menuName = "DailyQuestInfo", order = int.MinValue)]
public class DailyQuestUI_Info : ScriptableObject
{
    public QuestType questType;
    public string questName;
}
