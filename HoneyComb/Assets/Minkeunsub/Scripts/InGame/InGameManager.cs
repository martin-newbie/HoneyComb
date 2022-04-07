using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    [Header("Objects")]
    public GameObject Player;
    [SerializeField] Transform[] PlayerPoses = new Transform[3];
    public int curDir;

    void Start()
    {
        
    }

    void Update()
    {
        Player.transform.position = Vector3.Lerp(Player.transform.position, PlayerPoses[curDir].position, Time.deltaTime * 15f);
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
}
