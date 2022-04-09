using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    [Header("Objects")]
    public Player Player;
    [SerializeField] Transform[] PlayerPoses = new Transform[3];
    public int curDir;
    [SerializeField] HoneyItem honeyItem;
    [SerializeField] GameObject Obstruction;
    [SerializeField] Camera cam;
    Vector3 originPos;
    Stack<HoneyItem> HoneyItemPool = new Stack<HoneyItem>();

    [Header("Status")]
    public int roundHoney; //한 판에서 얻은 꿀, 정상적으로 라운드를 종료해야만 획득 가능
    public float distance;
    public float moveSpeed;

    void Start()
    {
        PoolInit(20);
        originPos = cam.transform.position;
    }

    void Update()
    {
        Player.transform.position = Vector3.Lerp(Player.transform.position, PlayerPoses[curDir].position, Time.deltaTime * 15f);
        DistanceLogic();
    }

    void DistanceLogic()
    {
        if (!Player.isGameOver)
            distance += Time.deltaTime * moveSpeed;
    }
    
    void PoolInit(int count)
    {
        for (int i = 0; i < count; i++)
        {
            HoneyItem temp = Instantiate(honeyItem, transform);
            temp.Init(this);
            temp.gameObject.SetActive(false);
            HoneyItemPool.Push(temp);
        }
    }

    public void Push(HoneyItem item)
    {
        item.gameObject.SetActive(false);
        HoneyItemPool.Push(item);
    }

    public HoneyItem Pop(Vector3 position)
    {
        HoneyItem retmp;
        retmp = HoneyItemPool.Pop();
        retmp.gameObject.SetActive(true);
        retmp.PosInit(position);
        return retmp;
    }

    public void GameOver()
    {
        InGameUI.Instance.GameOverUIOn(distance, roundHoney);
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
