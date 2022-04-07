using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class TitleManager : Singleton<TitleManager>
{

    bool trigger;

    [Header("Status")]
    public bool isQuestAble;

    [Header("UI Objects")]
    [SerializeField] Image SpeechImg;
    [SerializeField] Text SpeechTxt;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SpeechMessage()
    {
        if (isQuestAble)
        {

        }
        else
        {
            SpeechRandomMessage();
        }
    }

    void SpeechRandomMessage()
    {
        string[] messages = new string[4] { "1234", "3455", "5677", "34444" };
        SpeechOn(messages);
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
        SpeechImg.rectTransform.DOSizeDelta(Vector2.zero, 0.5f).SetEase(Ease.InBack);
    }

    public void RoyalSceneMove()
    {

    }

    public void LabSceneMove()
    {

    }

    public void LibrarySceneMove()
    {

    }
}
