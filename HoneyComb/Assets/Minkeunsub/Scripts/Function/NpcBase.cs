using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public enum NpcState
{
    FirstMeet,
    None,
    QuestExists,
    QuestClear
}

public abstract class NpcBase : MonoBehaviour
{

    bool trigger;
    bool isSpeeching;

    [Header("Status")]
    public List<string> Messages = new List<string>();
    public bool QuestAble;
    public NpcState npcState;
    public string path;
    public QuestData thisQuest;
    public QuestNpcState thisQuestState;

    [Header("UI Objects")]
    [SerializeField] Image SpeechImg;
    [SerializeField] GameObject Exclamation;
    [SerializeField] Text SpeechTxt;

    [Header("Quest UI Objects")]
    [SerializeField] Image questBG;
    [SerializeField] Text questName;
    [SerializeField] Text questProgress;

    private void Start()
    {
        TextAsset text = Resources.Load("Texts/" + path) as TextAsset;
        Messages = text.text.Split('\n').ToList();

        int temp = PlayerPrefs.GetInt("FirstMeet: " + path, 0);
        //debug
        //temp = 0;

        if (temp == 1) npcState = NpcState.None;
        else if (temp == 0) npcState = NpcState.FirstMeet;

        if (StatusManager.Instance.isQuestAble)
        {
            QuestData questTmp = StatusManager.Instance.CurQuest;
            if (questTmp.thisState == thisQuestState)
            {
                thisQuest = questTmp;
                if (thisQuest.QuestActive)
                {
                    thisQuest.SetValue();
                    QuestUIOn();
                }
            }
            else thisQuest = null;
        }
    }

    void QuestUIOn()
    {
        questName.text = thisQuest.SetQuestName(StatusManager.Instance.CurQuestIdx);
        questProgress.text = "현재 진행도: " + thisQuest.curValue.ToString() + "/" + thisQuest.maxValue.ToString();
        questBG.rectTransform.DOAnchorPosX(330f, 0.5f).SetEase(Ease.OutBack);
    }

    void QuestUIOff()
    {
        questBG.rectTransform.DOAnchorPosX(1150f, 0.5f).SetEase(Ease.InBack);
    }

    public void SpeechMessage()
    {
        if (!isSpeeching)
        {
            switch (npcState)
            {
                case NpcState.FirstMeet:
                    FirstMeetScript();

                    npcState = NpcState.None;
                    PlayerPrefs.SetInt("FirstMeet: " + path, 1);

                    break;
                case NpcState.None:
                    isSpeeching = true;
                    SpeechRandomMessage();
                    break;
                case NpcState.QuestExists:
                    QuestExistsMessage();
                    break;
                case NpcState.QuestClear:
                    QuestClearMessage();
                    break;
            }
        }
    }

    void LateUpdate()
    {
        ExclamationPrint();
        CheckQuestExists();

        thisQuest?.CheckIsClear();
    }

    void CheckQuestExists()
    {
        if (thisQuest != null && npcState == NpcState.None)
        {
            if (thisQuest.isCleared && thisQuest.QuestActive)
            {
                npcState = NpcState.QuestClear;
            }
            else if (!thisQuest.isCleared && !thisQuest.QuestActive)
            {
                npcState = NpcState.QuestExists;
            }
            StatusManager.Instance.QuestsList[StatusManager.Instance.CurQuestIdx] = thisQuest;
        }
    }

    protected abstract void FirstMeetScript();

    protected void ExclamationPrint()
    {
        Exclamation.SetActive(npcState != NpcState.None);
    }

    protected virtual void QuestExistsMessage()
    {
        thisQuest.SetDefaultValue();
        string[] scripts = thisQuest.GetQuestTextScript();
        SpeechOn(scripts);
        npcState = NpcState.None;
        QuestUIOn();
        GetQuestAction();
    }
    protected virtual void QuestClearMessage()
    {
        thisQuest.isCleared = true;
        string[] scripts = thisQuest.GetQuestClearScript();
        SpeechOn(scripts);
        thisQuest.GetReward(GetRewardAction);
        npcState = NpcState.None;
        QuestUIOff();
        thisQuest = null;
        StatusManager.Instance.CurQuestIdx++;
    }

    protected abstract void GetRewardAction();
    protected abstract void GetQuestAction();

    void SpeechRandomMessage()
    {
        SpeechOn(Messages[Random.Range(0, Messages.Count)]);
    }

    public void SpeechOn(params string[] messages)
    {
        SpeechImg.rectTransform.sizeDelta = Vector2.zero;
        SpeechImg.rectTransform.DOSizeDelta(new Vector2(1000f, 500f), 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            StartCoroutine(DoArrayText(SpeechTxt, 0.05f, messages.ToList()));
        });
    }

    public void NextSpeech()
    {
        trigger = true;
    }

    IEnumerator DoArrayText(Text txt, float delay, List<string> messages)
    {
        foreach (var message in messages)
        {
            yield return StartCoroutine(DoTextConsistentSpeed(txt, message, delay));
            trigger = false;
            while (!trigger) yield return null;
        }

        SpeechOff();
    }

    IEnumerator DoTextConsistentSpeed(Text txt, string message, float delay)
    {
        txt.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            txt.text += message[i];
            yield return new WaitForSeconds(delay);
        }
    }

    public void SpeechOff()
    {
        SpeechTxt.text = "";
        SpeechImg.rectTransform.DOSizeDelta(Vector2.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            isSpeeching = false;
        });
    }
}
