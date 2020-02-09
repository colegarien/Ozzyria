namespace Ozzyria.Server
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var connectionListener = new Networking.ConnectionListener();
            await connectionListener.Start();
        }
    }
}
