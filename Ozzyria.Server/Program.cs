namespace Ozzyria.Server
{
    class Program
    {

        static void Main(string[] args)
        {
            var server = new Networking.Server();
            server.Start(null);
        }
    }
}
