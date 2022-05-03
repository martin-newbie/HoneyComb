using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
    public Image blackImg;

    [Header("Cartoon")]
    [SerializeField] GameObject CartoonChoose;
    [SerializeField] Sprite[] ButtonSprites;
    [SerializeField] RectTransform button;
    [SerializeField] RectTransform ContentContainer;
    [SerializeField] RectTransform rightVoid;
    List<RectTransform> buttonList = new List<RectTransform>();

    [Header("Book Choose")]
    [SerializeField] BookChoose bookChoose;

    [Header("Data")]
    [SerializeField] int count = 5;
    [SerializeField] int curIdx;
    [SerializeField] int maxIdx;

    Vector2 DefaultSize = new Vector2(700, 1000);
    Vector2 SelectSize = new Vector2(800, 1200);

    void Start()
    {
        StartCoroutine(FadeIn(1f));
        InitScroll(count);
    }

    void Update()
    {
        SetScrollRect();
        SetButtonSize();
    }

    public void BookOn()
    {
        bookChoose.gameObject.SetActive(true);
        bookChoose.UIon();
    }

    void SetButtonSize()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if(i == curIdx)
                buttonList[i].sizeDelta = Vector2.Lerp(buttonList[i].sizeDelta, SelectSize, Time.deltaTime * 15f);
            else
                buttonList[i].sizeDelta = Vector2.Lerp(buttonList[i].sizeDelta, DefaultSize, Time.deltaTime * 15f);
        }
    }

    public void Back()
    {
        StartCoroutine(SceneMove("TitleScene"));
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

    void SetScrollRect()
    {
        float posX = -(curIdx * 800f + 480f);
        ContentContainer.anchoredPosition = Vector2.Lerp(ContentContainer.anchoredPosition, new Vector2(posX, 0), Time.deltaTime * 15f);
    }

    void InitScroll(int n)
    {
        //button.GetComponent<Image>().sprite = ButtonSprites[0];
        button.GetComponent<Button>().onClick.AddListener(() => { OnClickEvent(0); });
        buttonList.Add(button);

        maxIdx = n;
        for (int i = 1; i < maxIdx - 1; i++)
        {
            RectTransform temp = Instantiate(button, ContentContainer.transform);
            temp.GetComponent<Image>().sprite = ButtonSprites[i];

            int idx = i + 1;
            temp.GetComponent<Button>().onClick.AddListener(() => { OnClickEvent(idx); });
            buttonList.Add(temp);
        }

        rightVoid.SetAsLastSibling();
    }

    void OnClickEvent(int idx)
    {
        CartoonManager.Instance.CartoonStartFunction(idx, null);
    }

    public void SetScrollIdx(int idx)
    {
        curIdx += idx;
        if (curIdx == maxIdx - 1) curIdx = 0;
        else if (curIdx < 0) curIdx = maxIdx - 1;
    }

    public void OpenCartoon()
    {
        CartoonChoose.SetActive(true);
    }

    public void CloseCartoon()
    {
        CartoonChoose.SetActive(false);
    }
}
