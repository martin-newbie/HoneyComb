using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Cartoon
{
    public Image image;
    public Action<Image> action;
}

public class CartoonManager : MonoBehaviour
{
    float MaxDuration;
    IEnumerator CartoonStart(Cartoon[] funcCartoons, Action<Image> action)
    {
        foreach (Cartoon cartoon in funcCartoons)
        {
            cartoon.image.gameObject.SetActive(true);
            while (Input.GetMouseButtonDown(0))
                yield return null;
        }
    }
    public IEnumerator CartoonShake(RectTransform rectPostition, float shakeScale, int shakeTime, float shakeRange, float duration)
    {
        if (MaxDuration < shakeTime)
            MaxDuration = shakeTime;
        Vector2 pos = rectPostition.localPosition;
        while (duration <= 0)
        {
            rectPostition.position = UnityEngine.Random.insideUnitCircle * shakeScale * pos;

            yield return new WaitForSeconds(shakeTime);
            duration -= shakeTime * Time.deltaTime;

        }
        rectPostition.localPosition = pos;
    }
}

