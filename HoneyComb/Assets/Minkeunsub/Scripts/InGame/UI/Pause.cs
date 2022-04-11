using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public void Resume()
    {
        InGameUI.Instance.SetResumeTime();
        gameObject.SetActive(false);
    }

    public void Replay()
    {
        SaveData();
        Time.timeScale = 1f;
        SceneManager.LoadScene("InGameScene");
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
