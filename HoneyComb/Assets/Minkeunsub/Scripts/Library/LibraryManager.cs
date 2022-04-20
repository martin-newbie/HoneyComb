using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
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
        InitScroll(count);
    }

    void Update()
    {
        SetScrollRect();
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
