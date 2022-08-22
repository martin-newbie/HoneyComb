using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DailyQuestUI : MonoBehaviour
{
    public Text[] QuestTitle;
    public Text[] QuestDescription;
    private void Start()
    {
        List<BaseDailyQuest> dailyQuests = DailyQuest.Instance.dailyQuests;
        List<DailyQuestUI_Info> dailyQuestUI_Infos = Resources.LoadAll<DailyQuestUI_Info>("QuestInfo/").ToList();

        for (int i = 0; i < dailyQuests.Count; i++)
        {
            DailyQuestUI_Info questInfo = dailyQuestUI_Infos.Find((x) => x.questType == dailyQuests[i].type);
            QuestTitle[i].text = questInfo.questName;

            string DescriptionString = $"{GetCharacterName(dailyQuests[i].characterType)}캐릭터를 사용하여 {GetStageName(dailyQuests[i].stageType)}스테이지에서\n";
            DescriptionString += $"{dailyQuests[i]._index}/{dailyQuests[i].GetClearCount()}{questInfo.questDescription}";

            QuestDescription[i].text += DescriptionString;
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
                return "툰드라";
            case EStageType.DESSERT:
                return "사막";
            default:
                break;
        }
        return null;
    }
}
