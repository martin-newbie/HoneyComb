using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    [SerializeField] GameObject clickEffectObj;
    [SerializeField] ParticleSystem particle;
    TrailRenderer trail;
    private bool isClicked;
    private void Awake()
    {
        trail = clickEffectObj.GetComponent<TrailRenderer>();
        clickEffectObj = transform.GetChild(0).gameObject;
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        ClickEffectPlay();
        clickEffectObj.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;
        trail.enabled = isClicked;
    }
    void ClickEffectPlay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isClicked = true;
            particle.Play();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isClicked = false;
        }
    }
}
