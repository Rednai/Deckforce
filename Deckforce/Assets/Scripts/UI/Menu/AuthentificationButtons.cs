using System.Collections;
using System.Collections.Generic;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;

public class AuthentificationButtons : MonoBehaviour
{
    public Text errorText;
    public GameObject matchButtons;
    public InputField usernameInput;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Username"))
            usernameInput.text = PlayerPrefs.GetString("Username");
    }

    public void OnLogin()
    {
        if (usernameInput.text == "")
        {
            errorText.text = "Please enter an username.";
            errorText.gameObject.SetActive(true);
            return;
        }
        errorText.gameObject.SetActive(false);
        PlayerPrefs.SetString("Username", usernameInput.text);
        Authentification.instance.userInfo.username = usernameInput.text;

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
