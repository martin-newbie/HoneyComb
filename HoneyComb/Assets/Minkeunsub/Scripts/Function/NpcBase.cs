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

    [Header("UI Objects")]
    [SerializeField] Image SpeechImg;
    [SerializeField] GameObject Exclamation;
    [SerializeField] Text SpeechTxt;

    private void Start()
    {
        TextAsset text = Resources.Load("Texts/" + path) as TextAsset;
        Messages = text.text.Split('\n').ToList();

        int temp = PlayerPrefs.GetInt("FirstMeet: " + path, 0);
        if (temp == 1) npcState = NpcState.None;
        else if (temp == 0) npcState = NpcState.FirstMeet;
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
                    break;
                case NpcState.QuestClear:
                    break;
            }
            ExclamationPrint();
        }
    }

    protected abstract void FirstMeetScript();

    protected void ExclamationPrint()
    {
        /*switch (npcState)
        {
            case NpcState.None:
                Exclamation.SetActive(false);
                break;
            case NpcState.QuestExists:
                Exclamation.SetActive(true);
                break;
            case NpcState.QuestClear:
                Exclamation.SetActive(true);
                break;
        }*/
    }

    protected abstract void QuestExistsMessage();
    protected abstract void QuestClearMessage();

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
