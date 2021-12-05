using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendDataButton : MonoBehaviour
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
