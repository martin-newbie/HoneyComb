using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public EPlayableCharacter nowCharacterType;


    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Text nameText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text descriptionText;

    private SoundManager soundManager;
    private StatusManager statusManager;
    private CharacterScript[] characterScripts;
    private void Start()
    {
        statusManager = StatusManager.Instance;
        soundManager = SoundManager.Instance;

        selectButton.onClick.AddListener(() => ButtonClickFunc());
    }

    private void OnDestroy()
    {
        selectButton.onClick.RemoveAllListeners();
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
