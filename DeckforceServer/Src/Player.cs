using System;

namespace DeckforceServer
{
    class Player
    {
        public string tag = Guid.NewGuid().ToString();
        public int connectionId = 0;
        public string ip = "";
        public bool isConnected = false;
    }
}
