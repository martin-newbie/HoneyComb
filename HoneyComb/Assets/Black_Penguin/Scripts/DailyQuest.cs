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
            if (value < index)
            {
                index = value;
                return;
            }
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
        if (GetClearCount() <= _index) return true;
        else return false;
    }
    public int GetClearCount()
    {
        switch (type)
        {
            case QuestType.DailyQuestGoFaraway:
                return ReturnLevelIndex() * 3000;
            case QuestType.DailyQuestGetSomeHoney:
                return ReturnLevelIndex() * 250;
            case QuestType.DailyQuestHit:
                return ReturnLevelIndex() * 20;
            case QuestType.DailyQuestMove:
                return ReturnLevelIndex() * 200;
            case QuestType.DailyQuestPlayGame:
                return ReturnLevelIndex() * 5;
            case QuestType.DailyQuestCollectWax:
                return 10;
            case QuestType.DailyQuestCollectBee:
                return 10;
            default:
                Debug.Log("DailyQuestLog Eror");
                break;
        }
        return 0;
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

    public int distance
    {
        get
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestGoFaraway);
            if (quest != null)
                return quest._index;
            else return 0;
        }
        set
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestGoFaraway);
            if (quest != null)
                quest._index = value;
        }
    }
    public int getHoneyCount
    {
        get
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestGetSomeHoney);
            if (quest != null)
                return quest._index;
            else
                return 0;
        }
        set
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestGetSomeHoney);
            if (quest != null)
                quest._index = value;
        }
    }
    public int hitCount
    {
        get
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestHit);
            if (quest != null)
                return quest._index;
            else
                return 0;
        }
        set
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestHit);
            if (quest != null)
                quest._index = value;
        }
    }
    public int moveCount
    {
        get
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestMove);
            if (quest != null)
                return quest._index;
            else
                return 0;
        }
        set
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestMove);
            if (quest != null)
                quest._index = value;
        }
    }
    public int playCount
    {
        get
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestPlayGame);
            if (quest != null)
                return quest._index;
            else
                return 0;
        }
        set
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestPlayGame);
            if (quest != null)
                quest._index = value;
        }
    }
    public int makingWaxCount
    {
        get
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestCollectWax);
            if (quest != null)
                return quest._index;
            else
                return 0;
        }
        set
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestCollectWax);
            if (quest != null)
                quest._index = value;
        }
    }
    public int makingBeeCount
    {
        get
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestCollectBee);
            if (quest != null)
                return quest._index;
            else
                return 0;
        }
        set
        {
            BaseDailyQuest quest = dailyQuests.Find((x) => x.type == QuestType.DailyQuestCollectBee);
            if (quest != null)
                quest._index = value;
        }
    }

    public List<BaseDailyQuest> dailyQuests = new List<BaseDailyQuest>(3);
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

