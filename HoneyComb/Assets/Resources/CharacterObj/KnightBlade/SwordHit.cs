using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    BoxCollider2D boxCollider;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    public void OnAttack()
    {
        foreach (RaycastHit2D physics2D in Physics2D.BoxCastAll(transform.position, boxCollider.size, 0, Vector3.forward))
        {
            if (physics2D.transform.gameObject.GetComponent<Obstruction>())
            {
                Destroy(physics2D.transform.gameObject);
            }
        }
    }
    public void EndAnim()
    {
        gameObject.SetActive(false);
    }
}
