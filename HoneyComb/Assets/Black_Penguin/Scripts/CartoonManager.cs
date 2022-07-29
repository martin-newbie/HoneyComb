using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class Cartoon
{
    public Image image;
    public float Duration;

    [Header("특수효과")]
    public bool isShaking;
    public float shakingScale;

    public bool isFade;
    public bool isScaleFade;

    public Action<IEnumerator> actionQueue;
    public bool forcingQuit;
}
[Serializable]
public class CartoonArray
{
    public List<Cartoon> cartoons;
}

public class CartoonManager : Singleton<CartoonManager>
{
    //근섭선배가 넣어야할 함수
    public Action func;

    public List<CartoonArray> cartoons;
    public Text pressPleaseText;

    public Image cartoonBlackScreen;

    private float delay;
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
        pressPleaseText.gameObject.SetActive(false);
        foreach (CartoonArray cartoonary in cartoons)
        {
            foreach (Cartoon cartoon in cartoonary.cartoons)
            {
                if (cartoon.image != null) cartoon.image.gameObject.SetActive(false);
            }
        }
    }
    private void Update()
    {
        pressPleaseText.gameObject.SetActive(delay > 3 ? true : false);
        pressPleaseText.rectTransform.localPosition += new Vector3(0, Mathf.Cos(Time.time) * 200 * Time.deltaTime, 0);
        pressPleaseText.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(Time.time)));
    }
    public void test()
    {
        CartoonStartFunction(0, null);
    }
    public void CartoonStartFunction(int cartoonNum, Action action)
    {
        if (cartoonNum == 0)
        {
            cartoonNum = 1;
            action += () => StartCoroutine(ImageFadeBlack(cartoonBlackScreen));
            func = () => CartoonStartFunction(-1, action);
            func += () => cartoonBlackScreen.gameObject.SetActive(true);
        }
        else if (cartoonNum == -1)
        {
            cartoonNum = 0;
            func = action;
        }
        else if (cartoons[cartoonNum] != null)
        {
            cartoonNum++;
            func = action;
        }
        CartoonStartFunction(cartoonNum);
    }
    public void CartoonStartFunction(int cartoonNum) => StartCoroutine(CartoonStart(cartoons[cartoonNum]));
    IEnumerator CartoonStart(CartoonArray funcCartoons)
    {
        foreach (Cartoon cartoon in funcCartoons.cartoons)
        {
            delay = 0;
            if (cartoon.image != null)
            {
                cartoon.image.gameObject.SetActive(true);

                if (cartoon.isFade)
                    cartoon.actionQueue += (func) => StartCoroutine(CartoonFade(cartoon));
                if (cartoon.isShaking)
                    cartoon.actionQueue += (func) => StartCoroutine(CartoonShake(cartoon));
                if (cartoon.isScaleFade)
                    cartoon.actionQueue += (func) => StartCoroutine(CartoonScale(cartoon));

                cartoon.actionQueue?.Invoke(null);
                while (!Input.GetMouseButtonDown(0) || delay < cartoon.Duration)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        SoundManager.Instance.PlaySound("Button_Click");
                        cartoon.forcingQuit = true;
                        break;
                    }
                    delay += Time.deltaTime;
                    yield return null;
                }
                yield return new WaitForSeconds(0.1f);
                cartoon.forcingQuit = false;
            }
        }
        foreach (Cartoon cartoon in funcCartoons.cartoons)
        {
            if (cartoon.image != null)
                StartCoroutine(CartoonOff(cartoon));
        }
        delay = 0;
        cartoonBlackScreen.gameObject.SetActive(false);
        func?.Invoke();
    }
    public IEnumerator CartoonShake(Cartoon cartoon)
    {
        float duration = cartoon.Duration;
        Vector2 pos = cartoon.image.rectTransform.localPosition;
        while (duration > 0)
        {
            if (cartoon.forcingQuit) break;
            cartoon.image.rectTransform.localPosition = Random.insideUnitCircle * cartoon.shakingScale + pos;

            yield return null;
            duration -= Time.deltaTime;
        }
        cartoon.image.rectTransform.localPosition = pos;
    }

    public IEnumerator CartoonFade(Cartoon cartoon)
    {
        cartoon.image.color = new Color(1, 1, 1, 0) * cartoon.image.color;
        while (Mathf.Approximately(cartoon.image.color.a, 1) == false)
        {
            if (cartoon.forcingQuit) cartoon.image.color = Color.white;
            cartoon.image.color += new Color(0, 0, 0, Time.deltaTime / cartoon.Duration);
            yield return null;
        }
    }
    public IEnumerator ImageFadeBlack(Image cartoon)
    {
        cartoon.color = new Color(1, 1, 1, 1);//Color.White; 
        while (cartoon.color.a > 0.01f)
        {
            cartoon.color = Color.Lerp(cartoon.color, Color.clear, Time.deltaTime);
            yield return null;
        }
        cartoon.gameObject.SetActive(false);
    }
    public IEnumerator CartoonScale(Cartoon cartoon)
    {
        Vector2 originalSize = cartoon.image.rectTransform.sizeDelta;
        cartoon.image.rectTransform.sizeDelta = Vector2.zero;
        while (cartoon.image.rectTransform.sizeDelta.x < originalSize.x)
        {
            if (cartoon.forcingQuit) cartoon.image.rectTransform.sizeDelta = originalSize;
            cartoon.image.rectTransform.sizeDelta += (originalSize * Time.deltaTime) / cartoon.Duration;
            yield return null;
        }
    }
    public IEnumerator CartoonOff(Cartoon cartoon)
    {
        cartoon.image.color = new Color(1, 1, 1, 1);
        while (cartoon.image.color.a > 0.001f)
        {
            cartoon.image.color = new Color(1, 1, 1, Mathf.Lerp(cartoon.image.color.a, -1, Time.deltaTime * 4));
            yield return null;
        }
        cartoon.image.gameObject.SetActive(false);
    }
}

