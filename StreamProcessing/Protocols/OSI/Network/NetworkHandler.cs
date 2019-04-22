using System;
using Protocols.OSI.Network;

namespace ObserverReaderWriter.StreamProcessing.Protocols.OSI.Network
{
    class NetworkHandler : IHandler
    {
        public event Action<object> ProcessingInfo;

        public byte[][] Process(byte[] inData)
        {
            byte[][] result = null;
            using (SearchIpPacket searchIp = new SearchIpPacket() {IsBuffer = true, CheckChecksum = true})
            {
                //var temp = searchIp.Shearch(inData);
                //result = temp.ToArray();
                result = searchIp.Shearch(inData).ToArray();
            }
            return (result.Length != 0)? result : null;
        }
    }
}
