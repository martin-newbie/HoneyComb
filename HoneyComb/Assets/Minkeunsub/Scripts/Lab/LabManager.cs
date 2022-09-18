using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LabManager : MonoBehaviour
{
    public Text HoneyTxt;
    public Text WaxTxt;
    public Text RemainTimeTxt;
    public Text CurBeeQueueTxt;
    public int WaxCost = 100;

    private void Start()
    {
        Camera.main.CameraSizeSet(1440, 2960);
    }

    void Update()
    {
        SetText();
    }

    void SetText()
    {
        int minute = (int)StatusManager.Instance.curWaxDelay / 60;
        float second = StatusManager.Instance.curWaxDelay % 60f;
        string text = minute.ToString() + ":" + ((int)second).ToString();

        RemainTimeTxt.text = text;
        CurBeeQueueTxt.text = StatusManager.Instance.QueueWax.ToString();

        HoneyTxt.text = Format(StatusManager.Instance.Honey);
        WaxTxt.text = Format(StatusManager.Instance.BeeWax);
    }

    string Format(object value)
    {
        string ret = string.Format("{0:#,0}", value);
        return ret;
    }

    public void Up()
    {
        if(StatusManager.Instance.Honey >= WaxCost)
        {
            StatusManager.Instance.Honey -= WaxCost;
            StatusManager.Instance.QueueWax++;
        }
    }

    public void Down()
    {
        if(StatusManager.Instance.QueueWax > 0)
        {
            StatusManager.Instance.Honey += WaxCost;
            StatusManager.Instance.QueueWax--;
        }
    }

    public void Back()
    {
        SceneLoadManager.Instance.LoadScene("TitleScene");
    }
}
