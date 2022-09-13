using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField] Vector2 firstPressPos;
    [SerializeField] Vector2 secondPressPos;

    [SerializeField] bool isDrag = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        firstPressPos = eventData.position;
        isDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag)
        {
            secondPressPos = eventData.position;

            if(Vector3.Distance(firstPressPos, secondPressPos) > 300f)
            {
                isDrag = false;
                MoveLogic();
            }

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDrag)
        {
            MoveLogic();
        }
    }

    void MoveLogic()
    {
        if(firstPressPos.x > secondPressPos.x)
        {
            InGameManager.Instance.SetPlayerPos(-1);
        }
        else if(firstPressPos.x < secondPressPos.x)
        {
            InGameManager.Instance.SetPlayerPos(1);
        }
    }
}
