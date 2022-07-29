using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    public EPlayableCharacter nowCharacterType;


    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button seleteButton;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private SoundManager soundManager;
    private StatusManager statusManager;
    private CharacterScript[] characterScripts;
    private void Start()
    {
        statusManager = StatusManager.Instance;
        soundManager = SoundManager.Instance;

        seleteButton.onClick.AddListener(() => ButtonClickFunc());
    }
    void ButtonClickFunc()
    {
        if (statusManager.playableCharacterInfos.Find((x) => x.character == nowCharacterType).IsHave)
        {
            soundManager.PlaySound("Button_Click2");
            statusManager.nowCharacter = nowCharacterType;
        }
        else
        {
            soundManager.PlaySound("Button_Click_Fail");
        }
    }
}
