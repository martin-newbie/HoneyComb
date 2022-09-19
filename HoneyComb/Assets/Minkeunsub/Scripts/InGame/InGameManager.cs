using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{

    StageBase[] stages;
    StageBase curStage;

    [Header("Objects")]
    public Player Player;
    public Transform[] PlayerPoses = new Transform[3];
    public int curDir;
    [SerializeField] BookItem BookItem;
    [SerializeField] Camera cam;
    Vector3 originPos;

    [Header("Status")]
    public int roundHoney; //한 판에서 얻은 꿀, 정상적으로 라운드를 종료해야만 획득 가능
    public float distance;
    public float curObjectMoveSpeed;
    public float objectMoveSpeed;
    public float moveSpeed;
    public float damage;

    [SerializeField] List<string> FlowerTime = new List<string>();

    [Header("Books")]
    [SerializeField] bool bookCollectAble; //그 판에서 책 획득할 수 있는 조건
    [SerializeField] int ableBookIdx; //StatusManager의 BookUnlocked에서 해제될 수 있는 인덱스
    [SerializeField] int bookSpawnIdx; //책이 스폰될 꽃의 인덱스

    private float height;

    void Start()
    {

        SpawnCharacter();
        stages = GetComponents<StageBase>();
        int mapIdx = PlayerPrefs.GetInt("MapIdx", 0);

        curStage = stages[mapIdx];

        originPos = cam.transform.position;
        FlowerTime = curStage.GetFlowerSpawn();

        SetBookAble();
        StartCoroutine(SpawnCoroutine(0.5f));

        height = Camera.main.orthographicSize;

        for (int i = 0; i < 2; i++)
        {
            Background temp = curStage.SpawnBackground(i, height, this);
            SetSpriteCameraSize(temp.GetComponent<SpriteRenderer>());
        }
        Camera.main.CameraSizeSet(1440, 2960);
    }
    private void SpawnCharacter()
    {
        List<Player> players = Resources.LoadAll<Player>("CharacterObj/").ToList();
        Player = Instantiate(players.Find((x) => x.characterType == StatusManager.Instance.nowCharacter), new Vector3(0, -4, 0), Quaternion.identity);
    }
    public Vector2 SetSpriteCameraSize(SpriteRenderer SR)
    {
        float X = SR.bounds.size.x;
        float Y = SR.bounds.size.y;

        float screenY = Camera.main.orthographicSize * 2;
        float screenX = screenY / Screen.height * Screen.width;

        Vector2 Scale = new Vector2(screenY / Y, screenY / Y);
        SR.transform.localScale = Scale;
        return Scale;
    }


    void SetBookAble()
    {
        if (StatusManager.Instance.bookAble)
        {
            int chance = Random.Range(0, 100);
            int per = 15;
            if (StatusManager.Instance.debug) per = 100;
            if (chance <= per)
            {
                List<int> ableArr = new List<int>();
                for (int i = 0; i < StatusManager.Instance.BookUnlocked.Count; i++)
                {
                    if (!StatusManager.Instance.BookUnlocked[i])
                    {
                        ableArr.Add(i);
                    }
                }

                if (ableArr.Count > 0)
                {
                    ableBookIdx = ableArr[Random.Range(0, ableArr.Count)];
                    bookCollectAble = true;
                    bookSpawnIdx = Random.Range(150, 500);

                    if (StatusManager.Instance.debug) bookSpawnIdx = 10;
                }
            }
        }
    }

    void Update()
    {
        Player.transform.position = Vector3.Lerp(Player.transform.position, PlayerPoses[curDir].position, Time.deltaTime * 15f);
        DistanceLogic();
        InGameUI.Instance.SetStatusTexts(roundHoney, distance);

        if (!Player.isGameOver)
            ComputerMove();
    }

    void ComputerMove()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) SetPlayerPos(1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) SetPlayerPos(-1);
    }

    public void Revive()
    {
        Player.isGameOver = false;
        Player.Hp = Player.MaxHp / 3;

        StartCoroutine(ReviveSpeedUp(2f));
    }

    public void SaveDataToManager()
    {
        if (StatusManager.Instance.nowCharacter == EPlayableCharacter.PUMPKIN_BEE)
            StatusManager.Instance.Honey += (int)(roundHoney * 1.5f);
        else
            StatusManager.Instance.Honey += roundHoney;
    }

    void DistanceLogic()
    {
        if (!Player.isGameOver)
            distance += Time.deltaTime * moveSpeed;
    }

    public void BookGet()
    {
        bookCollectAble = false;
        StatusManager.Instance.BookUnlocked[ableBookIdx] = true;

        InGameUI.Instance.GetBookEffect(StatusManager.Instance.BookDatas[ableBookIdx], Player.transform.position);
    }

    IEnumerator SpawnCoroutine(float time)
    {
        float duration = time;
        int idx = 0;
        int curBookIdx = 0;
        while (true)
        {
            bool bookAble = curBookIdx == bookSpawnIdx && bookCollectAble;
            HoneyItem temp = curStage.SpawnFlower(PlayerPoses[int.Parse(FlowerTime[idx])].position + new Vector3(0, 9, 9), !bookAble);

            if (bookAble)
            {
                BookItem tempBook = Instantiate(BookItem, Vector3.zero, Quaternion.identity, temp.transform);
                tempBook.transform.localPosition = Vector3.zero;
            }
            idx++;

            if (bookCollectAble)
                curBookIdx++;

            if (idx == FlowerTime.Count) idx = FlowerTime.Count / 2;

            yield return new WaitForSeconds(duration);

            while (Player.isGameOver || curObjectMoveSpeed != objectMoveSpeed) yield return null;
        }
    }

    public void GameOver()
    {
        StartCoroutine(GameoverSlowDown(2f));
    }

    IEnumerator GameoverSlowDown(float duration)
    {
        float timer = duration;
        while (timer > 0f)
        {
            curObjectMoveSpeed = objectMoveSpeed * (timer / duration);
            timer -= Time.deltaTime;
            yield return null;
        }

        curObjectMoveSpeed = 0f;
        InGameUI.Instance.GameOverUIOn(distance, roundHoney);
    }

    IEnumerator ReviveSpeedUp(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            curObjectMoveSpeed = objectMoveSpeed * (timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        curObjectMoveSpeed = objectMoveSpeed;
    }

    /// <summary>
    /// move player position
    /// </summary>
    /// <param name="dir">only can add -1 or 1</param>
    public void SetPlayerPos(int dir)
    {
        if (dir == -1)
        {
            if (curDir != 0) curDir += dir;
        }
        else if (dir == 1)
        {
            if (curDir != 2) curDir += dir;
        }
        DailyQuest.Instance.moveCount++;
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
