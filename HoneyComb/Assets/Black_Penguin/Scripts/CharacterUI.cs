using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterUI : MonoBehaviour
{
    public EPlayableCharacter nowShowCharacterType;

    [SerializeField] Image backBlur;
    [SerializeField] private GameObject beeList;
    [SerializeField] private Button openButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Image characterIcon;

    private int beeListIndexNumber;
    private bool isOpen;
    public bool _isOpen
    {
        set
        {
            if (value == isOpen) return;
            transform.DOKill();
            switch (value)
            {
                case true:
                    backBlur.enabled = true;
                    transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutBack);
                    break;
                case false:
                    transform.DOLocalMoveX(1500, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        backBlur.enabled = false;
                    });
                    break;
            }
            isOpen = value;
        }
    }

    private SoundManager soundManager;
    private StatusManager statusManager;
    [SerializeField]
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

        foreach (CharacterScript script in characterScripts)
        {
            if (script.characterType == EPlayableCharacter.HONEY_BEE) continue;

            Image image = Instantiate(characterIcon, characterIcon.transform.parent);

            image.name = $"{script.name}Icon";
            image.sprite = script.Icon;

            if (statusManager.playableCharacterInfos.Find((x) => x.character == script.characterType).level <= 0)
            {
                image.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0.7f);
            }
        }

        nowShowCharacterType = statusManager.nowCharacter;
        beeListIndexNumber = (int)nowShowCharacterType;
        PannelReload();
    }

    private void Update()
    {
        beeList.transform.localPosition = Vector3.Lerp(beeList.transform.localPosition, new Vector3(beeListIndexNumber * -600, 0), Time.deltaTime * 15f);

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
        PlayableCharacterInfo playableCharacterInfo = statusManager.playableCharacterInfos[beeListIndexNumber];

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
