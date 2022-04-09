using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : Singleton<InGameUI>
{
    public Text HoneyTxt;
    public Text DistanceTxt;
    public InputUI Input;
    public Image HpImg;
    public GameOver gameOver;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetStatusTexts(int honey, float distance)
    {
        HoneyTxt.text = string.Format("{0:#,0}", honey);
        DistanceTxt.text = string.Format("{0:#,0}", distance) + "m";
    }

    public void GameOverUIOn(float distance, int honey)
    {
        gameOver.gameObject.SetActive(true);
        gameOver.Init(distance, honey);
    }

    public void SetPlayerHp(float fill)
    {
        HpImg.fillAmount = fill;
    }
}
