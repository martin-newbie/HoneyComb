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
                DescriptionString = $"<color=#FFFF33>{GetCharacterName(dailyQuests[i].characterType)}</color>캐릭터를 사용하여<color=#0099FF> {GetStageName(dailyQuests[i].stageType)}</color>스테이지에서\n";
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

    //이 함수 버튼에서 씀
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
