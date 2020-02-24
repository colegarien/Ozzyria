using Ozzyria.Networking.Model;
using System;
using System.IO;
using System.Text;

namespace Ozzyria.Networking
{
    class PacketFactory
    {
        private const char packetTerminator = '\0';
        public const int BUFFER_SIZE = 1024;

        public static Packet ReadPacket(Stream stream)
        {
            var data = "";
            byte[] buffer = new byte[BUFFER_SIZE];
            while (!data.Contains(packetTerminator))
            {
                stream.Read(buffer, 0, BUFFER_SIZE);
                data += Encoding.UTF8.GetString(buffer);

                // TODO something slightly smarter to avoid MASSIVE packets and timing things
            }

            return Packet.Deserialize(data.TrimEnd(packetTerminator));
        }

        public static void WritePacket(Stream stream, Packet packet)
        {
            var serializedPacket = packet.Serialize() + packetTerminator;
            byte[] data = Encoding.UTF8.GetBytes(serializedPacket);

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

    }
}
