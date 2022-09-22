using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    [Header("Objects")]
    public List<Transform> HexagonStencil = new List<Transform>();
    public GameObject Mask;

    [Header("Value")]
    public string nextSceneName;
    public float fadeSpeed = 0.5f;
    public float delaySpeed = 0.3f;
    public float hexSize = 0.11f;

    private List<Transform[]> HexagonMutArr = new List<Transform[]>();
    private int[] arrCount = new int[8] { 1, 2, 3, 4, 4, 3, 2, 1 };

    bool isActing = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        int index = 0;
        for (int i = 0; i < arrCount.Length; i++)
        {
            Transform[] trArr = new Transform[arrCount[i]];
            for (int j = 0; j < arrCount[i]; j++)
            {
                trArr[j] = HexagonStencil[index];
                index++;
            }
            HexagonMutArr.Add(trArr);
        }

    }

    public void LoadScene(string name)
    {
        if (!isActing)
        {
            nextSceneName = name;
            StartCoroutine(SceneMoveCoroutine());
        }
    }

    IEnumerator SceneMoveCoroutine()
    {
        isActing = true;
        
        yield return StartCoroutine(FadeInCoroutine());
        SceneManager.LoadScene(nextSceneName);
        yield return StartCoroutine(FadeOutCoroutine());

        isActing = false;
        yield break;
    }

    IEnumerator FadeOutCoroutine()
    {
        for (int i = 0; i < HexagonMutArr.Count; i++)
        {
            foreach (var item in HexagonMutArr[i])
            {
                item.DOScale(new Vector3(hexSize, hexSize, 1f), fadeSpeed);
            }
            yield return new WaitForSeconds(delaySpeed);
        }
        yield return new WaitForSeconds(fadeSpeed - delaySpeed);

        Mask.SetActive(false);
        foreach (var item in HexagonStencil)
        {
            item.gameObject.SetActive(false);
        }

        yield break;
    }

    IEnumerator FadeInCoroutine()
    {
        Mask.SetActive(true);

        foreach (var item in HexagonStencil)
        {
            item.localScale = new Vector3(hexSize, hexSize, 1);
            item.gameObject.SetActive(true);
        }

        for (int i = HexagonMutArr.Count - 1; i >= 0; i--)
        {
            foreach (var item in HexagonMutArr[i])
            {
                item.DOScale(new Vector3(0f, 0f, 1f), fadeSpeed);
            }
            yield return new WaitForSeconds(delaySpeed);
        }
        yield return new WaitForSeconds(fadeSpeed - delaySpeed);


        yield break;
    }
}
