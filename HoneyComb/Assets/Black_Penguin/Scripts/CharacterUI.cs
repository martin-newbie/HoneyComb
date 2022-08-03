using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public EPlayableCharacter nowShowCharacterType;

    [SerializeField] private GameObject beeList;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;

    private int beeListIndexNumber;

    private SoundManager soundManager;
    private StatusManager statusManager;
    private List<CharacterScript> characterScripts;

    private void Start()
    {
        statusManager = StatusManager.Instance;
        soundManager = SoundManager.Instance;

        characterScripts = Resources.LoadAll<CharacterScript>("Characters/").ToList();
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
    }

    private void OnDestroy()
    {
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
            statusManager.nowCharacter = nowShowCharacterType;
        }
        else
        {
            soundManager.PlaySound("Button_Click_Fail");
        }
    }
}
