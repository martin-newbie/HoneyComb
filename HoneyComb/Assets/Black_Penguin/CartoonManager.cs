using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


[System.Serializable]
public class Cartoon
{
    public Image image;
    public float Duration;

    [Header("특수효과")]
    public bool isShaking;
    public float shakingScale;

    public bool isFade;
    public bool isScaleFade;

    public System.Action<IEnumerator> actionQueue;
}
[System.Serializable]
public class CartoonArray
{
    public Cartoon[] cartoons;
}

public class CartoonManager : Singleton<CartoonManager>
{
    //근섭선배가 넣어야할 함수
    public System.Action Func;
    public List<CartoonArray> cartoons;

    private void Awake()
    {
        var objs = FindObjectsOfType<CartoonManager>();
        foreach (var obj in objs)
        {
            if (obj.gameObject != gameObject)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
    private void Start()
    {
        foreach (CartoonArray cartoonary in cartoons)
        {
            foreach (Cartoon cartoon in cartoonary.cartoons)
            {
                cartoon.image.gameObject.SetActive(false);
            }
        }
        //테스트용 코드
        //StartCoroutine(CartoonStart(cartoons[0]));

    }
    public void CartoonStartFunction(int cartoonNum, System.Action action)
    {
        Func = action;
        StartCoroutine(CartoonStart(cartoons[cartoonNum]));
    }
    public void CartoonStartFunction(int cartoonNum)
    {
        StartCoroutine(CartoonStart(cartoons[cartoonNum]));
    }

    IEnumerator CartoonStart(CartoonArray funcCartoons)
    {
        foreach (Cartoon cartoon in funcCartoons.cartoons)
        {
            float Delay = 0;

            cartoon.image.gameObject.SetActive(true);

            if (cartoon.isFade)
                cartoon.actionQueue += (func) => StartCoroutine(CartoonFade(cartoon));
            if (cartoon.isShaking)
                cartoon.actionQueue += (func) => StartCoroutine(CartoonShake(cartoon));
            if (cartoon.isScaleFade)
                cartoon.actionQueue += (func) => StartCoroutine(CartoonScale(cartoon));

            cartoon.actionQueue?.Invoke(null);
            while (!Input.GetMouseButtonDown(0) || Delay < cartoon.Duration)
            {
                Delay += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
        Func?.Invoke();
    }
    public IEnumerator CartoonShake(Cartoon cartoon)
    {
        float duration = cartoon.Duration;
        Vector2 pos = cartoon.image.rectTransform.localPosition;
        while (duration > 0)
        {
            cartoon.image.rectTransform.localPosition = Random.insideUnitCircle * cartoon.shakingScale + pos;

            yield return null;
            duration -= Time.deltaTime;
        }
        cartoon.image.rectTransform.localPosition = pos;
    }

    public IEnumerator CartoonFade(Cartoon cartoon)
    {
        cartoon.image.color = new Color(1, 1, 1, 0);
        while (Mathf.Approximately(cartoon.image.color.a, 1) == false)
        {
            cartoon.image.color += new Color(0, 0, 0, Time.deltaTime / cartoon.Duration);
            yield return null;
        }
    }

    public IEnumerator CartoonScale(Cartoon cartoon)
    {
        Vector2 originalSize = cartoon.image.rectTransform.sizeDelta;
        cartoon.image.rectTransform.sizeDelta = Vector2.zero;
        while (cartoon.image.rectTransform.sizeDelta.x < originalSize.x)
        {
            cartoon.image.rectTransform.sizeDelta += (originalSize * Time.deltaTime) / cartoon.Duration;
            yield return null;
        }
    }

}

