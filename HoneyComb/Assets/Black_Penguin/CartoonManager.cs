using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Cartoon
{
    public Image image;
    public Action<Image> action;
}

public class CartoonManager : MonoBehaviour
{
    Cartoon[] Cartoons;
    IEnumerator CartoonStart(Cartoon[] funcCartoons,Action<Image> action)
    {
        foreach (Cartoon cartoon in funcCartoons )
        {
            cartoon.image.gameObject.SetActive(true);
            cartoon
            while (Input.GetMouseButtonDown(0))
                yield return null;
        }
    }
}
