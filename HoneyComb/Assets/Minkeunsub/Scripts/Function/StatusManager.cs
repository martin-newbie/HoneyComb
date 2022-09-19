using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public enum EStageType
{
    PLAIN,
    FOREST,
    TUNDRA,
    DESSERT,
    END
}

[System.Serializable]
public enum EPlayableCharacter
{
    HONEY_BEE,
    FAT_BEE,
    MEGA_BEE,
    DIRT_BEE,
    PUMPKIN_BEE,
    GARDENER_BEE,
    FABULOUS_BEE,
    CYBORG_BEE,
    MISCHIEF_BEE,
    ONI_BEE,
    HORSE_BEE,
    KING_BEE,
    KNIGHT_BEE,
    END
}
[Serializable]
public class PlayableCharacterInfo
{
    public EPlayableCharacter character;
    public bool IsHave
    {
        get
        {
            if (level >= 1)
                return true;
            else
                return false;
        }
    }
    public int level;
    private int pieceCount;
    public int _pieceCount
    {
        get { return pieceCount; }
        set
        {
            pieceCount = value;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        int requireCount = level * 20 + 10;
        if (_pieceCount >= requireCount)
        {
            level++;
            _pieceCount -= requireCount;
        }
    }
    public int ReturnRequireCount()
    {
        return level * 20 + 10;
    }
}
public class StatusManager : Singleton<StatusManager>
{
    public bool debug;
    public StatusSave dataSave = new StatusSave();
    public string dataSaveName = "status data save";
    public string questSaveName = "quest data save";
    public string timeSaveName = "time save";
    public string characterSaveName = "character save";
    public string stageSaveName = "stage save";
    public List<Book> BookDatas = new List<Book>();

    [Header("Bee")]
    public int Room = 20; //ÃÑ ¹æ °¹¼ö (¿©¿Õ ¹æ¿¡¼­ ¾÷±×·¹ÀÌµå)
    public int MaxBee = 15; // ÃÑ ¹ú °¹¼ö (¿©¿Õ ¹æ¿¡¼­ ¾÷±×·¹ÀÌµå)
    public int CurBee = 15;
    public float Charging = 1f;
    public float BeeDelay = 300f;
    public float curBeeDelay;

    [Header("Bee Wax")]
    public int BeeWax; //¹Ð¶ø
    public int QueueWax;
    public float WaxDelay = 60f;
    public float curWaxDelay;

    [Header("Status")]
    public int Honey; //²Ü
    public int royalGel;
    public bool[] SceneUnlock = new bool[3]; // 0: royal, 1: lab, 2: library
    public bool bookAble;
    public bool gamePlayAble;
    public bool beeUpgradeAble;
    public bool roomUpgradeAble;

    [Header("Quest")]
    public int CurQuestIdx;
    [HideInInspector]
    public QuestData CurQuest; // == head
    public List<QuestData> QuestsList = new List<QuestData>();
    public QuestDataSave QuestSaveList = new QuestDataSave();
    public bool isQuestAble;
    public List<Action> QuestClearActions = new List<Action>();

    [Header("Books")]
    public List<bool> BookUnlocked = new List<bool>(new bool[5] { true, true, true, true, true });

    [Header("PlayableCharacter")]
    public EPlayableCharacter nowCharacter;
    public List<PlayableCharacterInfo> playableCharacterInfos = new List<PlayableCharacterInfo>();

    [Header("Stage")]
    public List<bool> stageInfos = new List<bool>();
    public EStageType nowStage;

    void RemoveSaveData()
    {
        //just for debug  
        PlayerPrefs.DeleteKey(dataSaveName);
        PlayerPrefs.DeleteKey(stageSaveName);
        PlayerPrefs.DeleteKey(characterSaveName);
        PlayerPrefs.DeleteKey(questSaveName);
        PlayerPrefs.DeleteKey(timeSaveName);
    }

    void LoadQuest()
    {
        string questTmp = PlayerPrefs.GetString(questSaveName, "none");
        if (questTmp == "none")
        {
            QuestSaveList.QuestLists = QuestsList;

            string jsonSave = JsonUtility.ToJson(QuestSaveList, true);
            PlayerPrefs.SetString(questSaveName, jsonSave);
        }
        else
        {
            QuestSaveList = JsonUtility.FromJson<QuestDataSave>(questTmp);
            QuestsList = QuestSaveList.QuestLists;
        }

        if (CurQuestIdx < QuestsList.Count)
            CurQuest = QuestsList[CurQuestIdx];
    }

    private void Awake()
    {
        //load data first
        DontDestroyOnLoad(this.gameObject);
        if (debug) RemoveSaveData();
        LoadData();
        LoadBeeTime();
        LoadQuest();

        InitClearActions();
        DailyQuest.Instance.StartDailyQuest();
    }

    void InitClearActions()
    {
        QuestClearActions.Add(() => { SceneUnlock[0] = true; });
        QuestClearActions.Add(() => { SceneUnlock[1] = true; });
        QuestClearActions.Add(() => { roomUpgradeAble = true; });
        QuestClearActions.Add(() => { SceneUnlock[2] = true; });
        QuestClearActions.Add(() => { bookAble = true; });
    }

    private void Update()
    {
        isQuestAble = CurQuestIdx < QuestsList.Count;
        if (isQuestAble)
            CurQuest = QuestsList[CurQuestIdx];

        BeeCharging();
        WaxCharging();
    }

    void WaxCharging()
    {
        if (QueueWax > 0)
        {
            curWaxDelay -= Time.deltaTime * Charging;

            if (curWaxDelay <= 0f)
            {
                QueueWax--;
                BeeWax++;
                curWaxDelay += WaxDelay;
            }
        }
        else
        {
            curWaxDelay = WaxDelay;
        }
    }

    void BeeCharging()
    {
        if (CurBee < MaxBee && SceneManager.GetActiveScene().name.Equals("InGameScene") == false)
        {
            curBeeDelay += Time.deltaTime * Charging;

            if (curBeeDelay >= BeeDelay)
            {
                CurBee++;
                curBeeDelay = 0f;
            }
        }
        else curBeeDelay = -0.01f;
    }
    public void LoadData()
    {
        string dataStr = PlayerPrefs.GetString(dataSaveName, "none");

        if (dataStr != "none")
        {
            StatusSave temp = JsonUtility.FromJson<StatusSave>(dataStr);
            if (temp != null)
            {
                dataSave = temp;
                GetDataFromSave();
            }
        }
        else
        {
            int count = (int)EPlayableCharacter.END;
            for (int i = 0; i < count; i++)
            {
                PlayableCharacterInfo character = new PlayableCharacterInfo() { character = (EPlayableCharacter)i };
                playableCharacterInfos.Add(character);
                if (i == 0)
                {
                    playableCharacterInfos[0].level = 1;
                }
            }
        }

    }

    void GetDataFromSave()
    {
        Room = dataSave.Room;
        MaxBee = dataSave.MaxBee;
        CurBee = dataSave.CurBee;
        Honey = dataSave.Honey;
        BeeWax = dataSave.BeeWax;
        curBeeDelay = dataSave.curBeeDelay;
        QueueWax = dataSave.QueueWax;
        curWaxDelay = dataSave.curWaxDelay;
        CurQuestIdx = dataSave.CurQuestIdx;
        SceneUnlock = dataSave.SceneUnlock;
        bookAble = dataSave.bookAble;
        gamePlayAble = dataSave.gamePlayAble;
        beeUpgradeAble = dataSave.beeUpgradeAble;
        roomUpgradeAble = dataSave.roomUpgradeAble;
        BookUnlocked = dataSave.BookUnlocked;
        playableCharacterInfos = dataSave.CharactersInfo;
        stageInfos = dataSave.stageInfo;
        nowCharacter = dataSave.nowCharacter;
    }

    void SetDataToSave()
    {
        dataSave.Room = Room;
        dataSave.MaxBee = MaxBee;
        dataSave.CurBee = CurBee;
        dataSave.Honey = Honey;
        dataSave.BeeWax = BeeWax;
        dataSave.curBeeDelay = curBeeDelay;
        dataSave.QueueWax = QueueWax;
        dataSave.curWaxDelay = curWaxDelay;
        dataSave.CurQuestIdx = CurQuestIdx;
        dataSave.SceneUnlock = SceneUnlock;
        dataSave.bookAble = bookAble;
        dataSave.gamePlayAble = gamePlayAble;
        dataSave.beeUpgradeAble = beeUpgradeAble;
        dataSave.roomUpgradeAble = roomUpgradeAble;
        dataSave.BookUnlocked = BookUnlocked;
        dataSave.CharactersInfo = playableCharacterInfos;
        dataSave.stageInfo = stageInfos;
        dataSave.nowCharacter = nowCharacter;
    }

    public void SaveData()
    {
        SetDataToSave();

        string saveStr = JsonUtility.ToJson(dataSave, true);
        PlayerPrefs.SetString(dataSaveName, saveStr);
    }
    public void SaveCharacterData()
    {
        string saveStr = JsonUtility.ToJson(playableCharacterInfos, true);
        PlayerPrefs.SetString(characterSaveName, saveStr);
    }

    void LoadBeeTime()
    {
        string timeStr = PlayerPrefs.GetString(timeSaveName, "none");
        if (timeStr != "none")
        {
            DateTime endTime = Convert.ToDateTime(timeStr);

            DateTime curTime = DateTime.Now;
            TimeSpan timeDif = curTime - endTime;
            float f_timeDif = (float)timeDif.TotalSeconds;

            curWaxDelay -= f_timeDif;

            int beeCount = (int)(f_timeDif / BeeDelay);
            curBeeDelay += f_timeDif % BeeDelay;
            CurBee += beeCount;

            if (CurBee > MaxBee) CurBee = MaxBee;
        }
        else CurBee = MaxBee;
    }
    void SaveStageInfo()
    {
        string saveData = JsonUtility.ToJson(stageInfos);
        PlayerPrefs.SetString(stageSaveName, saveData);
    }
    void SaveBeeTime()
    {
        DateTime endTime = DateTime.Now;
        PlayerPrefs.SetString(timeSaveName, endTime.ToString());
    }

    void SaveQuestData()
    {
        QuestSaveList.QuestLists = QuestsList;
        string jsonSave = JsonUtility.ToJson(QuestSaveList, true);
        PlayerPrefs.SetString(questSaveName, jsonSave);
    }

    public void DataSave()
    {
        SaveData();
        SaveQuestData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            DataSave();
            SaveBeeTime();
            SaveStageInfo();
            SaveCharacterData();
        }
        else
        {
            LoadBeeTime();
        }
    }

    private void OnApplicationQuit()
    {
        DataSave();
        SaveBeeTime();
        SaveStageInfo();
    }
}

[Serializable]
public class StatusSave
{
    public int Room;
    public int MaxBee;
    public int CurBee;
    public int Honey;
    public int BeeWax;
    public float curBeeDelay;
    public int QueueWax;
    public int CurQuestIdx;
    public float curWaxDelay;
    public bool[] SceneUnlock = new bool[3];
    public bool bookAble;
    public bool gamePlayAble;
    public bool beeUpgradeAble;
    public bool roomUpgradeAble;
    public List<bool> BookUnlocked = new List<bool>();
    public List<PlayableCharacterInfo> CharactersInfo = new List<PlayableCharacterInfo>();
    public List<bool> stageInfo = new List<bool>();
    public EPlayableCharacter nowCharacter;
}

public enum QuestNpcState
{
    Worker,
    RoyalBee,
    LabBee,
    LibraryBee
}

public enum QuestValueKind
{
    Honey,
    Bee,
    Wax,
    Room
}

[Serializable]
public class QuestDataSave
{
    public List<QuestData> QuestLists = new List<QuestData>();
}

[Serializable]
public class QuestData
{
    public string questName;

    public QuestNpcState thisState;
    public QuestValueKind thisKind;
    public QuestValueKind rewardKind;

    public bool isCleared = false;
    public bool QuestActive = false;

    public int defaultValue;
    public int curValue;
    public int maxValue;

    public string textId;

    public bool comicAble = true;
    [DrawIf("comicAble", true)] public int comicIdx;

    public int Reward;

    public string SetQuestName(int idx)
    {
        TextAsset asset = Resources.Load("Texts/QuestName") as TextAsset;
        string[] temp = asset.text.Split('\n');
        questName = temp[idx];
        return questName;
    }

    public void SetDefaultValue()
    {
        switch (thisKind)
        {
            case QuestValueKind.Honey:
                defaultValue = StatusManager.Instance.Honey;
                break;
            case QuestValueKind.Bee:
                defaultValue = StatusManager.Instance.MaxBee;
                break;
            case QuestValueKind.Wax:
                defaultValue = StatusManager.Instance.BeeWax;
                break;
            case QuestValueKind.Room:
                defaultValue = StatusManager.Instance.Room;
                break;
        }

        QuestActive = true;
    }

    public void SetValue()
    {
        switch (thisKind)
        {
            case QuestValueKind.Honey:
                curValue = StatusManager.Instance.Honey - defaultValue;
                break;
            case QuestValueKind.Bee:
                curValue = StatusManager.Instance.MaxBee - defaultValue;
                break;
            case QuestValueKind.Wax:
                curValue = StatusManager.Instance.BeeWax - defaultValue;
                break;
            case QuestValueKind.Room:
                curValue = StatusManager.Instance.Room - defaultValue;
                break;
        }
    }

    public string[] GetQuestTextScript()
    {
        TextAsset asset = Resources.Load("Texts/QuestScripts/" + textId) as TextAsset;
        string[] ret = asset.text.Split('\n');
        return ret;
    }

    public string[] GetQuestClearScript()
    {
        TextAsset asset = Resources.Load("Texts/QuestClear/" + textId) as TextAsset;
        string[] ret = asset.text.Split('\n');
        return ret;
    }

    public bool CheckIsClear()
    {
        bool ret;

        if (curValue >= maxValue) ret = true;
        else ret = false;
        isCleared = ret;

        return isCleared;
    }

    public void GetReward(Action action = null)
    {
        QuestActive = false;
        switch (rewardKind)
        {
            case QuestValueKind.Honey:
                StatusManager.Instance.Honey += Reward;
                break;
            case QuestValueKind.Wax:
                StatusManager.Instance.BeeWax += Reward;
                break;
        }

        action?.Invoke();
    }
}