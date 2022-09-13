using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameOver : MonoBehaviour
{

    [Header("Colors")]
    [SerializeField] Color[] TextColors = new Color[16];

    [Header("State")]
    public int maxWait = 15;
    [SerializeField] int state = 0; //0: revive, 2: result ( 0 -> 1 -> 2 )
    int waitTime;

    [Header("Revive Objects")]
    [SerializeField] GameObject reviveObj;
    [SerializeField] Text reviveTime;
    [SerializeField] Image reviveTimeImg;

    [Header("Result Objects")]
    [SerializeField] GameObject resultObj;
    [SerializeField] Text DistanceTxt;
    [SerializeField] Text HoneyTxt;
    [SerializeField] Button ReplayBtn;
    [SerializeField] Button TitleBtn;
    [SerializeField] Text CurBee;

    float distance;
    int honey;
    bool already;

    Coroutine activeCoroutine;

    public void Init(float _distance, int _honey)
    {
        waitTime = maxWait;
        reviveObj.SetActive(true);
        resultObj.SetActive(false);

        ReplayBtn.gameObject.SetActive(false);
        TitleBtn.gameObject.SetActive(false);

        distance = _distance;
        honey = _honey;
    }
    void OnMergeDailyQuest()
    {
        DailyQuest.Instance.distance += (int)distance;
        DailyQuest.Instance.getHoneyCount += honey;
        DailyQuest.Instance.playCount++;
    }

    private void Update()
    {
        switch (state)
        {
            case 0:
                if (!already)
                    StateRevive();
                else
                {
                    StartCoroutine(ReviveToReulst(0.5f));
                    state = 1;
                }
                break;
            case 1:
                StateStandby();
                break;
            case 2:
                StateResult();
                break;
        }
        SetText();
    }

    void SetText()
    {
        CurBee.text = string.Format("{0:#,0}", StatusManager.Instance.CurBee);
    }

    public void ResultReplay()
    {
        if (StatusManager.Instance.CurBee > 0)
        {
            SoundManager.Instance.PlaySound("Button_Click");
            StatusManager.Instance.CurBee--;

            SceneLoadManager.Instance.LoadScene("InGameScene");
            InGameManager.Instance.SaveDataToManager();
        }
        else
            SoundManager.Instance.PlaySound("Button_Click_Fail");
    }

    public void ResultTitle()
    {
        OnMergeDailyQuest();
        SoundManager.Instance.PlaySound("Button_Click");

        SceneLoadManager.Instance.LoadScene("TitleScene");
        InGameManager.Instance.SaveDataToManager();
    }

    void StateRevive()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        activeCoroutine = StartCoroutine(ReviveCoroutine());
        state = 1;
    }

    IEnumerator ReviveCoroutine()
    {

        waitTime = maxWait;
        reviveTime.text = waitTime.ToString();
        reviveTime.color = TextColors[15 - waitTime];
        while (waitTime > 0)
        {
            int dir = waitTime % 2 == 0 ? 1 : -1;
            reviveTime.transform.DORotate(new Vector3(0, 0, 30f * dir), 0.3f);
            reviveTime.transform.DOScale(2f, 0.3f).OnComplete(() =>
            {
                reviveTime.transform.DOScale(1f, 0.05f);
                reviveTime.transform.DORotate(Vector3.zero, 0.05f).OnComplete(() =>
                {
                    waitTime--;
                    reviveTime.text = waitTime.ToString();
                    reviveTime.color = TextColors[15 - waitTime];
                });
            });

            yield return new WaitForSecondsRealtime(1f);
        }

        yield return null;
        StartCoroutine(ReviveToReulst(0.5f));
    }

    IEnumerator ReviveToReulst(float delay)
    {
        resultObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -2960f);
        resultObj.SetActive(true);

        reviveObj.GetComponent<RectTransform>().DOAnchorPosY(-2960f, delay).SetEase(Ease.InBack).OnComplete(() =>
        {
            resultObj.GetComponent<RectTransform>().DOAnchorPosY(0, delay).SetEase(Ease.OutBack);
            reviveObj.SetActive(false);
        });
        yield return new WaitForSeconds(delay * 2);
        state = 2;
    }

    IEnumerator ResultEffect()
    {
        state = 1;
        yield return StartCoroutine(TextCounting(DistanceTxt, distance, 2f, "m"));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(TextCounting(HoneyTxt, honey, 2f));
        yield return new WaitForSeconds(0.5f);

        TitleBtn.gameObject.SetActive(true);
        ReplayBtn.gameObject.SetActive(true);

        TitleBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(1200f, -1200f);
        ReplayBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1200f, -1200f);

        TitleBtn.GetComponent<RectTransform>().DOAnchorPosX(380f, 0.5f);
        ReplayBtn.GetComponent<RectTransform>().DOAnchorPosX(-380f, 0.5f);
    }

    IEnumerator TextCounting(Text text, float max, float timer, string unit = "")
    {
        float duration = timer;
        float cur = 0f;
        float offset = (max - cur) / duration;

        while (cur < max)
        {
            cur += offset * timer * Time.deltaTime;
            text.text = ((int)cur).ToString() + unit;
            yield return null;
        }
        cur = max;
        text.text = ((int)cur).ToString() + unit;
    }

    void StateStandby()
    {

    }

    void StateResult()
    {
        StartCoroutine(ResultEffect());
    }

    public void ReviveSkipButton()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        waitTime = 0;
    }

    public void ReviveAdWatch()
    {
        //play AD
        Revive();
    }

    public void ReviveUseHoney()
    {
        if (StatusManager.Instance.Honey >= 700)
        {
            SoundManager.Instance.PlaySound("Button_Click");
            StatusManager.Instance.Honey -= 700;
            Revive();
        }
        else
            SoundManager.Instance.PlaySound("Button_Click_Fail");
    }

    void Revive()
    {
        already = true;
        state = 0;
        waitTime = maxWait;
        InGameManager.Instance.Revive();
        StopCoroutine(activeCoroutine);
        gameObject.SetActive(false);
    }
}
