using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisconnectButton : MonoBehaviour
{
    public Button connectButton;
    public Button disconnectButton;

    public void OnDisconnect()
    {
        GameServer.instance.disconnect(DisconnectedFromServer);
    }

    private void DisconnectedFromServer()
    {
        connectButton.interactable = true;
        disconnectButton.interactable = false;
    }
}
