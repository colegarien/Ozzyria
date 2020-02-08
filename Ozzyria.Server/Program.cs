namespace Ozzyria.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionListener = new Networking.ConnectionListener();
            connectionListener.Run();
        }
    }
}
