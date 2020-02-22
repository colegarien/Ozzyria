namespace Ozzyria.Server
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var server = new Networking.Server();
            await server.StartListening();
        }
    }
}
