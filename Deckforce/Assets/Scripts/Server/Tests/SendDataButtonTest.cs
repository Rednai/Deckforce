using System;
using UnityEngine;

public class SendDataButtonTest : MonoBehaviour
{
    [SerializableAttribute]
    public class Message
    {
        public string text;
    }

    public void OnSendData()
    {
        Message message = new Message();
        message.text = "Test de transmission entre clients";
        GameServer.instance.SendData(message);
    }
}
