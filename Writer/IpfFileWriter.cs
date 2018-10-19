using System;
using System.Collections.Generic;

namespace Observer.Writer
{
    [Serializable]
    internal class IpfFileWriter : BaseFileWriter
    {
        #region Static Fields

        private static readonly List<byte> I3MSyns = new List<byte> { 112, 115, 51, 73 };

        #endregion

        #region Constructors and Destructors

        public IpfFileWriter(string fileName)
            : base(fileName)
        {
        }

        #endregion

        #region Methods

        protected override byte[] PrepareDataToWrite(byte[] data)
        {
            var resultData = new List<byte>();
            resultData.AddRange(I3MSyns);
            resultData.AddRange(this.GetPacketData(data));

            return resultData.ToArray();
        }

        private IEnumerable<byte> GetPacketData(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            var result = new byte[data.Length + 4];
            int length = data.Length;
            byte[] byteLength = BitConverter.GetBytes(length);
            Array.Copy(byteLength, 0, result, 0, 4);
            Array.Copy(data, 0, result, 4, data.Length);
            return result;
        }

        #endregion
    }
}