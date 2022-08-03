using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterUI : MonoBehaviour
{
    public EPlayableCharacter nowShowCharacterType;

    [SerializeField] private GameObject beeList;
    [SerializeField] private Button openButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;

    private int beeListIndexNumber;
    private bool isOpen;
    private bool _isOpen
    {
        set
        {
            if (value == isOpen) return;
            transform.DOKill();
            switch (value)
            {
                case true:
                    transform.DOLocalMoveX(0, 1).SetEase(Ease.InOutBack);
                    Debug.Log(Time.timeScale);
                    break;
                case false:
                    transform.DOLocalMoveX(1500, 1).SetEase(Ease.InOutBack);
                    break;
            }
            isOpen = value;
        }
    }

    private SoundManager soundManager;
    private StatusManager statusManager;
    private List<CharacterScript> characterScripts;

    private void Start()
    {
        statusManager = StatusManager.Instance;
        soundManager = SoundManager.Instance;

        characterScripts = Resources.LoadAll<CharacterScript>("Characters/").ToList();
        openButton.onClick.AddListener(() => _isOpen = true);
        selectButton.onClick.AddListener(() => SelectButtonClickFunc());
        leftButton.onClick.AddListener(() => ArrowButtonClickFunc(-1));
        rightButton.onClick.AddListener(() => ArrowButtonClickFunc(1));

        nowShowCharacterType = statusManager.nowCharacter;
        beeListIndexNumber = (int)nowShowCharacterType;
        PannelReload();
    }
    private void Update()
    {
        beeList.transform.localPosition = Vector3.Lerp(beeList.transform.localPosition, new Vector3(beeListIndexNumber * -600, 0), Time.deltaTime * 3);

        openButton.image.sprite = characterScripts[(int)statusManager.nowCharacter].Icon;
    }

    private void OnDestroy()
    {
        openButton.onClick.RemoveAllListeners();
        selectButton.onClick.RemoveAllListeners();
        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
    }
    void ArrowButtonClickFunc(int index)
    {
        switch (index)
        {
            case -1:
                beeListIndexNumber--;
                break;
            case 1:
                beeListIndexNumber++;
                break;
            default:
                Debug.Assert(false, "Error: beeListIndexNumber is Wrong");
                break;
        }
        if (beeListIndexNumber <= -1)
        {
            beeListIndexNumber = characterScripts.Count - 1;
        }
        else if (beeListIndexNumber >= characterScripts.Count)
        {
            beeListIndexNumber = 0;
        }
        nowShowCharacterType = characterScripts[beeListIndexNumber].characterType;
        PannelReload();
    }
    void PannelReload()
    {
        PlayableCharacterInfo playableCharacterInfo = statusManager.playableCharacterInfos.Find((x) => x.character == nowShowCharacterType);

        nameText.text = $"Lv.{playableCharacterInfo.level} {characterScripts[beeListIndexNumber].characterName}";
        descriptionText.text = characterScripts[beeListIndexNumber].Description;
    }

    void SelectButtonClickFunc()
    {
        if (statusManager.playableCharacterInfos.Find((x) => x.character == nowShowCharacterType).IsHave)
        {
            soundManager.PlaySound("Button_Click2");
            _isOpen = false;
            statusManager.nowCharacter = nowShowCharacterType;
        }
        else
        {
            soundManager.PlaySound("Button_Click_Fail");
        }
    }
}
