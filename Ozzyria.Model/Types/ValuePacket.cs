namespace Ozzyria.Model.Types
{
    public class ValuePacket : Dictionary<string, string>
    {
        public const string DIG_OPERATOR = "::";

        public ValuePacket Clone()
        {
            var cloned = new ValuePacket();
            foreach(var kv in this)
            {
                cloned[kv.Key] = kv.Value;
            }

            return cloned;
        }

        public ValuePacket Extract(string locator)
        {
            var result = new ValuePacket();
            foreach (var kv in this)
            {
                if (kv.Key.StartsWith(locator + DIG_OPERATOR))
                {
                    // +DIG_OPERATOR.Length to skip over dig-syntax
                    result[kv.Key.Substring(locator.Length + DIG_OPERATOR.Length)] = kv.Value;
                }
            }

            return result;
        }

        public bool HasValueFor(string locator)
        {
            return this.Any(kv => kv.Key == locator || kv.Key.StartsWith(locator + DIG_OPERATOR));
        }

        public static ValuePacket Combine(params ValuePacket[] packets)
        {
            var result = new ValuePacket();
            var allKeys = packets.Where(p => p != null).SelectMany(p => p.Keys).Distinct();
            foreach (var key in allKeys)
            {
                foreach (var packet in packets)
                {
                    if (packet != null && packet.ContainsKey(key))
                        result[key] = packet[key];
                }
            }

            return result;
        }
    }
}
