using System;

namespace Protocols.OSI.Network
{
    public sealed class Ipv4
    {
#region Fields

        private readonly byte[] ipData;

#endregion

#region Constructors and Destructors

        public Ipv4(byte[] paket)
        {
            this.ipData = paket;
        }

#endregion

#region Public Properties

        /// <summary>
        ///     Gets the destination.
        /// </summary>
        /// <value>
        ///     The destination.
        /// </value>
        public byte[] Destination
        {
            get
            {
                var adress = new byte[4];
                Buffer.BlockCopy(this.ipData, 16, adress, 0, 4);
                return adress;
            }
        }

        /// <summary>
        ///     Gets IP adress in int presentation
        /// </summary>
        public int DestinationInt
        {
            get
            {
                return BitConverter.ToInt32(this.Destination, 0);
            }
        }

        /// <summary>
        ///     Gets the destination string.
        /// </summary>
        /// <value>
        ///     The destination string.
        /// </value>
        public string DestinationString
        {
            get
            {
                byte[] ips = this.Destination;
                return string.Format("{0}.{1}.{2}.{3}", ips[0], ips[1], ips[2], ips[3]);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="Ipv4" /> is fragmentation.
        /// </summary>
        /// <value>
        ///     <c>true</c> if fragmentation; otherwise, <c>false</c>.
        /// </value>
        public bool Fragmentation
        {
            get
            {
                return (this.ipData[6] & 32) == 32;
            }
        }

        /// <summary>
        ///     Gets the length of the header.
        /// </summary>
        /// <value>
        ///     The length of the header.
        /// </value>
        public int HeaderLength
        {
            get
            {
                switch (this.ipData[0] & 0x0F)
                {
                    case 6:
                        return 24;
                }

                return 20;
            }
        }

        /// <summary>
        ///     Gets the indentification of fragmentation.
        /// </summary>
        /// <value>
        ///     The indentification.
        /// </value>
        public byte[] Indentification
        {
            get
            {
                var result = new byte[2];
                Buffer.BlockCopy(this.ipData, 4, result, 0, result.Length);
                return result;
            }
        }

        public byte[] IpData
        {
            get
            {
                return this.ipData;
            }
        }

        /// <summary>
        ///     Gets Ip destination adress in int value.
        /// </summary>
        public int IpDestination
        {
            get
            {
                var ip = new byte[4];
                Array.ConstrainedCopy(this.Destination, 0, ip, 0, 4);
                Array.Reverse(ip);
                return BitConverter.ToInt32(ip, 0);
            }
        }

        /// <summary>
        ///     Gets Ip source adress in int value.
        /// </summary>
        public int IpSource
        {
            get
            {
                var ip = new byte[4];
                Array.ConstrainedCopy(this.Source, 0, ip, 0, 4);
                Array.Reverse(ip);
                return BitConverter.ToInt32(ip, 0);
            }
        }

        /// <summary>
        ///     Gets the length.
        /// </summary>
        /// <value>
        ///     The length.
        /// </value>
        public int Length
        {
            get
            {
                int result = this.ipData[2] << 8;
                result += this.ipData[3];
                return result;
            }
        }

        /// <summary>
        ///     Gets the length payload.
        /// </summary>
        /// <value>
        ///     The length payload.
        /// </value>
        public int LengthPayload
        {
            get
            {
                return this.Length - this.HeaderLength;
            }
        }

        /// <summary>
        ///     Gets the next protocol.
        /// </summary>
        /// <value>
        ///     The next protocol.
        /// </value>
        public byte NextProtocol
        {
            get
            {
                return this.ipData[0x09];
            }
        }

        /// <summary>
        ///     Gets the payload.
        /// </summary>
        /// <value>
        ///     The payload.
        /// </value>
        public byte[] Payload
        {
            get
            {
                var result = new byte[this.LengthPayload];
                Buffer.BlockCopy(this.ipData, this.HeaderLength, result, 0, this.LengthPayload);
                return result;
            }
        }

        /// <summary>
        ///     Gets the source.
        /// </summary>
        /// <value>
        ///     The source.
        /// </value>
        public byte[] Source
        {
            get
            {
                var adress = new byte[4];
                Buffer.BlockCopy(this.ipData, 12, adress, 0, 4);
                return adress;
            }
        }

        /// <summary>
        ///     Gets Source IP in int presentation.
        /// </summary>
        public int SourceInt
        {
            get
            {
                return BitConverter.ToInt32(this.Source, 0);
            }
        }

        /// <summary>
        ///     Gets the source string.
        /// </summary>
        /// <value>
        ///     The source string.
        /// </value>
        public string SourceString
        {
            get
            {
                byte[] ips = this.Source;
                return string.Format("{0}.{1}.{2}.{3}", ips[0], ips[1], ips[2], ips[3]);
            }
        }

#endregion

#region Public Methods and Operators

        /// <summary>
        ///     Calculate checksum of ip packect.
        /// </summary>
        /// <param name="header">Header of ip packet.</param>
        /// <returns>Crc checksum.</returns>
        public static byte[] CheckSum(byte[] header)
        {
            var result = new byte[2];
            ushort check = ComputeHeaderIpChecksum(header, 0, 20);
            result[0] = (byte)(check >> 8);
            result[1] = (byte)(check - (result[0] * 256));
            return result;
        }

        public static ushort ComputeHeaderIpChecksum(byte[] header, int start, int length)
        {
            ushort word16;
            long sum = 0;
            for (int i = start; i < (length + start); i += 2)
            {
                word16 = (ushort)(((header[i] << 8) & 0xFF00) + (header[i + 1] & 0xFF));
                sum += word16;
            }

            while ((sum >> 16) != 0)
            {
                sum = (sum & 0xFFFF) + (sum >> 16);
            }

            sum = ~sum;
            return (ushort)sum;
        }

        public static string GetIpString(int ipAdress)
        {
            byte[] ip = BitConverter.GetBytes(ipAdress);
            Array.Reverse(ip);
            return string.Format("{0}_{1}_{2}_{3}", ip[0], ip[1], ip[2], ip[3]);
        }

        /// <summary>
        ///     Determines whether this ipv4 is correct.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this ipv4 is correct; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCorrect()
        {
            if (this.ipData == null)
            {
                return false;
            }

            if (this.ipData.Length < 20)
            {
                return false;
            }

            if (this.HeaderLength <= 0)
            {
                return false;
            }

            if ((this.LengthPayload + this.HeaderLength) > this.IpData.Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "Ipv4 protocol: NextProtocol: {0} Source: {1} Destination {2}, CorrectCheckSum {3}",
                this.NextProtocol,
                this.SourceString,
                this.DestinationString,
                this.IsCorrect());
        }

#endregion
    }
}