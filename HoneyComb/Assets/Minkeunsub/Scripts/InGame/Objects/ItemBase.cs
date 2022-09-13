using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected virtual void Update()
    {
        if (transform.position.y <= -6f)
        {
            DestroyItem();
        }
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime * InGameManager.Instance.objectMoveSpeed);
        }
    }
    public abstract void DestroyItem();
}
