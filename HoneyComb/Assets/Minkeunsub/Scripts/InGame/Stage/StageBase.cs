using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StageBase : MonoBehaviour
{
    [Header("Stage Base")]
    public string spawnInfoTextPath;
    public Background background;
    public HoneyItem flower;
    public Hostile[] commonHurdle;

    public virtual List<string> GetFlowerSpawn()
    {
        TextAsset flowerSpawnTxt = Resources.Load("Texts/" + spawnInfoTextPath) as TextAsset;
        List<string> flowerTime = flowerSpawnTxt.text.Split('\n').ToList();
        return flowerTime;
    }

    public virtual Background SpawnBackground(int idx, float height, InGameManager manager)
    {
        Background temp = Instantiate(background);
        temp.Init(height * 4, height * -2, manager);
        temp.transform.position = new Vector3(0, idx * height * 2, 0);

        return temp;
    }

    public virtual HoneyItem SpawnFlower(Vector3 position, bool hostileAble)
    {
        HoneyItem temp = Instantiate(flower);
        flower.transform.position = position;

        if(Random.Range(0, 10) == 0)
        {
            SpawnHurdle(position);
        }

        return temp;
    }

    public virtual Hostile SpawnHurdle(Vector3 position)
    {
        int rand = Random.Range(0, commonHurdle.Length);
        Hostile temp = Instantiate(commonHurdle[rand], position, Quaternion.identity);
        temp.Init();
        return temp;
    }

}
