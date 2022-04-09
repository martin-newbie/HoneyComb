using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruction : MonoBehaviour
{

    [SerializeField] List<SpriteRenderer> Obj = new List<SpriteRenderer>();

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    void Init()
    {
        Obj.ForEach(item => item.gameObject.SetActive(false));
        int rand = Random.Range(0, 2);
        Obj[rand].gameObject.SetActive(true);
    }
}
