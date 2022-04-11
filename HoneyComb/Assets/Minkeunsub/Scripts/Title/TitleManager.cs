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


    void Start()
    {
        StartCoroutine(FadeIn(1f));
    }

    void Update()
    {
        SetTexts();
    }

    void SetTexts()
    {
        beewaxTxt.text = Format(StatusManager.Instance.BeeWax);
        honeyTxt.text = Format(StatusManager.Instance.Honey);
        beeCntTxt.text = Format(StatusManager.Instance.CurBee) + "/" + Format(StatusManager.Instance.MaxBee);
        beeChargeTxt.text = StatusManager.Instance.curBeeDelay >= 0f ? Format(StatusManager.Instance.curBeeDelay) + "/" + Format(StatusManager.Instance.BeeDelay) : "";
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
        if (StatusManager.Instance.CurBee > 0)
        {
            StatusManager.Instance.CurBee--;
            StartCoroutine(SceneMove("InGameScene"));
        }
    }

    public void RoyalSceneMove()
    {
        if (StatusManager.Instance.SceneUnlock[0])
            StartCoroutine(SceneMove("RoyalScene"));
    }

    public void LabSceneMove()
    {
        if (StatusManager.Instance.SceneUnlock[1])
            StartCoroutine(SceneMove("LabScene"));
    }

    public void LibrarySceneMove()
    {
        if (StatusManager.Instance.SceneUnlock[2])
            StartCoroutine(SceneMove("LibraryScene"));
    }
}
