using System;
using System.Timers;
using System.Collections.Generic;
using Microsoft.Playfab.Gaming.GSDK.CSharp;
using System.Threading.Tasks;

namespace DeckforceServer
{
    class Server
    {
        private Telepathy.Server server = new Telepathy.Server(Settings.maxMessageSize);
        private bool isRunning = false;
        private List<Player> players = new List<Player>();
        private List<ConnectedPlayer> connectedPlayers = new List<ConnectedPlayer>();
        private bool firstTimeoutCheck = false;

        /// <summary>
        /// Start the server
        /// </summary>
        public void Start()
        {
            Telepathy.Log.Info = GameserverSDK.LogMessage;
            Telepathy.Log.Warning = GameserverSDK.LogMessage;
            Telepathy.Log.Error = GameserverSDK.LogMessage;

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
        private void OnShutdown()
        {
            Stop();
        }

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
        public async void Stop()
        {
            isRunning = false;
            // TODO : Trouver mieux (Débugger server.Stop())
            //server.Stop();
            GameserverSDK.LogMessage("Server will stop in 5 seconds...");
            await Task.Delay(TimeSpan.FromSeconds(5));
            Environment.Exit(0);
        }

        /// <summary>
        /// Run the server loop
        /// </summary>
        private void Loop()
        {
            // We stop the server if no one is connected after 30 seconds
            Timer timer = new Timer(20000);
            timer.Elapsed += CheckTimeout;
            timer.AutoReset = true;
            timer.Enabled = true;

            while (isRunning) server.Tick(Settings.tick);
        }

        /// <summary>
        /// Check if the server timeout
        /// </summary>
        void CheckTimeout(Object source, ElapsedEventArgs e) {
            GameserverSDK.LogMessage("Timeout check, player connected count : " + connectedPlayers.Count);

            if (connectedPlayers.Count == 0)
            {
                if (firstTimeoutCheck)
                {
                    GameserverSDK.LogMessage("Second timeout check passed.");
                    Stop();
                }
                else
                {
                    GameserverSDK.LogMessage("First timeout check passed.");
                    firstTimeoutCheck = true;
                }
                return;
            }
            firstTimeoutCheck = false;
        }

        /// <summary>
        /// Function called when a client connects to the server
        /// </summary>
        /// <param name="connectionId">Connection id of the client</param>
        private void HandleConnection(int connectionId)
        {
            string ip = server.GetClientAddress(connectionId);
            Player player = players.Find(player => player.connectionId == connectionId); // players.Find(player => player.ip == ip); TODO: remettre avec l'ip en prod !

            if (player != null)
            {
                player.connectionId = connectionId;
                player.isConnected = true;
                GameserverSDK.LogMessage("Player reconnected (tag = " + player.tag + ")");
            } else
            {
                player = new Player {
                    connectionId = connectionId,
                    isConnected = true,
                    ip = ip
                };
                players.Add(player);
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
            {
                player.isConnected = false;
                GameserverSDK.LogMessage("Player disconnected (tag = " + player.tag + ")");
            }
            UpdateSdkConnectedPlayers();
        }

        /// <summary>
        /// Function called when a client send data to the server
        /// </summary>
        /// <param name="connectionId">Connection id of the data sender</param>
        /// <param name="data">Data received</param>
        private void HandleDataReceived(int connectionId, ArraySegment<Byte> data)
        {
            GameserverSDK.LogMessage("Data received from : " + connectionId);

            // Check if the game is stopped
            if (CheckStopRequest(data)) return;

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

        /// <summary>
        /// Check if the data received is a stop request
        /// </summary>
        /// <param name="data">Received data</param>
        /// <returns>Return true if the data is a stop request, return false in the opposite case</returns>
        private bool CheckStopRequest(ArraySegment<Byte> data)
        {
            try
            {
                if (System.Text.Encoding.UTF8.GetString(data.Array, 0, 4) == "Stop")
                {
                    Stop();
                    return true;
                }
            }
            catch {}
            return false;
        }
    }
}
