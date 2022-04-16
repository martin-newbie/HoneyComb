using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text curBee;

    private void Update()
    {
        curBee.text = StatusManager.Instance.CurBee.ToString();
    }

    public void Resume()
    {
        InGameUI.Instance.SetResumeTime();
        gameObject.SetActive(false);
    }

    public void Replay()
    {
        if(StatusManager.Instance.CurBee > 0)
        {
            SaveData();
            Time.timeScale = 1f;
            SceneManager.LoadScene("InGameScene");
            StatusManager.Instance.CurBee--;
        }
    }

    public void Title()
    {
        SaveData();
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    void SaveData()
    {
        StatusManager.Instance.Honey += InGameManager.Instance.roundHoney;
    }
}
