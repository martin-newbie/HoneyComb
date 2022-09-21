using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] private float ItemSpeed = 1;
    protected virtual void Update()
    {
        if (transform.position.y <= -6f)
        {
            DestroyItem();
        }
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime * InGameManager.Instance.curObjectMoveSpeed * ItemSpeed);
        }
    }
    public abstract void DestroyItem();
}
