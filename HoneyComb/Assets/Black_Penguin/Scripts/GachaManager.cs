using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class GachaManager : MonoBehaviour
{
    public GameObject MainBoard;
    public Image characterImage;
    public Text text;
    public Image whiteImage;
    public void DoGacha()
    {
        if (StatusManager.Instance.BeeWax >= 10)
        {
            SoundManager.Instance.PlaySound("Button_Click");
            StartCoroutine(DoingGacha());
        }
        else
        {
            Debug.Log("왁스가 10개 필요해용");
            SoundManager.Instance.PlaySound("Button_Click_Fail");
        }
    }
    IEnumerator DoingGacha()
    {
        whiteImage.DOFade(1, 2);
        yield return new WaitForSeconds(2);

        whiteImage.color = new Color(1, 1, 1, 0);
        List<CharacterScript> characterScripts = Resources.LoadAll<CharacterScript>("Characters/").ToList();
        MainBoard.SetActive(true);

        int addCount = Random.Range(10, 20);

        StatusManager.Instance.BeeWax -= 10;
        PlayableCharacterInfo playableCharacterInfo = StatusManager.Instance.playableCharacterInfos[Random.Range(0, StatusManager.Instance.playableCharacterInfos.Count)];

        playableCharacterInfo._pieceCount += addCount;

        CharacterScript Info = characterScripts.Find((x) => x.characterType == playableCharacterInfo.character);
        characterImage.sprite = Info.Icon;
        text.text = $"{Info.characterName}의 조각 {addCount}개";

        StartCoroutine(OffGacha());
    }
    IEnumerator OffGacha()
    {
        yield return new WaitForSeconds(1);

        List<Image> FadeImg = new List<Image>();
        FadeImg.Add(MainBoard.GetComponent<Image>());
        FadeImg.Add(MainBoard.transform.GetChild(0).GetComponent<Image>());
        Text text = MainBoard.GetComponentInChildren<Text>();

        foreach (Image image in FadeImg) image.DOFade(0, 2f);
        text.DOFade(0, 2f);
        yield return new WaitForSeconds(2f);
        foreach (Image image in FadeImg) image.color = Color.white;
        text.color = Color.white;
        MainBoard.SetActive(false);
    }
}
