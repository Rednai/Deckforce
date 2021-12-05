using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Telepathy;

public class GameServer : MonoBehaviour
{
    public static GameServer instance;

    public bool isConnected { get { return client.Connected; } }

    private Client client = new Client(Settings.maxMessageSize);

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

        Log.Info = Debug.Log;
        Log.Warning = Debug.LogWarning;
        Log.Error = Debug.LogError;
    }

    private void Start()
    {
        client.OnData = HandleDataReceived;
    }

    public void Connect(string serverIp, int serverPort, Action onConnected = null)
    {
        if (onConnected != null)
            client.OnConnected = onConnected;
        client.Connect(serverIp, serverPort);
    }

    public void Disconnect(Action onDisconnected = null)
    {
        if (onDisconnected != null)
            client.OnDisconnected = onDisconnected;
        client.Disconnect();
    }

    public void SendData(object objectToSend)
    {
        Debug.Log("Sending data...");
        ArraySegment<byte> data = Serialize(objectToSend);
        client.Send(data);
    }

    private void HandleDataReceived(ArraySegment<Byte> data)
    {
        Debug.Log("Data received !");

        object obj = Deserialize(data);

        // TODO: A supprimer
        if (obj is SendDataButton.Message)
            Debug.Log((obj as SendDataButton.Message).text);
    }

    private ArraySegment<Byte> Serialize(object anySerializableObject)
    {
        using (var memoryStream = new MemoryStream())
        {
            // TODO: gestion d'erreurs
            (new BinaryFormatter()).Serialize(memoryStream, anySerializableObject);
            return new ArraySegment<byte>(memoryStream.ToArray());
        }
    }

    private object Deserialize(ArraySegment<byte> data)
    {
        // TODO: gestion d'erreurs
        using (var memoryStream = new MemoryStream(data.Array))
            return (new BinaryFormatter()).Deserialize(memoryStream);
    }

    private void Update()
    {
        client.Tick(Settings.tick);
    }

    void OnApplicationQuit()
    {
        client.Disconnect();
    }
}
