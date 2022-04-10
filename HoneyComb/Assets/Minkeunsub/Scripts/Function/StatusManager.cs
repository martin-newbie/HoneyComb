using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatusManager : Singleton<StatusManager>
{
    public StatusSave dataSave = new StatusSave();
    public string dataSaveName = "status data save";
    public string questSaveName = "quest data save";
    public string timeSaveName = "time save";

    [Header("Bee")]
    public int Room = 15;
    public int MaxBee = 15; // ÃÑ ¹ú °¹¼ö (¿©¿Õ ¹æ¿¡¼­ ¾÷±×·¹ÀÌµå)
    public int CurBee = 20;
    public float Charging = 1f;
    public float BeeDelay = 300f;
    public float curDelay;

    [Header("Status")]
    public int Honey; //²Ü
    public int BeeWax; //¹Ð¶ø
    public bool[] SceneUnlock = new bool[3]; // 0: royal, 1: lab, 2: library

    [Header("Quest")]
    public int CurQuestIdx;
    public QuestData CurQuest; // == head
    public List<QuestData> QuestsList = new List<QuestData>();
    public QuestDataSave QuestSaveList = new QuestDataSave();
    public bool isQuestAble;

    void LoadQuest()
    {
        PlayerPrefs.DeleteKey(questSaveName);

        string questTmp = PlayerPrefs.GetString(questSaveName, "none");
        if (questTmp == "none")
        {
            QuestSaveList.QuestLists = QuestsList;

            string jsonSave = JsonUtility.ToJson(QuestSaveList, true);
            Debug.Log(jsonSave);
            PlayerPrefs.SetString(questSaveName, jsonSave);
        }
        else
        {
            QuestsList = QuestSaveList.QuestLists;
        }

        CurQuest = QuestsList[CurQuestIdx];
    }

    private void Awake()
    {
        //load data first
        DontDestroyOnLoad(this.gameObject);
        LoadData();
        LoadBeeTime();
        LoadQuest();
    }

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }

    private void Update()
    {
        isQuestAble = CurQuestIdx < QuestsList.Count;

        BeeCharging();
    }

    void BeeCharging()
    {
        if (CurBee < MaxBee)
        {
            curDelay += Time.deltaTime * Charging;

            if (curDelay >= BeeDelay)
            {
                CurBee++;
                curDelay = 0f;
            }
        }
        else curDelay = -0.01f;
    }
    #region Debug
    void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(dataSaveName);
        PlayerPrefs.DeleteKey(timeSaveName);
    }
    #endregion
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
    }

    void GetDataFromSave()
    {
        MaxBee = dataSave.MaxBee;
        CurBee = dataSave.CurBee;
        Honey = dataSave.Honey;
        BeeWax = dataSave.BeeWax;
        CurQuestIdx = dataSave.CurQuestIdx;
        SceneUnlock = dataSave.SceneUnlock;
    }

    void SetDataToSave()
    {
        dataSave.MaxBee = MaxBee;
        dataSave.CurBee = CurBee;
        dataSave.Honey = Honey;
        dataSave.BeeWax = BeeWax;
        dataSave.CurQuestIdx = CurQuestIdx;
        dataSave.SceneUnlock = SceneUnlock;
    }

    public void SaveData()
    {
        SetDataToSave();

        string saveStr = JsonUtility.ToJson(dataSave, true);
        PlayerPrefs.SetString(dataSaveName, saveStr);
    }

    void LoadBeeTime()
    {
        string timeStr = PlayerPrefs.GetString(timeSaveName, "");
        if (timeStr != "")
        {
            DateTime endTime = Convert.ToDateTime(timeStr);

            DateTime curTime = DateTime.Now;
            TimeSpan timeDif = curTime - endTime;
            float f_timeDif = (float)timeDif.TotalSeconds;

            int beeCount = (int)(f_timeDif / BeeDelay);
            curDelay = f_timeDif % BeeDelay;
            CurBee += beeCount;

            if (CurBee > MaxBee) CurBee = MaxBee;
        }
        else CurBee = MaxBee;
    }

    void SaveBeeTime()
    {
        DateTime endTime = DateTime.Now;
        PlayerPrefs.SetString(timeSaveName, endTime.ToString());
    }

    private void OnApplicationQuit()
    {
        SaveData();
        SaveBeeTime();
    }
}

[Serializable]
public class StatusSave
{
    public int MaxBee;
    public int CurBee;
    public int Honey;
    public int BeeWax;
    public int CurQuestIdx;
    public bool[] SceneUnlock = new bool[3];
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
    Wax
}

[Serializable]
public class QuestDataSave
{
    public List<QuestData> QuestLists = new List<QuestData>();
}

[Serializable]
public class QuestData
{
    public QuestNpcState thisState;
    public QuestValueKind thisKind;

    public bool isCleared = false;
    public bool QuestActive = false;

    public int defaultValue;
    public int curValue;
    public int maxValue;

    public string textId;


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
        action?.Invoke();
    }
}