using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public void Login()
    {
        SceneLoadManager.Instance.LoadScene("TitleScene");
    }
}