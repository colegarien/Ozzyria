using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzyria.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Networking.Client();
            client.Start();
        }
    }
}
