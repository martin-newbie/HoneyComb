using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class LoginManager : MonoBehaviour
{

    private void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    void Login()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {

                }
                else
                {

                }
            });
        }
    }
}