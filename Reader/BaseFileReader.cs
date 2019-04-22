using System;
using System.IO;
using System.Threading.Tasks;
using ObserverReaderWriter.Reader;

namespace Observer.Reader
{
    [Serializable]
    internal class BaseFileReader : IDisposable
    {
        #region Fields

        public volatile bool closeFile;

        [NonSerialized]
        protected FileStream fileStream;

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
            this.bufferLength = bufferLength;
        }

        public BaseFileReader()
        {
        }

        #endregion

        #region Public Properties

        public int bufferLength { get; set; }

        #endregion

        #region Public Methods and Operators

        public void CloseReading()
        {
            if (this.fileStream != null)
            {
                this.fileStream.Close();
                this.fileStream.Dispose();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public virtual byte[] ReadNextBuff()
        {
            var outData = new byte[bufferLength];
            int readedLength = 0;
            if (fileStream.CanRead && !closeFile)
            {
                readedLength = fileStream.Read(outData, 0, bufferLength);
                if (readedLength < bufferLength)
                {
                    fileLengthReaded = 0;
                    ReadProgress?.Invoke(fileLength, fileLength);
                    Array.Resize(ref outData, readedLength);
                    return (readedLength != 0) ? outData : null;
                }
                fileLengthReaded += outData.Length;
                ReadProgress?.Invoke(fileLengthReaded, this.fileLength);
            }
            return (readedLength != 0) ? outData : null;
        }
        public virtual async void  ReadNextBuffAsync()
        {
            var outData = new byte[fileStream.Length];
            if (this.fileStream.CanRead && !this.closeFile){
                await this.fileStream.ReadAsync(outData, 0, this.bufferLength); 
                fileLengthReaded += outData.Length;
                ReadProgress?.Invoke(fileLengthReaded, this.fileLength);
                ReadedDataAsync?.Invoke(outData);
            }
        }

        public virtual void StartNewFile(string fileName)
        {
            if (this.fileStream != null)
            {
                this.CloseReading();
            }

            try
            {
                this.fileStream = new FileStream(fileName, FileMode.Open);
                this.closeFile = false;
                fileLength = fileStream.Length;
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
                if (this.fileStream != null)
                {
                    this.fileStream.Dispose();
                }
            }
        }
        #endregion
    }
}