using System;
using System.Net.Sockets;
using System.Text;

namespace Ozzyria.Networking
{
    class PacketBuilder
    {
        public const int BUFFER_SIZE = 1024;

        const string packetStart = "#PACKET#";
        const string packetEnd = "#END_PACKET#";
        const string headerStart = "#HEADER#";
        const string headerEnd = "#END_HEADER#";
        const string bodyStart = "#BODY#";
        const string bodyEnd = "#END_BODY#";

        public static byte[] Build(string message)
        {
            string header = "";
            return Encode($"{packetStart}{headerStart}{header}{headerEnd}{bodyStart}{message}{bodyEnd}{packetEnd}");
        }

        public static string ReadPacket(NetworkStream stream)
        {
            var packet = "";
            byte[] buffer = new byte[BUFFER_SIZE];
            while (!packet.Contains(packetEnd))
            {
                stream.Read(buffer, 0, BUFFER_SIZE);
                packet += Encode(buffer);

                // TODO something slightly smarter to avoid MASSIVE packets and timing things
            }

            var headerStartIndex = packet.IndexOf(headerStart) + headerStart.Length;
            var headerEndIndex = packet.IndexOf(headerEnd);
            var header = packet.Substring(headerStartIndex, headerEndIndex - headerStartIndex).Trim();

            var bodyStartIndex = packet.IndexOf(bodyStart) + bodyStart.Length;
            var bodyEndIndex = packet.IndexOf(bodyEnd);
            var body = packet.Substring(bodyStartIndex, bodyEndIndex - bodyStartIndex).Trim();

            return body;
        }

        public static void WritePacket(NetworkStream stream, string message)
        {
            byte[] data = Build(message);
            var dataLength = data.Length;
            var numberOfChunks = Math.Ceiling((float)dataLength / (float)BUFFER_SIZE);
            for (var i = 0; i < numberOfChunks; i++)
            {
                var sliceStart = i * BUFFER_SIZE;
                var dataLeft = dataLength - sliceStart;
                var sliceLength = dataLeft >= BUFFER_SIZE
                    ? BUFFER_SIZE
                    : dataLeft;

                var slice = data.Slice(sliceStart, sliceLength);
                stream.Write(slice, 0, sliceLength);
            }
        }


        private static string Encode(byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer);
        }

        private static byte[] Encode(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

    }
}
