using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButtonTest : MonoBehaviour
{
    public Button connectButton;
    public Button disconnectButton;
    public Button sendDataButton;
    public Button stopServer;
    public Text text;

    public void OnConnect()
    {
        GameServer.instance.Connect("127.0.0.1", 56100, ConnectedToServer);
    }

    private void ConnectedToServer()
    {
        text.text = "Connected to game server";
        connectButton.interactable = false;
        disconnectButton.interactable = true;
        sendDataButton.interactable = true;
        stopServer.interactable = true;
    }
}
