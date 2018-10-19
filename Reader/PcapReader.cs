using System;

namespace Observer.Reader
{
    /// <summary>
    ///     Pcap Reader. Using Pcap format file.
    /// </summary>
    [Serializable]
    internal class PcapReader : BaseFileReader
    {
        #region Constants

        private const int LengthHeader = 24;

        #endregion

        #region Fields

        private bool start;

        private byte[] bytePacket;
        public override event Action<long, long> ReadProgress;

        #endregion

        #region Constructors and Destructors

        public PcapReader(string fileName)
            : base(fileName)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override byte[] ReadNextBuff()
        {
            if (!this.start)
            {
                this.bytePacket = new byte[LengthHeader];
                this.FileStream.Read(this.bytePacket, 0, LengthHeader);
                this.start = true;
            }

            int length = this.GetLengthPacket();
            if (length == 0)
            {
                fileLengthReaded = 0;
                ReadProgress.Invoke(fileLength, fileLength);
                return null;
            }

            var packet = new byte[length];
            this.FileStream.Read(packet, 0, length);
            var data = new byte[length - 16];
            System.Buffer.BlockCopy(packet, 16, data, 0, data.Length);
            fileLengthReaded += packet.Length;
            ReadProgress.Invoke(fileLengthReaded, fileLength);
            return packet;
        }

        #endregion

        #region Methods

        private int GetLengthPacket()
        {
            var headerPcap = new byte[16];
            this.FileStream.Read(headerPcap, 0, 16);
            int length = headerPcap[8];
            length += headerPcap[9] << 8;
            return length;
        }

        #endregion
    }
}