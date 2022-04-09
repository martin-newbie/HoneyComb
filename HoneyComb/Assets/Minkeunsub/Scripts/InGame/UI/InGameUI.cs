using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : Singleton<InGameUI>
{
    public InputUI Input;
    public Image HpImg;
    public GameOver gameOver;

    void Start()
    {
        
    }

    void Update()
    {
        
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
