using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RoyalSceneManager : Singleton<RoyalSceneManager>
{
    [SerializeField] Image blackImg;

    [Header("Status Texts")]
    [SerializeField] Text HoneyTxt;
    [SerializeField] Text BeewaxTxt;
    [SerializeField] Text RoomTxt;
    [SerializeField] Text BeeTxt;

    [Header("Upgrade Window")]
    [SerializeField] GameObject RoomUpgradeWindow;
    [SerializeField] GameObject RoomUpgradeBG;
    [SerializeField] GameObject BeeUpgradeWindow;
    [SerializeField] GameObject BeeUpgradeBG;
    [SerializeField] Text CurHoneyValue;
    [SerializeField] Text CurWaxValue;

    [Header("Value")]
    [SerializeField] int RoomCost = 250;
    [SerializeField] int BeeCost = 1000;

    [Header("Locked")]
    [SerializeField] GameObject BeeLocked;
    [SerializeField] GameObject RoomLocked;

    void Start()
    {
        StartCoroutine(FadeIn(1f));
    }

    void Update()
    {
        SetTexts();

        BeeLocked.SetActive(!StatusManager.Instance.beeUpgradeAble);
        RoomLocked.SetActive(!StatusManager.Instance.roomUpgradeAble);
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

    void SetTexts()
    {
        HoneyTxt.text = Format(StatusManager.Instance.Honey);
        BeewaxTxt.text = Format(StatusManager.Instance.BeeWax);
        RoomTxt.text = Format(StatusManager.Instance.Room);
        BeeTxt.text = Format(StatusManager.Instance.CurBee) + "/" + Format(StatusManager.Instance.MaxBee);
    }

    string Format(float value)
    {
        string ret = string.Format("{0:#,0}", value);
        return ret;
    }

    public void Back()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        StartCoroutine(SceneMove("TitleScene"));
    }

    public void RoomUpgrade()
    {
        if (StatusManager.Instance.roomUpgradeAble)
        {
            SoundManager.Instance.PlaySound("Button_Click");
            RoomUpgradeWindow.SetActive(true);
            RoomUpgradeBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -2960f);
            RoomUpgradeBG.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack);
            CurWaxValue.text = "현재: " + Format(StatusManager.Instance.BeeWax);
        }
        else
        {
            SoundManager.Instance.PlaySound("Button_Click_Fail");
            //you must clear the quest first
        }
    }

    public void RoomClose()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        RoomUpgradeBG.GetComponent<RectTransform>().DOAnchorPosY(-2960f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            RoomUpgradeWindow.SetActive(false);
        });
    }

    public void RoomUpgradeConfirm()
    {
        if (StatusManager.Instance.BeeWax >= RoomCost)
        {
            SoundManager.Instance.PlaySound("Button_Click");
            SoundManager.Instance.PlaySound("Build");
            StatusManager.Instance.BeeWax -= RoomCost;
            StatusManager.Instance.Room++;
            RoomClose();
        }
        else
        {
            SoundManager.Instance.PlaySound("Button_Click_Fail");
            //not enought wax remain
        }
    }

    public void BeeUpgrade()
    {
        if (StatusManager.Instance.beeUpgradeAble)
        {
            SoundManager.Instance.PlaySound("beebuzz");
            SoundManager.Instance.PlaySound("Button_Click");
            BeeUpgradeWindow.SetActive(true);
            BeeUpgradeBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -2960f);
            BeeUpgradeBG.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack);
            CurHoneyValue.text = "현재: " + Format(StatusManager.Instance.Honey);
        }
        else
        {
            SoundManager.Instance.PlaySound("Button_Click_Fail");
            //you must clear the quest first
        }
    }

    public void BeeClose()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        BeeUpgradeBG.GetComponent<RectTransform>().DOAnchorPosY(-2960f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            BeeUpgradeWindow.SetActive(false);
        });
    }

    public void BeeUpgradeConfirm()
    {
        if (StatusManager.Instance.Honey >= BeeCost)
        {
            if (StatusManager.Instance.MaxBee < StatusManager.Instance.Room)
            {
                SoundManager.Instance.PlaySound("Button_Click");
                StatusManager.Instance.Honey -= BeeCost;
                StatusManager.Instance.MaxBee++;
                BeeClose();
            }
            else
            {
                SoundManager.Instance.PlaySound("Button_Click_Fail");
                //not enought room remain
            }
        }
        else
        {
            SoundManager.Instance.PlaySound("Button_Click_Fail");
            //not enought honey remain
        }
    }
}
