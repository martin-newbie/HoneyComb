using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Cartoon
{
    public Image image;
    public float Duration;
    System.Action<Image> action;
}

public class CartoonManager : MonoBehaviour
{
    float MaxDuration;

    IEnumerator CartoonStart(Cartoon[] funcCartoons)
    {
        foreach (Cartoon cartoon in funcCartoons)
        {
            cartoon.image.gameObject.SetActive(true);
            while (Input.GetMouseButtonDown(0))
                yield return null;
        }

    }
    public IEnumerator CartoonShake(Cartoon cartoon, float shakeScale, float shakeRange, float duration)
    {
        Vector2 pos = cartoon.image.rectTransform.localPosition;
        while (duration <= 0)
        {
            cartoon.image.rectTransform.position = Random.insideUnitCircle * shakeScale * pos;

            yield return new WaitForSeconds(cartoon.Duration);
            duration -= cartoon.Duration * Time.deltaTime;

        }
        cartoon.image.rectTransform.localPosition = pos;
    }

    public IEnumerator AlphaCotrol(Cartoon cartoon)
    {
        while (cartoon.image.color.a != 1)
        {
            cartoon.image.color += new Color(0, 0, 0, Time.deltaTime / cartoon.Duration);
            yield return null;
        }
    }

}

