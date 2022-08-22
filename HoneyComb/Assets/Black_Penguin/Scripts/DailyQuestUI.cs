using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DailyQuestUI : MonoBehaviour
{
    public bool isUIOn;

    public Text[] QuestTitle;
    public Text[] QuestDescription;
    private void Start()
    {
        List<BaseDailyQuest> dailyQuests = DailyQuest.Instance.dailyQuests.quests;
        List<DailyQuestUI_Info> dailyQuestUI_Infos = Resources.LoadAll<DailyQuestUI_Info>("QuestInfo/").ToList();
        for (int i = 0; i < dailyQuests.Count; i++)
        {
            DailyQuestUI_Info questInfo = dailyQuestUI_Infos.Find((x) => x.questType == dailyQuests[i].type);
            QuestTitle[i].text = questInfo.questName;

            string DescriptionString = "";
            if (dailyQuests[i].type < QuestType.DailyQuestCollectWax)
                DescriptionString = $"<color=#FFFF33>{GetCharacterName(dailyQuests[i].characterType)}</color>ĳ���͸� ����Ͽ�<color=#0099FF> {GetStageName(dailyQuests[i].stageType)}</color>������������\n";
            DescriptionString += $"<color=#FF9999>{dailyQuests[i]._index}/{dailyQuests[i].GetClearCount()}</color>{questInfo.questDescription}";

            QuestDescription[i].text = DescriptionString;
        }
    }
    private void Update()
    {
        if (isUIOn)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * 5);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1420, 0, 0), Time.deltaTime * 5);
        }
    }

    //�� �Լ� ��ư���� ��
    public void UIOnOff(bool isOn)
    {
        isUIOn = isOn;
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
                return "�ʿ�";
            case EStageType.FOREST:
                return "��";
            case EStageType.TUNDRA:
                return "�����";
            case EStageType.DESSERT:
                return "�縷";
            default:
                break;
        }
        return null;
    }
}
