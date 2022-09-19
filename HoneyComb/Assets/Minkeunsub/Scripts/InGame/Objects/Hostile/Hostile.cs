using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hostile : MonoBehaviour
{
    public virtual float objectMoveSpeed => InGameManager.Instance.curObjectMoveSpeed;

    protected virtual void Update()
    {
        if (transform.position.y <= -6f)
        {
            DestroyItem();
        }
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime * objectMoveSpeed);
        }
    }

    public abstract void DestroyItem();
    public abstract void Init();
}
