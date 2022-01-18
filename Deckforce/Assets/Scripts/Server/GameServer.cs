using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using Telepathy;

public class GameServer : MonoBehaviour
{
    public static GameServer instance;

    public bool isConnected { get { return client.Connected; } }

    private Client client = new Client(Settings.maxMessageSize);
    private List<PlayerJoin> connectedPlayers = new List<PlayerJoin>();
    private bool isWaitingForPlayers = false;
    private Action<List<PlayerJoin>> OnAllPlayersConnected;
    private int numberOfPlayersToWait;
    public bool isOffline;

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
        ArraySegment<byte> data = Serialize(objectToSend);
        client.Send(data);
    }

    private void HandleDataReceived(ArraySegment<Byte> data)
    {
        object obj = Deserialize(data);
        Parser.instance.ParseData(obj);
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

    public void WaitForOtherPlayers(int numberOfPlayersToWait, Action<List<PlayerJoin>> OnAllPlayersConnected)
    {
        connectedPlayers.Clear();
        isWaitingForPlayers = true;
        this.OnAllPlayersConnected = OnAllPlayersConnected;
        this.numberOfPlayersToWait = numberOfPlayersToWait;
        PlayerJoin playerJoin = new PlayerJoin() {
            id = Authentification.instance.userInfo.playFabId,
            team = Authentification.instance.userInfo.currentTeam,
            username = Authentification.instance.userInfo.username
        };
        SendData(playerJoin);
        playerJoin.isClient = true;
        connectedPlayers.Add(playerJoin);
    }

    public void OnPlayerJoin(PlayerJoin playerJoin)
    {
        PlayerJoin alreadyConnected = connectedPlayers.Find(connectedPlayer => connectedPlayer.id == playerJoin.id);

        if (alreadyConnected != null || !isWaitingForPlayers)
            return;

        connectedPlayers.Add(playerJoin);
        SendData(new PlayerJoin() {
            id = Authentification.instance.userInfo.playFabId,
            team = Authentification.instance.userInfo.currentTeam,
            username = Authentification.instance.userInfo.username
        });
        if (connectedPlayers.Count == numberOfPlayersToWait)
        {
            isWaitingForPlayers = false;
            OnAllPlayersConnected?.Invoke(connectedPlayers);
        }
    }

    public void StopServer()
    {
        client.Send(new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes("Stop")));
        client.Disconnect();
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
