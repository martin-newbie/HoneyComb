using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    [SerializeField] private Transform positionParent;
    private Transform[] positions = new Transform[3];

    private float itemCreateTime = 10;
    private float curItemCreateTime = 0;
    private ItemBase[] items;
    private void Start()
    {
        items = Resources.LoadAll<ItemBase>("Items/");
        positions = new Transform[] { positionParent };
    }
    private void Update()
    {
        if (curItemCreateTime > itemCreateTime)
        {
            curItemCreateTime = 0;
            Instantiate(items[Random.Range(0, items.Length)], positions[Random.Range(0, 3)].position, Quaternion.identity);
        }
        curItemCreateTime += Time.deltaTime;
    }
}
