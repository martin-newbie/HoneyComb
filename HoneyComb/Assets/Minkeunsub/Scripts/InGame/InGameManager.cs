using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    [Header("Objects")]
    public Player Player;
    [SerializeField] Transform[] PlayerPoses = new Transform[3];
    public int curDir;
    [SerializeField] GameObject HoneyItem;
    [SerializeField] GameObject Obstruction;
    [SerializeField] Camera cam;
    Vector3 originPos;

    [Header("Status")]
    public int roundHoney; //한 판에서 얻은 꿀, 정상적으로 라운드를 종료해야만 획득 가능

    void Start()
    {
        originPos = cam.transform.position;
    }

    void Update()
    {
        Player.transform.position = Vector3.Lerp(Player.transform.position, PlayerPoses[curDir].position, Time.deltaTime * 15f);
    }

    public void SpawnHoney(int idx)
    {
        Instantiate(HoneyItem, PlayerPoses[idx].position + new Vector3(0, 9f), Quaternion.identity);
    }

    public void SpawnObstruction(int idx)
    {
        Instantiate(Obstruction, PlayerPoses[idx].position + new Vector3(0, 9f), Quaternion.identity);
    }

    public void GameOver()
    {

    }

    /// <summary>
    /// move player position
    /// </summary>
    /// <param name="dir">only can add -1 or 1</param>
    public void SetPlayerPos(int dir)
    {
        if(dir == -1)
        {
            if (curDir != 0) curDir += dir;
        }
        else if(dir == 1)
        {
            if (curDir != 2) curDir += dir;
        }
    }

    public void CameraShake(float duration)
    {
        StartCoroutine(CameraShakeCoroutine(duration));
    }

    IEnumerator CameraShakeCoroutine(float duration)
    {
        float timer = duration;
        while (timer > 0f)
        {
            Vector3 randPos = Random.insideUnitCircle * 0.3f;
            cam.transform.position = originPos + randPos;
            yield return null;
        }

        cam.transform.position = originPos;
    }
}
