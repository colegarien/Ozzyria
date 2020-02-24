using System;

namespace Ozzyria.Networking.Model
{
    public enum MessageType
    {
        NONE,
        HAS_SLOT, // server can accept client
        CLIENT_JOIN, // client ask to join server
        SERVER_JOIN, // server confirms client join
        CLIENT_LEAVE, // client is disconnecting
        SERVER_LEAVE, // server is disconnecting (shutting down probably)
        SERVER_REJECT, // server rejects client connection
        CHAT // sending chat message
    }

    class Packet
    {
        private const char _separator = '|';

        public MessageType MessageType { get; set; }
        public string Data { get; set; }

        public string Serialize()
        {
            return  (Enum.GetName(typeof(MessageType), MessageType) + _separator + Data.Replace(_separator, ' ')).Trim();
        }

        public static Packet Deserialize(string data)
        {
            if(data.Trim() == "" || !data.Contains(_separator))
            {
                return new Packet { MessageType = MessageType.NONE, Data = "" };
            }

            var type = data.Substring(0, data.IndexOf(_separator)).Trim();
            var message = data.Substring(data.IndexOf(_separator) + 1).Trim();

            return new Packet
            {
                MessageType = Enum.Parse<MessageType>(type),
                Data = message
            };
        }



    }
}
