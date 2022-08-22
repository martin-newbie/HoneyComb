using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyQuestInfo", menuName = "DailyQuestInfo", order = int.MinValue)]
public class DailyQuestUI_Info : ScriptableObject
{
    public QuestType questType;
    public string questName;
    public string questDescription;
}