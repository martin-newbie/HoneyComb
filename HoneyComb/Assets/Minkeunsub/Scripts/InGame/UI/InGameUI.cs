using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : Singleton<InGameUI>
{
    public InputUI Input;
    public Image HpImg;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GameOverUIOn()
    {

    }

    public void SetPlayerHp(float fill)
    {
        HpImg.fillAmount = fill;
    }
}
