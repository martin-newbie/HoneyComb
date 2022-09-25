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
                DescriptionString = $"<color=#FFFF33>{GetCharacterName(dailyQuests[i].characterType)}</color>ĳ���͸� ����Ͽ�<color=#0099FF> {GetStageName(dailyQuests[i].stageType)}</color>������������\n";
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
        questDescription[3].text = $"��� ��������Ʈ�� Ŭ�����ϸ� �߰� ������ �ֽ��ϴ�! ({questClearCount}/3)";
        questGuage.fillAmount = questClearCount / 3;
    }

    //�� �Լ� ��ư���� ��
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
                return "�ʿ�";
            case EStageType.FOREST:
                return "��";
            case EStageType.TUNDRA:
                return "����";
            case EStageType.DESSERT:
                return "�縷";
            default:
                break;
        }
        return null;
    }
}
