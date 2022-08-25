using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GachaManager : MonoBehaviour
{
    public GameObject MainBoard;
    public Image image;
    public Text text;
    public void DoGacha()
    {
        if (StatusManager.Instance.BeeWax >= 10)
        {
            SoundManager.Instance.PlaySound("Button_Click");
            List<CharacterScript> characterScripts = Resources.LoadAll<CharacterScript>("Characters/").ToList();
            MainBoard.SetActive(true);

            int addCount = Random.Range(10, 20);

            StatusManager.Instance.BeeWax -= 10;
            PlayableCharacterInfo playableCharacterInfo = StatusManager.Instance.playableCharacterInfos
                [Random.Range(0, StatusManager.Instance.playableCharacterInfos.Count)];

            playableCharacterInfo.pieceCount += addCount;

            CharacterScript Info = characterScripts.Find((x) => x.characterType == playableCharacterInfo.character);
            image.sprite = Info.Icon;
            text.text = $"{Info.characterName}의 조각 {addCount}개";
        }
        else
        {
            Debug.Log("왁스가 10개 필요해용");
            SoundManager.Instance.PlaySound("Button_Click_Fail");
        }
    }
    IEnumerator OffGacha()
    {
        yield return new WaitForSeconds(1.5f);
        MainBoard.SetActive(false);
    }
}
