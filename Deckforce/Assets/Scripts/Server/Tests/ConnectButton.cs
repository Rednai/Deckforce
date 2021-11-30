using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    public Button disconnectButton;
    public Button connectButton;
    public Text text;

    public void OnConnect()
    {
        GameServer.instance.connect("127.0.0.1", 56100, ConnectedToServer);
    }

    private void ConnectedToServer()
    {
        text.text = "Connected to game server";
        connectButton.interactable = false;
        disconnectButton.interactable = true;
    }
}
