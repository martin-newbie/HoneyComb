using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBackground : MonoBehaviour
{
    public float decrease;
    float start, end, moveSpeed;
    InGameManager manager;

    private void Update()
    {
        moveSpeed = manager.curObjectMoveSpeed * decrease;
        if (transform.position.y < end)
        {
            transform.position += new Vector3(0, start, 0);
        }

        transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
    }

    public void Init(float _start, float _end, InGameManager _manager)
    {
        start = _start;
        end = _end;
        manager = _manager;
    }
}
