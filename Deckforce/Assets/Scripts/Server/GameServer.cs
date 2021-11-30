using System;
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

    public void connect(string serverIp, int serverPort, Action onConnected = null)
    {
        if (onConnected != null)
            client.OnConnected = onConnected;
        client.Connect(serverIp, serverPort);
    }

    public void disconnect(Action onDisconnected = null)
    {
        if (onDisconnected != null)
            client.OnDisconnected = onDisconnected;
        client.Disconnect();
    }

    private void HandleDataReceived(ArraySegment<Byte> data)
    {
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
