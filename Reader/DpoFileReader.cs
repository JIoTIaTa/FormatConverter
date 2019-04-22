using System;
using ObserverReaderWriter.Reader;

namespace Observer.Reader
{
    [Serializable]
    internal class DpoFileReader : BaseFileReader
    {
        #region Constructors and Destructors
        public DpoFileReader(string fileName)
            : base(fileName)
        {
        }

        public override event Action<long, long> ReadProgress;
        public override event Action<byte[]> ReadedDataAsync;

        #endregion

        #region Public Methods and Operators

       

        public override byte[] ReadNextBuff()
        {
            int low = this.fileStream.ReadByte();
            int high = this.fileStream.ReadByte();
            int count = low + (high << 8);
            if (count < 0)
            {
                fileLengthReaded = 0;
                ReadProgress?.Invoke(fileLength, fileLength);
                return null;
            }

            var outData = new byte[count];
            this.fileStream.Read(outData, 0, count);
            fileLengthReaded += outData.Length;
            ReadProgress?.Invoke(fileLengthReaded,fileLength);
            return outData;
        }

        public override async void ReadNextBuffAsync()
        {
            int low = this.fileStream.ReadByte();
            int high = this.fileStream.ReadByte();
            int count = low + (high << 8);
            if (count < 0)
            {
                fileLengthReaded = 0;
                ReadProgress?.Invoke(fileLength, fileLength);
                ReadedDataAsync?.Invoke(null);
            }
            else
            {
                var outData = new byte[count];
                await this.fileStream.ReadAsync(outData, 0, count);
                fileLengthReaded += outData.Length;
                ReadProgress?.Invoke(fileLengthReaded, fileLength);
                ReadedDataAsync?.Invoke(outData);
            }

        }
        #endregion
    }
}