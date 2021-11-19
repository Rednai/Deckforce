using Microsoft.Playfab.Gaming.GSDK.CSharp;

namespace DeckforceServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Api call to alert Playfab that the server is starting
            GameserverSDK.Start();

            Server server = new Server();
            server.Start();
        }
    }
}
