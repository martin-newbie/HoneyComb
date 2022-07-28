using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : Singleton<TitleManager>
{
    public Image blackImg;

    [Header("Status Texts")]
    [SerializeField] Text beewaxTxt;
    [SerializeField] Text honeyTxt;
    [SerializeField] Text beeCntTxt;
    [SerializeField] Text beeChargeTxt;

    [Header("UI Objects")]
    [SerializeField] Image[] SceneLock;
    [SerializeField] Image GameLock;

    [Header("Click Effect")]
    [SerializeField] GameObject clickEffectObj;

    void Start()
    {
        StartCoroutine(FadeIn(1f));
    }

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

    IEnumerator SceneMove(string sceneName)
    {
        yield return StartCoroutine(FadeOut(1f));
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeIn(float delay)
    {
        float timer = delay;
        blackImg.gameObject.SetActive(true);
        while (timer > 0)
        {
            blackImg.color = new Color(0, 0, 0, timer / delay);
            timer -= Time.deltaTime;
            yield return null;
        }

        blackImg.gameObject.SetActive(false);
    }

    IEnumerator FadeOut(float delay)
    {
        float timer = 0f;
        blackImg.gameObject.SetActive(true);
        while (timer < delay)
        {
            blackImg.color = new Color(0, 0, 0, timer / delay);
            timer += Time.deltaTime;
            yield return null;
        }

    }

    public void GameStart()
    {
        if (StatusManager.Instance.gamePlayAble)
        {
            if (StatusManager.Instance.CurBee > 0)
            {

                SoundManager.Instance.PlaySound("Button_Click");
                StatusManager.Instance.CurBee--;
                StartCoroutine(SceneMove("InGameScene"));
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
            StartCoroutine(SceneMove("RoyalScene"));
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
            StartCoroutine(SceneMove("LabScene"));
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
            StartCoroutine(SceneMove("LibraryScene"));
        }
        else
        {
            SoundManager.Instance.PlaySound("Button_Click_Fail");

            //you must clear the quest first
        }
    }
}
