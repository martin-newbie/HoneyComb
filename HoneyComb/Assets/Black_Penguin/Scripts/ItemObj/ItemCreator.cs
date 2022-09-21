using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    [SerializeField] private Transform positionParent;
    [SerializeField] private Transform[] positions = new Transform[3];

    private float itemCreateTime = 10;
    [SerializeField] private float curItemCreateTime = 0;
    [SerializeField] private ItemBase[] items;
    private void Start()
    {
        items = Resources.LoadAll<ItemBase>("Items/");
        positions = new Transform[] { positionParent.GetChild(0), positionParent.GetChild(1), positionParent.GetChild(2) };
    }
    private void Update()
    {
        if (curItemCreateTime > itemCreateTime)
        {
            curItemCreateTime = 0;
            Instantiate(items[Random.Range(0, items.Length)].gameObject, positions[Random.Range(0, 3)].position + new Vector3(0, 9, 9), Quaternion.identity);
        }
        curItemCreateTime += Time.deltaTime;
    }
}
