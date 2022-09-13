using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : Singleton<TitleManager>
{
    [Header("Status Texts")]
    [SerializeField] Text beewaxTxt;
    [SerializeField] Text honeyTxt;
    [SerializeField] Text beeCntTxt;
    [SerializeField] Text beeChargeTxt;

    [Header("UI Objects")]
    [SerializeField] Image[] SceneLock;
    [SerializeField] Image GameLock;
    public MapSelect mapSelect;

    [Header("Click Effect")]
    [SerializeField] GameObject clickEffectObj;

    void Update()
    {
        SetTexts();
        SetLockImage();
    }

    void SetLockImage()
    {
        for (int i = 0; i < SceneLock.Length; i++)
            SceneLock[i].gameObject.SetActive(!StatusManager.Instance.SceneUnlock[i]);

        GameLock.gameObject.SetActive(!StatusManager.Instance.gamePlayAble);
    }


    int cheatCount = 0;
    public void Cheat()
    {
        if (cheatCount < 5) cheatCount++;
        else if (StatusManager.Instance.isQuestAble)
        {
            switch (StatusManager.Instance.CurQuestIdx)
            {
                case 0:
                    StatusManager.Instance.Honey += 500;
                    break;
                case 1:
                    StatusManager.Instance.MaxBee += 5;
                    break;
                case 2:
                    StatusManager.Instance.BeeWax += 100;
                    break;
                case 3:
                    StatusManager.Instance.Room += 15;
                    break;
                case 4:
                    StatusManager.Instance.Honey += 5000;
                    break;
                default:
                    break;
            }
        }
    }

    public void OpenMapSelect()
    {
        mapSelect.gameObject.SetActive(true);
        mapSelect.UIon();
    }

    void SetTexts()
    {
        beewaxTxt.text = Format(StatusManager.Instance.BeeWax);
        honeyTxt.text = Format(StatusManager.Instance.Honey);
        beeCntTxt.text = Format(StatusManager.Instance.CurBee) + "/" + Format(StatusManager.Instance.MaxBee);

        string minute = ((int)StatusManager.Instance.curBeeDelay / 60).ToString();
        int i_second = (int)StatusManager.Instance.curBeeDelay % 60;

        string second = i_second < 10 ? "0" + i_second.ToString() : i_second.ToString();

        beeChargeTxt.text = StatusManager.Instance.curBeeDelay >= 0f ? minute + ":" + second + "/" + "5:00" : "";
    }

    string Format(float value)
    {
        string retStr = string.Format("{0:#,0}", value);
        return retStr;
    }

    public void GameStart()
    {
        if (StatusManager.Instance.gamePlayAble)
        {
            if (StatusManager.Instance.CurBee > 0)
            {

                SoundManager.Instance.PlaySound("Button_Click");
                StatusManager.Instance.CurBee--;
                SceneLoadManager.Instance.LoadScene("InGameScene");
            }
            else
            {
                SoundManager.Instance.PlaySound("Button_Click_Fail");
                //bee is not able now
            }
        }
        else
        {
            SoundManager.Instance.PlaySound("Button_Click_Fail");
            //you must clear the quest first
        }
    }

    public void RoyalSceneMove()
    {
        if (StatusManager.Instance.SceneUnlock[0])
        {
            SoundManager.Instance.PlaySound("Button_Click");
            SceneLoadManager.Instance.LoadScene("RoyalScene");
        }
        else
        {
            SoundManager.Instance.PlaySound("Button_Click_Fail");
            //you must clear the quest first
        }
    }

    public void LabSceneMove()
    {
        if (StatusManager.Instance.SceneUnlock[1])
        {
            SoundManager.Instance.PlaySound("Button_Click");
            SceneLoadManager.Instance.LoadScene("LabScene");
        }
        else
        {
            SoundManager.Instance.PlaySound("Button_Click_Fail");
            //you must clear the quest first
        }
    }

    public void LibrarySceneMove()
    {
        if (StatusManager.Instance.SceneUnlock[2])
        {
            SoundManager.Instance.PlaySound("Button_Click");
            SceneLoadManager.Instance.LoadScene("LibraryScene");
        }
        else
        {
            SoundManager.Instance.PlaySound("Button_Click_Fail");

            //you must clear the quest first
        }
    }
}
