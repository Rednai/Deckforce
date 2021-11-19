using System;
using System.Collections.Generic;
using Microsoft.Playfab.Gaming.GSDK.CSharp;

namespace DeckforceServer
{
    class Server
    {
        private Telepathy.Server server = new Telepathy.Server(Settings.maxMessageSize);
        private bool isRunning = false;
        private List<Player> players = new List<Player>();
        private List<ConnectedPlayer> connectedPlayers = new List<ConnectedPlayer>();

        /// <summary>
        /// Start the server
        /// </summary>
        public void Start()
        {
            server.OnConnected = HandleConnection;
            server.OnDisconnected = HandleDisconnection;
            server.OnData = HandleDataReceived;

            GameserverSDK.RegisterHealthCallback(OnHealth);
            GameserverSDK.RegisterMaintenanceCallback(OnMaintenance);
            GameserverSDK.RegisterShutdownCallback(OnShutdown);

            // We send the informations to Playfab that the server is ready for players
            while (!GameserverSDK.ReadyForPlayers()) {}

            isRunning = true;
            server.Start(Settings.port);
            Loop();
        }

        /// <summary>
        /// Handle function for healty calls
        /// </summary>
        private bool OnHealth()
        {
            return true;
        }

        /// <summary>
        /// Handle function for maintenance calls
        /// </summary>
        private void OnMaintenance(DateTimeOffset time) {}

        /// <summary>
        /// Handle function for shutdown calls
        /// </summary>
        private void OnShutdown() {}

        /// <summary>
        /// Update the connected players on the GameServerSDK
        /// </summary>
        private void UpdateSdkConnectedPlayers()
        {
            connectedPlayers.Clear();
            foreach (Player player in players)
            {
                if (!player.isConnected)
                    continue;
                connectedPlayers.Add(new ConnectedPlayer(player.tag));
            }
            GameserverSDK.UpdateConnectedPlayers(connectedPlayers);
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            server.Stop();
        }

        /// <summary>
        /// Run the server loop
        /// </summary>
        private void Loop()
        {
            while (isRunning)
            {
                server.Tick(Settings.tick);
            }
        }

        /// <summary>
        /// Function called when a client connects to the server
        /// </summary>
        /// <param name="connectionId">Connection id of the client</param>
        private void HandleConnection(int connectionId)
        {
            string ip = server.GetClientAddress(connectionId);
            Player player = players.Find(player => player.ip == ip);

            if (player != null)
            {
                player.connectionId = connectionId;
                player.isConnected = true;
                GameserverSDK.LogMessage("Player reconnected (tag = " + player.tag + ")");
            } else
            {
                players.Add(new Player
                {
                    connectionId = connectionId,
                    isConnected = true,
                    ip = ip
                });
                GameserverSDK.LogMessage("Player connected (tag = " + player.tag + ")");
            }
            UpdateSdkConnectedPlayers();
        }

        /// <summary>
        /// Function called when a client disconnects from the server
        /// </summary>
        /// <param name="connectionId">Connection id of the client</param>
        private void HandleDisconnection(int connectionId)
        {
            Player player = players.Find(player => player.connectionId == connectionId);
            if (player != null)
                player.isConnected = false;
            GameserverSDK.LogMessage("Player disconnected (tag = " + player.tag + ")");
        }

        /// <summary>
        /// Function called when a client send data to the server
        /// </summary>
        /// <param name="connectionId">Connection id of the data sender</param>
        /// <param name="data">Data received</param>
        private void HandleDataReceived(int connectionId, ArraySegment<Byte> data)
        {
            TransmitToOtherPlayers(connectionId, data);
        }

        /// <summary>
        /// Transmit the received data to all the players except the emitter
        /// </summary>
        /// <param name="connectionId">Connection id of the data sender</param>
        /// <param name="data">Data received</param>
        private void TransmitToOtherPlayers(int connectionId, ArraySegment<Byte> data)
        {
            foreach (Player player in players)
            {
                if (player.connectionId == connectionId)
                    continue;
                else if (player.isConnected)
                    server.Send(player.connectionId, data);
            }
        }
    }
}
