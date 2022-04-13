using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Cartoon
{
    public Image image;
    public float Duration;
    public delegate IEnumerator action(Cartoon cartoon);
}

public class CartoonManager : MonoBehaviour
{
    float MaxDuration;

    private void Start()
    {
        Cartoon cartoon = new Cartoon();
        //cartoon.action += CartoonAlpha(cartoon);
        //cartoon.action
    }
    IEnumerator CartoonStart(Cartoon[] funcCartoons)
    {
        foreach (Cartoon cartoon in funcCartoons)
        {
            cartoon.image.gameObject.SetActive(true);
            while (Input.GetMouseButtonDown(0))
                yield return null;
        }

    }
    public IEnumerator CartoonShake(Cartoon cartoon, float shakeScale, float duration)
    {
        Vector2 pos = cartoon.image.rectTransform.localPosition;
        while (cartoon.Duration <= 0)
        {
            cartoon.image.rectTransform.position = Random.insideUnitCircle * shakeScale * pos;

            yield return new WaitForSeconds(cartoon.Duration);
            cartoon.Duration -= Time.deltaTime;
        }
        cartoon.image.rectTransform.localPosition = pos;
    }

    public IEnumerator CartoonAlpha(Cartoon cartoon)
    {
        while (cartoon.image.color.a != 1)
        {
            cartoon.image.color += new Color(0, 0, 0, Time.deltaTime / cartoon.Duration);
            yield return null;
        }
    }

}

