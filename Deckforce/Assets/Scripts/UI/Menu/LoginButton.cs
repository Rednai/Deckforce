using System.Collections;
using System.Collections.Generic;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour
{
    public Text errorText;
    public GameObject matchButtons;

    public void OnLogin()
    {
        Authentification.instance.Login(loginSuccess, loginError);
    }

    private void loginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        gameObject.SetActive(false);
        matchButtons.SetActive(true);
    }

    private void loginError(PlayFabError error)
    {
        errorText.text = error.ErrorMessage;
        errorText.gameObject.SetActive(true);
    }
}
