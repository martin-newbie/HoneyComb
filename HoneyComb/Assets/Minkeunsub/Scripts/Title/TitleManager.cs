using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : Singleton<TitleManager>
{
    public Image blackImg;

    void Start()
    {
        StartCoroutine(FadeIn(3f));
    }

    void Update()
    {

    }

    IEnumerator SceneMove(string sceneName)
    {
        yield return StartCoroutine(FadeOut(3f));
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeIn(float delay)
    {
        float timer = delay;
        blackImg.gameObject.SetActive(true);
        while (timer > 0)
        {
            blackImg.color = new Color(0, 0, 0, timer / delay);
            timer -= Time.deltaTime;
            yield return null;
        }

        blackImg.gameObject.SetActive(false);
    }

    IEnumerator FadeOut(float delay)
    {
        float timer = 0f;
        blackImg.gameObject.SetActive(true);
        while (timer < delay)
        {
            blackImg.color = new Color(0, 0, 0, timer / delay);
            timer += Time.deltaTime;
            yield return null;
        }

    }

    public void GameStart()
    {
        StartCoroutine(SceneMove("InGameScene"));
    }

    public void RoyalSceneMove()
    {
        StartCoroutine(SceneMove("RoyalScene"));
    }

    public void LabSceneMove()
    {
        StartCoroutine(SceneMove("LabScene"));
    }

    public void LibrarySceneMove()
    {
        StartCoroutine(SceneMove("LibraryScene"));
    }
}
