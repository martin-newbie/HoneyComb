using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyQuest : MonoBehaviour
{
    public abstract class BaseQuest
    {
        public string questName;
        public int index;
        public virtual bool isClear()
        {
            bool index = default;
            return index;
        }
    }
}
