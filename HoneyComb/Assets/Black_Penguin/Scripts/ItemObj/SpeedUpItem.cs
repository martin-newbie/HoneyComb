using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : ItemBase
{
    private bool canGet = true;
    public override void DestroyItem()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canGet == true)
        {
            canGet = false;
            InGameManager.Instance.StartCoroutine(SpeedUp());
            SoundManager.Instance.PlaySound("Button_Click2");
            Destroy(gameObject);
        }
    }
    public IEnumerator SpeedUp()
    {
        InGameManager.Instance.curObjectMoveSpeed += 3;
        yield return new WaitForSeconds(5);
        InGameManager.Instance.curObjectMoveSpeed -= 3;
    }
}
