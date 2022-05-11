using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameQuest : MonoBehaviour
{

    RectTransform rt;
    bool questAble;
    public QuestData thisQuest;

    [Header("Quests")]
    [SerializeField] RectTransform questObj;
    [SerializeField] Text TitleTxt;
    [SerializeField] Text ProgressTxt;

    void Start()
    {
        rt = GetComponent<RectTransform>();

        if (StatusManager.Instance.CurQuest != null)
        {
            questAble = true;
            InitQuest(StatusManager.Instance.CurQuest);
            UIInit();
        }
        else questAble = false;
    }

    void Update()
    {
        if (questAble)
            SetQuestValue();
    }

    void InitQuest(QuestData data)
    {
        thisQuest = data;
        TitleTxt.text = thisQuest.SetQuestName(StatusManager.Instance.CurQuestIdx);
    }

    void SetQuestValue()
    {
        ProgressTxt.text = "현재 진행도: " + thisQuest.curValue.ToString() + "/" + thisQuest.maxValue.ToString(); ;
    }

    void UIInit()
    {
        rt.anchoredPosition = new Vector2(240f, -770f);
        rt.DOAnchorPosX(-70f, 0.5f).SetDelay(0.3f);
    }

    public void UIon()
    {
        if (questAble)
            questObj.DOAnchorPosX(-250f, 0.5f).SetEase(Ease.OutBack);
    }

    public void UIoff()
    {
        questObj.DOAnchorPosX(600f, 0.5f).SetEase(Ease.InBack);
    }
}
