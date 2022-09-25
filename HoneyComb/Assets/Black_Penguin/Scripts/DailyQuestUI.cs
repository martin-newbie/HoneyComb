using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DailyQuestUI : MonoBehaviour
{
    public bool isUIOn;

    public Text[] questTitle;
    public Text[] questDescription;
    public Image[] questClearShow;

    public Image questGuage;
    public int questClearCount;
    private void Start()
    {
        List<BaseDailyQuest> dailyQuests = DailyQuest.Instance.dailyQuests.quests;
        List<DailyQuestUI_Info> dailyQuestUI_Infos = Resources.LoadAll<DailyQuestUI_Info>("QuestInfo/").ToList();
        for (int i = 0; i < dailyQuests.Count; i++)
        {
            DailyQuestUI_Info questInfo = dailyQuestUI_Infos.Find((x) => x.questType == dailyQuests[i].type);
            questTitle[i].text = questInfo.questName;

            string DescriptionString = "";
            if (dailyQuests[i].type < QuestType.DailyQuestCollectWax)
                DescriptionString = $"<color=#FFFF33>{GetCharacterName(dailyQuests[i].characterType)}</color>캐릭터를 사용하여<color=#0099FF> {GetStageName(dailyQuests[i].stageType)}</color>스테이지에서\n";
            DescriptionString += $"<color=#FF9999>{dailyQuests[i]._index}/{dailyQuests[i].GetClearCount()}</color>{questInfo.questDescription}";

            questDescription[i].text = DescriptionString;

            if (dailyQuests[i].isCompleted)
            {
                questClearCount++;
            }
        }



        QuestAllCompleteUIShow();
    }
    private void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            questClearShow[i].gameObject.SetActive(DailyQuest.Instance.dailyQuests.quests[i].isCompleted);
        }
        if (questClearCount >= 3)
        {
            questClearShow[3].gameObject.SetActive(true);
        }

        if (isUIOn)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) isUIOn = false;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * 15);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1420, 0, 0), Time.deltaTime * 15);
        }
    }
    void QuestAllCompleteUIShow()
    {
        questDescription[3].text = $"모든 일일퀘스트를 클리어하면 추가 보상이 있습니다! ({questClearCount}/3)";
        questGuage.fillAmount = questClearCount / 3;
    }

    //이 함수 버튼에서 씀
    public void UIOnOff(bool isOn)
    {
        isUIOn = isOn;
    }
    public void QuestClearCheck(int index)
    {
        BaseDailyQuest quest = DailyQuest.Instance.dailyQuests.quests[index];
        if (quest.isClear())
        {
            SoundManager.Instance.PlaySound("Button_Click");

            quest.isCompleted = true;
            questClearCount++;

            QuestAllCompleteUIShow();

            StatusManager.Instance.Honey += 500;
        }
        else
        {
            SoundManager.Instance.PlaySound("Button_Click_Fail");
        }
    }
    public string GetCharacterName(EPlayableCharacter character)
    {
        return Resources.LoadAll<CharacterScript>("Characters/").ToList().Find((x) => x.characterType == character).characterName;
    }
    public string GetStageName(EStageType stageType)
    {
        switch (stageType)
        {
            case EStageType.PLAIN:
                return "초원";
            case EStageType.FOREST:
                return "숲";
            case EStageType.TUNDRA:
                return "설원";
            case EStageType.DESSERT:
                return "사막";
            default:
                break;
        }
        return null;
    }
}
