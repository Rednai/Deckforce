using System;
using UnityEngine;
using PlayFab;

public class Authentification : MonoBehaviour
{
    public static Authentification instance;

    public bool isAuthenticated { get; private set; } = false;
    public UserInfo userInfo = new UserInfo();

    [HideInInspector]
    public string sessionTicket;

    private Action<PlayFab.ClientModels.LoginResult> loginSuccessCallback;
    private Action<PlayFabError> loginErrorCallback;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Login(Action<PlayFab.ClientModels.LoginResult> successCallback = null, Action<PlayFabError> errorCallback = null)
    {
        this.loginSuccessCallback = successCallback;
        this.loginErrorCallback = errorCallback;

        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                LoginCustomID();
                break;
            case RuntimePlatform.IPhonePlayer:
                LoginIOS();
                break;
            case RuntimePlatform.Android:
                LoginAndroid();
                break;
            default:
                break;
        }
    }

    private void LoginCustomID()
    {
        string id;

        if (PlayerPrefs.HasKey("CustomId"))
            id = PlayerPrefs.GetString("CustomId");
        else
            id = System.Guid.NewGuid().ToString();

        PlayFabClientAPI.LoginWithCustomID(new PlayFab.ClientModels.LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = true
        },
        result => {
            PlayerPrefs.SetString("CustomId", id);
            LoginSuccess(result);
        }, LoginError);
    }

    private void LoginIOS()
    {
        PlayFabClientAPI.LoginWithIOSDeviceID(new PlayFab.ClientModels.LoginWithIOSDeviceIDRequest
        {
            DeviceId = SystemInfo.deviceUniqueIdentifier,
            DeviceModel = SystemInfo.deviceModel,
            OS = SystemInfo.operatingSystem,
            CreateAccount = true
        }, LoginSuccess, LoginError);
    }

    private void LoginAndroid()
    {
        PlayFabClientAPI.LoginWithAndroidDeviceID(new PlayFab.ClientModels.LoginWithAndroidDeviceIDRequest
        {
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            AndroidDevice = SystemInfo.deviceModel,
            OS = SystemInfo.operatingSystem,
            CreateAccount = true,
        }, LoginSuccess, LoginError);
    }

    private void LoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        isAuthenticated = true;
        sessionTicket = result.SessionTicket;
        userInfo.playFabId = result.PlayFabId;
        userInfo.entityId = result.EntityToken.Entity.Id;
        loginSuccessCallback?.Invoke(result);
    }

    private void LoginError(PlayFabError error)
    {
        loginErrorCallback?.Invoke(error);
    }
}
