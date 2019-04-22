using System;

namespace Observer.Reader
{
    [Serializable]
    internal class SigFileReader : BaseFileReader
    {
        #region Constructors and Destructors

        public SigFileReader(string fileName)
            : base(fileName)
        {
        }
        public override event Action<long, long> ReadProgress;
        #endregion

        #region Public Methods and Operators

        public override byte[] ReadNextBuff()
        {
            var low1 = (ushort)this.fileStream.ReadByte();
            var high1 = (ushort)this.fileStream.ReadByte();
            int count = low1 + (high1 << 8);
            var outData = new byte[count];
            int i = this.fileStream.Read(outData, 0, count);
            if (i <= 0)
            {
                fileLengthReaded = 0;
                ReadProgress?.Invoke(fileLength, fileLength);
                return null;
            }

            var temp = new byte[outData.Length - 2];
            Buffer.BlockCopy(outData, 2, temp, 0, temp.Length);
            fileLengthReaded += outData.Length;
            ReadProgress?.Invoke(fileLengthReaded, fileLength);
            return temp;
        }

        #endregion
    }
}