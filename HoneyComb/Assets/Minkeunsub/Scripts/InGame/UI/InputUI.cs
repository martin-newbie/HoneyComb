using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputUI : MonoBehaviour
{

    [SerializeField] Vector2 firstPressPos;
    [SerializeField] Vector2 secondPressPos;
    [SerializeField] Vector3 currentSwipe;

    [SerializeField] bool isTouching = false;
    Coroutine nowCoroutine;

    private void Update()
    {
        if (!InGameManager.Instance.Player.isGameOver)
            TouchCheck();
    }

    void TouchCheck()
    {
        if (isTouching)
        {
            if (Input.GetMouseButton(0) && isTouching)
            {
                firstPressPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                nowCoroutine = StartCoroutine(SwipeCoroutine());
                isTouching = false;
            }
            if(Input.GetMouseButtonUp(0) && !isTouching)
            {
                StopCoroutine(nowCoroutine);
                isTouching = true;
            }
        }
    }

    IEnumerator SwipeCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        secondPressPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentSwipe = secondPressPos - firstPressPos;
        currentSwipe.Normalize();

        if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
        {
            InGameManager.Instance.SetPlayerPos(-1);
            //swipe left
        }
        if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
        {
            InGameManager.Instance.SetPlayerPos(1);
            //swipe right
        }
    }

    public void OnPointerDown()
    {
        isTouching = true;
    }

    public void OnEndDrag()
    {
        isTouching = false;
    }
}
