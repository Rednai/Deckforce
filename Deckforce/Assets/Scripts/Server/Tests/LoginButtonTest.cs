using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;

public class LoginButtonTest : MonoBehaviour
{
    public Button loginButton;
    public Button matchButton;
    public Text text;

    public void OnLogin()
    {
        Authentification.instance.Login(loginSuccess, loginError);
    }

    private void loginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        text.text = "Connected to PlayFab";
        loginButton.interactable = false;
        matchButton.interactable = true;
    }

    private void loginError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
    }
}
