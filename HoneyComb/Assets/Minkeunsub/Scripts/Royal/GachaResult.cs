using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GachaResult : MonoBehaviour
{
    public Image characterImage;
    public Text characterCount;
    public Image whiteBlur;

    public void Init(Sprite character, int count)
    {
        whiteBlur.color = Color.white;
        whiteBlur.DOColor(new Color(1, 1, 1, 0), 0.5f);

        gameObject.SetActive(true);

        characterImage.sprite = character;
        characterImage.SetNativeSize();
        characterImage.rectTransform.sizeDelta *= 0.5f;

        characterCount.text = "x" + count.ToString();
    }
}
