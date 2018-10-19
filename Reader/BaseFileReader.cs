using System;
using System.IO;
using System.Threading.Tasks;
using ObserverReaderWriter.Reader;

namespace Observer.Reader
{
    [Serializable]
    internal class BaseFileReader : IDisposable, IReader
    {
        #region Fields

        public volatile bool CloseFile;

        [NonSerialized]
        protected FileStream FileStream;

        [NonSerialized]
        protected long fileLength = 0;
        [NonSerialized]
        protected long fileLengthReaded = 0;

        public virtual event Action<long, long> ReadProgress;
        public virtual event Action<byte[]> ReadedDataAsync;

        #endregion

        #region Constructors and Destructors

        public BaseFileReader(string fileName, int bufferLength = 512)
        {
            this.StartNewFile(fileName);
            this.BufferLength = bufferLength;
        }

        public BaseFileReader()
        {
        }

        #endregion

        #region Public Properties

        public int BufferLength { get; set; }

        #endregion

        #region Public Methods and Operators

        public void CloseReading()
        {
            if (this.FileStream != null)
            {
                this.FileStream.Close();
                this.FileStream.Dispose();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public virtual byte[] ReadNextBuff()
        {
            var outData = new byte[this.BufferLength];
            if (this.FileStream.CanRead && !this.CloseFile)
            {
                int i = this.FileStream.Read(outData, 0, this.BufferLength);
                if (i != this.BufferLength)
                {
                    if (i <= 0)
                    {
                        fileLengthReaded = 0;
                        return null;
                    }

                    var tempBuff = new byte[i];
                    for (int j = 0; j < i; j++)
                    {
                        tempBuff[j] = outData[j];
                    }
                    fileLengthReaded += tempBuff.Length;
                    ReadProgress.Invoke(fileLengthReaded, this.fileLength);
                    return tempBuff;
                }
            }

            return outData;
        }
        public virtual async void  ReadNextBuffAsync()
        {
            var outData = new byte[FileStream.Length];
            if (this.FileStream.CanRead && !this.CloseFile)
            {
                await this.FileStream.ReadAsync(outData, 0, this.BufferLength);
               
                fileLengthReaded += outData.Length;
                ReadProgress.Invoke(fileLengthReaded, this.fileLength);
                ReadedDataAsync?.Invoke(outData);
            }
        }

        public virtual void StartNewFile(string fileName)
        {
            if (this.FileStream != null)
            {
                this.CloseReading();
            }

            try
            {
                this.FileStream = new FileStream(fileName, FileMode.Open);
                this.CloseFile = false;
                fileLength = FileStream.Length;
            }
            catch (Exception exception)
            {

            }
        }

        #endregion

        #region Methods

        private void Dispose(bool flag)
        {
            if (flag)
            {
                if (this.FileStream != null)
                {
                    this.FileStream.Dispose();
                }
            }
        }

        #endregion
    }
}