using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    Animator anim;
    bool nextTrigger;
    bool eventActing;

    [Header("UI")]
    public Image cardImg;
    public Text cardCount;
    public Image characterImg;
    public Image beeSilhouette;
    public GachaResult[] results;
    public Button offButton;
    public Text remainCount;

    [Header("Sprites")]
    public Sprite cardBack;
    public Sprite cardFront;

    [Header("Info")]
    public List<Sprite> characterImages = new List<Sprite>();
    public EPlayableCharacter[] randIdx;
    public int[] randCount;
    int curIdx;

    private void Start()
    {
        foreach (var item in ResourcesManager.Instance.PlayerFirstSprites)
        {
            characterImages.Add(item);
        }
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ResultDeactive();
        offButton.gameObject.SetActive(false);
        remainCount.gameObject.SetActive(true);
        List<EPlayableCharacter> characterType = new List<EPlayableCharacter>();
        for (int i = 0; i < (int)EPlayableCharacter.END; i++)
        {
            characterType.Add((EPlayableCharacter)i);
        }

        cardImg.gameObject.SetActive(false);
        cardCount.gameObject.SetActive(false);
        characterImg.gameObject.SetActive(false);

        randIdx = new EPlayableCharacter[Random.Range(1, 4)];
        randCount = new int[randIdx.Length];

        for (int i = 0; i < randIdx.Length; i++)
        {
            int rand = Random.Range(0, characterType.Count);
            randIdx[i] = characterType[rand];
            randCount[i] = Random.Range(1, 6);
            StatusManager.Instance.playableCharacterInfos[(int)randIdx[i]].IncreasePiece(randCount[i]);

            characterType.Remove(characterType[rand]);
        }


        StartCoroutine(PrintResult());
    }

    IEnumerator PrintResult()
    {
        yield return null;
        anim.SetTrigger("enable");
        eventActing = false;
        remainCount.text = (randIdx.Length).ToString();

        for (int i = 0; i <= randIdx.Length; i++)
        {
            nextTrigger = false;
            while (nextTrigger == false) yield return null;

            eventActing = true;

            curIdx = i;

            if (i < randIdx.Length)
            {
                remainCount.text = (randIdx.Length - (curIdx + 1)).ToString();
                anim.SetTrigger("nextEvent");
            }
            else
            {
                remainCount.text = "0";
                anim.SetTrigger("endEvent");
            }
        }
    }

    public void NextButton()
    {
        if (!eventActing)
            nextTrigger = true;
    }

    void PrintCharacterInfo()
    {
        StartCoroutine(PrintCharacterInfoCoroutine());
    }

    IEnumerator PrintCharacterInfoCoroutine()
    {

        eventActing = false;
        yield break;
    }

    void CardDrawStrat()
    {
        cardImg.gameObject.SetActive(true);
        cardCount.gameObject.SetActive(false);
        characterImg.gameObject.SetActive(false);

        cardImg.sprite = cardBack;
    }

    void CardDrawEnd()
    {
        cardCount.gameObject.SetActive(true);
        characterImg.gameObject.SetActive(true);

        cardImg.sprite = cardFront;
        cardCount.text = "x" + randCount[curIdx].ToString();

        characterImg.sprite = characterImages[(int)randIdx[curIdx]];
        characterImg.SetNativeSize();
    }

    void CardResultStart()
    {
        cardImg.gameObject.SetActive(false);
        StartCoroutine(CardResultCoroutine());
    }

    void ResultDeactive()
    {
        foreach (var item in results)
        {
            item.gameObject.SetActive(false);
        }
    }

    IEnumerator CardResultCoroutine()
    {
        for (int i = 0; i < randIdx.Length; i++)
        {
            results[i].Init(characterImages[(int)randIdx[i]], randCount[i]);
            yield return new WaitForSeconds(0.5f);
        }
        offButton.gameObject.SetActive(true);
    }
}
