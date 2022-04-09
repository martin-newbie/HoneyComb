using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyItem : MonoBehaviour
{
    public int value;
    [SerializeField] List<SpriteRenderer> Flowers = new List<SpriteRenderer>();
    [SerializeField] GameObject Honey;
    InGameManager manager;

    void Start()
    {
        
    }


    public void Init(InGameManager manager)
    {
        this.manager = manager;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">spawn position</param>
    /// <param name="kind">-1 is random</param>
    public void PosInit(Vector3 position, int kind = -1)
    {
        if(kind == -1)
        {
            kind = Random.Range(0, Flowers.Count);
        }

        Flowers.ForEach(item => item.gameObject.SetActive(false));
        Flowers[kind].gameObject.SetActive(true);
        transform.position = position;
        Honey.SetActive(true);

    }

    void Update()
    {
        if(transform.position.y <= -6f)
        {
            Push();
        }
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime * 5);
        }
    }

    void Push()
    {
        manager.Push(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Honey.SetActive(false);
            InGameManager.Instance.roundHoney += value;
        }
    }
}
