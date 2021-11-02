using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;

public class Authentification : MonoBehaviour
{
    private void Login()
    {
    }

    private void Register()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                RegisterPC();
                break;
            case RuntimePlatform.IPhonePlayer:
                RegisterIOS();
                break;
            case RuntimePlatform.Android:
                RegisterAndroid();
                break;
            default:
                break;
        }
    }

    private void RegisterPC()
    {
        System.Guid id = System.Guid.NewGuid();
        PlayFabClientAPI.LoginWithCustomID(new PlayFab.ClientModels.LoginWithCustomIDRequest
        {
            CustomId = id.ToString(),
            CreateAccount = true
        }, LoginSuccess, LoginError);
    }

    private void RegisterIOS()
    {
        PlayFabClientAPI.LoginWithIOSDeviceID(new PlayFab.ClientModels.LoginWithIOSDeviceIDRequest
        {
            DeviceId = SystemInfo.deviceUniqueIdentifier,
            DeviceModel = SystemInfo.deviceModel,
            OS = SystemInfo.operatingSystem,
            CreateAccount = true
        }, LoginSuccess, LoginError);
    }

    private void RegisterAndroid()
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
    }

    private void LoginError(PlayFabError error)
    {
    }
}
