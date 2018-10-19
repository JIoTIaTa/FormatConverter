using System;
using System.Collections.Generic;
using System.Linq;

namespace Protocols.OSI.Network
{
    /// <summary>
    ///     Shearch ip packets in binary data.
    /// </summary>
    public sealed class SearchIpPacket : IDisposable
    {
        private List<byte> buffer = new List<byte>();

        /// <summary>
        ///     Gets or sets a value indicating whether checksum is enable or disable.[check checksum].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [check checksum]; otherwise, <c>false</c>.
        /// </value>
        public bool CheckChecksum { get; set; }

        /// <summary>
        ///     Gets or sets the next protocol for shearching.
        /// </summary>
        /// <value>
        ///     The next protocol.
        /// </value>
        public byte[] NextProtocol { get; set; }

        /// <summary>
        ///     Gets or sets the start position for shearch ip packet.
        /// </summary>
        /// <value>
        ///     The start position.
        /// </value>
        public int StartPosition { get; set; }

        /// <summary>
        /// Gets or sets if buffer is use.
        /// </summary>
        public bool IsBuffer { get; set; }

        private Spark.Decoders.Utills.Buffer buff = new Spark.Decoders.Utills.Buffer();

        public List<byte[]> Shearch(byte[] indata)
        {

            if (IsBuffer)
            {
                var buffData = buff.GetData();
                var result = new byte[indata.Length + buffData.Length];
                System.Buffer.BlockCopy(buffData, 0, result, 0, buffData.Length);
                System.Buffer.BlockCopy(indata, 0,  result, buffData.Length, indata.Length);
                indata = result;
            }

            var resultPacket = new List<byte[]>();
            int index = StartPosition;
            while (index < indata.Length - 20)
            {
                if (indata[index] != 0x45)
                {
                    index += 1;
                    continue;
                }

                if (this.NextProtocol != null)
                {
                    if (!this.NextProtocol.Contains(indata[index + 9]))
                    {
                        index += 1;
                        continue;
                    }
                }

                if (this.CheckChecksum)
                {
                    if (Ipv4.ComputeHeaderIpChecksum(indata, index, 20) != 0)
                    {
                        index += 1;
                        continue;
                    }
                }

                int lengthPacket = indata[index + 2] << 8;
                lengthPacket += indata[index + 3];

                if (lengthPacket > 1600)
                {
                    index += 1;
                    continue;
                }

                if ((lengthPacket + index) > indata.Length)
                {
                    break;
                }

                var result = new byte[lengthPacket];

                Buffer.BlockCopy(indata, index, result, 0, result.Length);

                resultPacket.Add(result);

                index += 20;
            }

            if (this.IsBuffer)
            {
                var delta = new byte[indata.Length - index];
                System.Buffer.BlockCopy(indata, index, delta, 0, delta.Length);
                this.buff.AddData(delta);
            }

            return resultPacket;
        }

        public void Dispose()
        {
            this.buffer = null;
            this.buff = null;
        }
    }
}