using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
    public Image blackImg;

    [SerializeField] Sprite[] ButtonSprites;
    [SerializeField] RectTransform button;
    [SerializeField] RectTransform ContentContainer;
    [SerializeField] ScrollRect Scroll;
    List<RectTransform> buttonList = new List<RectTransform>();

    [Header("Data")]
    [SerializeField] int count = 5;
    [SerializeField] float offset;
    [SerializeField] float curSet;
    [SerializeField] int curIdx;
    [SerializeField] int maxIdx;

    void Start()
    {
        StartCoroutine(FadeIn(1f));
        InitScroll(count);
    }

    void Update()
    {
        SetScrollRect();
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
        Scroll.verticalScrollbar.value = Mathf.Lerp(Scroll.verticalScrollbar.value, curSet, Time.deltaTime * 15f);
    }

    void InitScroll(int n)
    {
        button.GetComponent<Image>().sprite = ButtonSprites[0];
        button.GetComponent<Button>().onClick.AddListener(() => { OnClickEvent(0); });
        buttonList.Add(button);

        maxIdx = n;
        for (int i = 1; i < maxIdx; i++)
        {
            RectTransform temp = Instantiate(button, ContentContainer.transform);
            temp.GetComponent<Image>().sprite = ButtonSprites[i];

            int idx = i;
            temp.GetComponent<Button>().onClick.AddListener(() => { OnClickEvent(idx); });
            buttonList.Add(temp);
        }


        offset = 1f / (n - 1);
    }

    void OnClickEvent(int idx)
    {
        CartoonManager.Instance.CartoonStartFunction(idx, null);
    }

    public void SetScrollIdx(int idx)
    {
        curIdx += idx;
        if (curIdx == maxIdx) curIdx = 0;
        else if (curIdx < 0) curIdx = maxIdx - 1;

        curSet = 1f - offset * curIdx;
    }
}
