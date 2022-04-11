using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float damage;

    TextAsset FlowerSpawnTxt;
    [SerializeField] List<string> FlowerTime = new List<string>();

    void Start()
    {
        PoolInit(20);
        originPos = cam.transform.position;
        FlowerSpawnTxt = Resources.Load("Texts/FlowerSpawn") as TextAsset;
        FlowerTime = FlowerSpawnTxt.text.Split('\n').ToList();
        StartCoroutine(SpawnCoroutine(0.5f));
    }

    void Update()
    {
        Player.transform.position = Vector3.Lerp(Player.transform.position, PlayerPoses[curDir].position, Time.deltaTime * 15f);
        DistanceLogic();
        InGameUI.Instance.SetStatusTexts(roundHoney, distance);
    }

    public void Revive()
    {
        Player.isGameOver = false;
        Player.Hp = Player.MaxHp;
    }

    public void SaveDataToManager()
    {
        StatusManager.Instance.Honey += roundHoney;
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
    
    IEnumerator SpawnCoroutine(float time)
    {
        float duration = time;
        int idx = 0;
        while (true)
        {
            Pop(PlayerPoses[int.Parse(FlowerTime[idx])].position + new Vector3(0, 9, 0));

            idx++;

            if (idx == FlowerTime.Count) idx = 0;

            yield return new WaitForSeconds(duration);

            while (Player.isGameOver) yield return null;
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
            Vector3 randPos = Random.insideUnitCircle * 0.1f;
            cam.transform.position = originPos + randPos;
            timer -= Time.deltaTime;
            yield return null;
        }

        cam.transform.position = originPos;
    }
}
