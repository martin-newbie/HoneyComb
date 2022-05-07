using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Text;

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
    [SerializeField] RectTransform canvasRt;

    [Header("Book")]
    public RectTransform BookImg;
    public RectTransform BookTarget;
    public RectTransform BookContainer;
    public Text BookName;
    [SerializeField] Sprite BookSprite;

    void Start()
    {
        Intro();
        pauseUI.gameObject.SetActive(false);
        BookImg.gameObject.SetActive(false);
        BookContainer.anchoredPosition = new Vector2(775f, 250f);
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
        DistanceTxt.text = "°Å¸®: " + string.Format("{0:#,0}", distance) + "M";
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

    public void GetBookEffect(Book bookData, Vector3 startPos)
    {
        BookName.text = "";
        string bookName = bookData.Init().BookName;

        Vector2 viewPos = Camera.main.WorldToViewportPoint(startPos);
        Vector2 worldPos = new Vector2(
            ((viewPos.x * canvasRt.sizeDelta.x) - (canvasRt.sizeDelta.x * 0.5f)),
            ((viewPos.y * canvasRt.sizeDelta.y) - (canvasRt.sizeDelta.y * 0.5f))
            );

        BookImg.gameObject.SetActive(true);
        BookImg.anchoredPosition = worldPos;

        BookContainer.DOAnchorPosX(0f, 0.5f).SetEase(Ease.OutBack);
        BookImg.DOAnchorPosY(worldPos.y + 400f, 1f).OnComplete(() =>
        {
            BookImg.DOSizeDelta(BookTarget.sizeDelta, 0.5f);
            BookImg.DOMoveY(BookTarget.transform.position.y, 0.5f).SetEase(Ease.InBack);
            BookImg.DOMoveX(BookTarget.transform.position.x, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                BookContainer.GetComponent<Image>().sprite = BookSprite;
                BookName.text = bookName;
                BookImg.gameObject.SetActive(false);
                BookContainer.DOAnchorPosX(775f, 0.5f).SetEase(Ease.InBack).SetDelay(2f);
            });
        });

    }
}
