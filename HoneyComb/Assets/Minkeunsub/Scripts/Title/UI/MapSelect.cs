using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MapSelect : MonoBehaviour
{
    public int curIdx;
    
    [Header("UI")]
    public Button[] mapSelect;
    public RectTransform content;
    public RectTransform background;
    public Text TitleTxt;
    public Text DescTxt;
    
    [Header("Texts")]
    public string[] names = new string[4];
    public string[] descs = new string[4];

    void Start()
    {
        for (int i = 0; i < mapSelect.Length; i++)
        {
            int idx = i;
            mapSelect[i].onClick.AddListener(() => MapSelectButton(idx));
        }

        SetFlavorTexts();
    }

    void SetFlavorTexts()
    {
        TitleTxt.text = names[curIdx];

        string temp = descs[curIdx].Replace("\\n", "\n");
        DescTxt.text = temp;
    }

    public void UIon()
    {
        background.anchoredPosition = new Vector2(1594, 0f);
        background.DOAnchorPosX(0f, 0.5f).SetEase(Ease.OutBack);
    }

    public void UIoff()
    {
        background.DOAnchorPosX(1594f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    void Update()
    {
        for (int i = 0; i < mapSelect.Length; i++)
        {
            if (i == curIdx)
            {
                mapSelect[i].image.rectTransform.sizeDelta = Vector2.Lerp(mapSelect[i].image.rectTransform.sizeDelta, new Vector2(470f, 850f), Time.deltaTime * 15f);
            }
            else
            {
                mapSelect[i].image.rectTransform.sizeDelta = Vector2.Lerp(mapSelect[i].image.rectTransform.sizeDelta, new Vector2(370f, 770f), Time.deltaTime * 15f);
            }
        }

        content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, new Vector2(-(310f + (470f * curIdx)), 0), Time.deltaTime * 15f);
    }

    public void RightButton()
    {
        if (curIdx < mapSelect.Length - 1) curIdx++;
        else curIdx = 0;

        SetFlavorTexts();
    }

    public void LeftButton()
    {
        if (curIdx > 0) curIdx--;
        else curIdx = mapSelect.Length - 1;

        SetFlavorTexts();
    }

    public void MapSelectButton(int idx)
    {

        PlayerPrefs.SetInt("MapIdx", idx);
        StatusManager.Instance.nowStage = (EStageType)idx;
        UIoff();
    }
}
