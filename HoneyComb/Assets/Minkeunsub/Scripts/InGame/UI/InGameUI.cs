using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameUI : Singleton<InGameUI>
{
    public Text HoneyTxt;
    public Text DistanceTxt;
    public InputUI Input;
    public Image HpImg;
    public GameOver gameOver;
    public RectTransform Upper;
    public RectTransform PauseButton;
    public Pause pauseUI;
    public GameObject ResumeTimeContainer;
    public Text ResumeTimeTxt;

    void Start()
    {
        Intro();
        pauseUI.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    void Intro()
    {
        Upper.anchoredPosition = new Vector2(0, 450f);
        PauseButton.anchoredPosition = new Vector2(240f, -500f);

        Upper.DOAnchorPosY(0f, 0.5f);
        PauseButton.DOAnchorPosX(-70f, 0.5f);
    }

    public void PauseOn()
    {
        pauseUI.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SetResumeTime()
    {
        StartCoroutine(ResumeCoroutine());
    }

    IEnumerator ResumeCoroutine()
    {
        ResumeTimeContainer.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            ResumeTimeTxt.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        ResumeTimeTxt.text = "START";
        yield return new WaitForSecondsRealtime(1f);
        ResumeTimeContainer.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetStatusTexts(int honey, float distance)
    {
        HoneyTxt.text = string.Format("{0:#,0}", honey);
        DistanceTxt.text = "°Å¸®: "+string.Format("{0:#,0}", distance) + "M";
    }

    public void GameOverUIOn(float distance, int honey)
    {
        gameOver.gameObject.SetActive(true);
        gameOver.Init(distance, honey);
    }

    public void SetPlayerHp(float fill)
    {
        HpImg.fillAmount = fill;
    }
}
