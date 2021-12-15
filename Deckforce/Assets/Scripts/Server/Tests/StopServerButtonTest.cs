using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopServerButtonTest : MonoBehaviour
{
    public Button connectButton;
    public Button disconnectButton;
    public Button sendDataButton;
    public Button stopServer;

    public void OnStopServer()
    {
        GameServer.instance.StopServer();

        connectButton.interactable = true;
        disconnectButton.interactable = false;
        sendDataButton.interactable = false;
        stopServer.interactable = false;
    }
}
