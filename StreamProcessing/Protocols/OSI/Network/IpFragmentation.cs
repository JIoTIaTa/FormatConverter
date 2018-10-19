using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Protocols.OSI.Network
{
    /// <summary>
    ///     Class for fragmentation ip. Uisng sigleton pattern.
    /// </summary>
    public sealed class IpFragmentation
    {
#region Fields

        private readonly List<Ip> ips = new List<Ip>();

        private int fragmentedIppacket;

#endregion

#region Public Methods and Operators

        /// <summary>
        /// Fragmentation packet in byte mode.
        /// </summary>
        /// <param name="ippacket">
        /// The ippacket.
        /// </param>
        /// <param name="shearchIp">
        /// The shearch Ip.
        /// </param>
        /// <returns>
        /// Fragmentation packet in byte array.
        /// </returns>
        public byte[] Processing(byte[] ippacket, bool shearchIp = true)
        {
            var ipPacket = new Ipv4(ippacket);

            if (ipPacket.IsCorrect())
            {
                if (!this.IsExist(ipPacket.Indentification))
                {
                    if (ipPacket.Fragmentation)
                    {
                        this.AddIp(ipPacket);
                        return null;
                    }

                    return ipPacket.IpData;
                }

                if (this.IsExist(ipPacket.Indentification))
                {
                    Ip selected = this.SelectIp(ipPacket.Indentification);
                    if (ipPacket.Fragmentation)
                    {
                        selected.Data.AddRange(ipPacket.Payload);
                        return null;
                    }

                    if (!ipPacket.Fragmentation)
                    {
                        selected.Data.AddRange(ipPacket.IpData);

                        var result = new Ipv4(this.GenerateIp(selected));

                        this.fragmentedIppacket += 1;

                        Debug.WriteLine("Fragmetation ip packet");

                        return result.IpData;
                    }
                }
            }

            return null;
        }

        public override string ToString()
        {
            return this.fragmentedIppacket.ToString();
        }

#endregion

#region Methods

        private void AddIp(Ipv4 packet)
        {
            var newList = new Ip
                              {
                                  Indentifivation = packet.Indentification,
                                  IpSource = packet.Source,
                                  IpDestination = packet.Destination,
                                  Data = new List<byte>(packet.Payload),
                                  NextProtocol = packet.NextProtocol
                              };

            this.ips.Add(newList);
        }

        private byte[] GenerateIp(Ip select)
        {
            Ip conection = select;
            var result = new byte[conection.Data.Count + 20];
            result[0] = 0x45;
            result[1] = 0x00;
            result[2] = (byte)(result.Length / 256);
            result[3] = (byte)(result.Length - (result[2] * 256));
            result[4] = conection.Indentifivation[0];
            result[5] = conection.Indentifivation[1];
            result[6] = 64;
            result[7] = 0;
            result[8] = 0x39;
            result[9] = conection.NextProtocol;
            Array.Copy(conection.IpSource, 0, result, 12, 4);
            Array.Copy(conection.IpDestination, 0, result, 16, 4);
            byte[] checkSum = Ipv4.CheckSum(result);
            result[10] = checkSum[0];
            result[11] = checkSum[1];
            Array.Copy(select.Data.ToArray(), 0, result, 20, result.Length - 20);
            this.ips.Remove(select);
            return result;
        }

        private bool IsExist(byte[] indentification)
        {
            IEnumerable<Ip> result = from inden in this.ips
                                     where
                                         inden.Indentifivation[0] == indentification[0]
                                         && inden.Indentifivation[1] == indentification[1]
                                     select inden;
            return result.Count() == 1;
        }

        private Ip SelectIp(byte[] indentification)
        {
            IEnumerable<Ip> result = from inden in this.ips
                                     where
                                         inden.Indentifivation[0] == indentification[0]
                                         && inden.Indentifivation[1] == indentification[1]
                                     select inden;
            return result.ToArray()[0];
        }

#endregion
    }

    public sealed class Ip
    {
#region Fields

        public List<byte> Data;

        public byte[] Indentifivation;

        public byte[] IpDestination;

        public byte[] IpSource;

        public byte NextProtocol;

#endregion
    }
}