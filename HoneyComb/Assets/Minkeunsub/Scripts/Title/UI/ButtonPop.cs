using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ButtonPop : MonoBehaviour
{

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ButtonAnim()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        anim.SetTrigger("Trigger_1");
    }
}
