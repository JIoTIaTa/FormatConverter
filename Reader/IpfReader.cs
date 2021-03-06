﻿using System;

namespace Observer.Reader
{
    [Serializable]
    internal class IpfReader : BaseFileReader
    {
        #region Constants

        private const int LengthHeader = 4;

        #endregion

        #region Fields

        private byte[] bytePacket;
        public override event Action<long, long> ReadProgress;

        #endregion

        #region Constructors and Destructors

        public IpfReader(string fileName)
            : base(fileName)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override byte[] ReadNextBuff()
        {
            this.bytePacket = new byte[LengthHeader];
            this.fileStream.Read(this.bytePacket, 0, LengthHeader);
            byte[] packet = this.GetPacketData();
            fileLengthReaded += packet.Length;
            ReadProgress?.Invoke(fileLengthReaded, fileLength);
            return packet;
        }

        #endregion

        #region Methods

        private byte[] GetPacketData()
        {
            int length = this.fileStream.ReadByte();
            length += this.fileStream.ReadByte() << 8;
            length += this.fileStream.ReadByte() << 16;
            if (length < 0)
            {
                return null;
            }

            int tagbyte = this.fileStream.ReadByte();
            this.bytePacket = new byte[length];
            this.fileStream.Read(this.bytePacket, 0, length);

            return tagbyte == 0 ? this.bytePacket : new byte[0];
        }

        #endregion
    }
}