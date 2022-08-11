using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDailyQuest
{
    public string questName;
    public int index;
    public abstract bool isClear();
}
public class GoFast : BaseDailyQuest
{

    public override bool isClear()
    {

        return true;
    }
}
public class DailyQuest : MonoBehaviour
{

}
