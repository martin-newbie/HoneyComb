using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LabManager : MonoBehaviour
{
    [SerializeField] Image blackImg;

    public Text HoneyTxt;
    public Text WaxTxt;
    public Text RemainTimeTxt;
    public Text CurBeeQueueTxt;
    public int WaxCost = 100;

    void Start()
    {
        StartCoroutine(FadeIn(1f));
    }

    void Update()
    {
        SetText();
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


    void SetText()
    {
        int minute = (int)StatusManager.Instance.curWaxDelay / 60;
        float second = StatusManager.Instance.curWaxDelay % 60f;
        string text = minute.ToString() + ":" + ((int)second).ToString();

        RemainTimeTxt.text = text;
        CurBeeQueueTxt.text = StatusManager.Instance.QueueWax.ToString();

        HoneyTxt.text = Format(StatusManager.Instance.Honey);
        WaxTxt.text = Format(StatusManager.Instance.BeeWax);
    }

    string Format(object value)
    {
        string ret = string.Format("{0:#,0}", value);
        return ret;
    }

    public void Up()
    {
        if(StatusManager.Instance.Honey >= WaxCost)
        {
            StatusManager.Instance.Honey -= WaxCost;
            StatusManager.Instance.QueueWax++;
        }
    }

    public void Down()
    {
        if(StatusManager.Instance.QueueWax > 0)
        {
            StatusManager.Instance.Honey += WaxCost;
            StatusManager.Instance.QueueWax--;
        }
    }

    public void Back()
    {
        StartCoroutine(SceneMove("TitleScene"));
    }
}
