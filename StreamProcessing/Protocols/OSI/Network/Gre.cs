using System;

namespace Protocols.OSI.Network
{
    /// <summary>
    ///     Generic Routing Encapsulation (GRE) is a tunneling protocol developed by Cisco Systems that can encapsulate a wide
    ///     variety of seachIP layer protocols inside virtual point-to-point links over an Internet Protocol internetwork.
    /// </summary>
    public sealed class Gre
    {
#region Fields

        private byte[] Data;

#endregion

#region Public Properties

        public bool ChecksumBit
        {
            get
            {
                return (this.Data[0] & 128) == 128;
            }
        }

        public int Flags
        {
            get
            {
                int result = this.Data[1] >> 3;
                return result;
            }
        }

        public bool KeyBit
        {
            get
            {
                return (this.Data[0] & 32) == 32;
            }
        }

        public byte[] Payload
        {
            get
            {
                if (this.Protocol == Protocol.IP)
                {
                    if (this.Data.Length < 4)
                    {
                        return null;
                    }

                    var result = new byte[this.Data.Length - 4];
                    Array.Copy(this.Data, 4, result, 0, result.Length);
                    return result;
                }

                return this.Data;
            }
        }

        public Protocol Protocol
        {
            get
            {
                int result = this.Data[2] << 8;
                result += this.Data[3];

                switch (result)
                {
                    case 0x0800:
                        return Protocol.IP;
                    case 0x880b:
                        return Protocol.PPP;
                }

                return Protocol.NONE;
            }
        }

        public int RecursionControl
        {
            get
            {
                int result = 0;

                if ((this.Data[0] & 4) == 4)
                {
                    result += 4;
                }

                if ((this.Data[0] & 2) == 2)
                {
                    result += 2;
                }

                if ((this.Data[0] & 1) == 1)
                {
                    result += 1;
                }

                return result;
            }
        }

        public bool RoutingBit
        {
            get
            {
                return (this.Data[0] & 64) == 64;
            }
        }

        public bool SequenceNumberBit
        {
            get
            {
                return (this.Data[0] & 16) == 16;
            }
        }

        public bool StrictSourceBit
        {
            get
            {
                return (this.Data[0] & 8) == 8;
            }
        }

        public int Versions
        {
            get
            {
                int result = 0;

                if ((this.Data[0] & 4) == 4)
                {
                    result += 4;
                }

                if ((this.Data[0] & 2) == 2)
                {
                    result += 2;
                }

                if ((this.Data[0] & 1) == 1)
                {
                    result += 1;
                }

                return result;
            }
        }

#endregion

#region Public Methods and Operators

        public byte[] Processing(Ipv4 ip4)
        {
            if (ip4.NextProtocol != 0x2f)
            {
                return ip4.IpData;
            }

            this.Data = ip4.Payload;
            return this.Payload;
        }

        public override string ToString()
        {
            return string.Format("PayloadType: {0} DataLength {1}", this.Protocol, this.Payload.Length);
        }

#endregion
    }

    public enum Protocol
    {
        IP,

        PPP,

        NONE
    }
}