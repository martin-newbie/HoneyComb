using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatusManager : Singleton<StatusManager>
{
    public StatusSave dataSave = new StatusSave();
    public string dataSaveName = "status data save";
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

    private void Awake()
    {
        //load data first
        DontDestroyOnLoad(this.gameObject);
        LoadData();
        LoadBeeTime();
    }

    private void Update()
    {
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
    }

    void SetDataToSave()
    {
        dataSave.MaxBee = MaxBee;
        dataSave.CurBee = CurBee;
        dataSave.Honey = Honey;
        dataSave.BeeWax = BeeWax;
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
}