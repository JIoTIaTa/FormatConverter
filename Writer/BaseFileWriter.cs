using System;
using System.IO;
using ObserverReaderWriter.Writer;

namespace Observer.Writer
{
    public class BaseFileWriter : IDisposable, IWriter
    {
        #region Fields

        protected BufferedStream BufferedOutput;

        protected string Extension;

        protected bool FileClosed;

        protected FileStream OutputStream;

        protected string TempName;

        public virtual event Action<long> WriteProgress;


        #endregion

        #region Constructors and Destructors

        public BaseFileWriter(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                this.CreateNewFileToWrite(fileName);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Close file, streams connected with and release using resources
        /// </summary>
        public virtual void CloseWriting()
        {
            if (!this.FileClosed && this.OutputStream != null && this.BufferedOutput != null)
            {
                if (this.BufferedOutput.CanWrite)
                {
                    this.BufferedOutput.Flush();
                }

                this.BufferedOutput.Close();
                this.BufferedOutput.Dispose();
                this.OutputStream.Close();
                this.OutputStream.Dispose();
                if (!string.Equals(this.TempName, this.TempName + this.Extension))
                {
                    do
                    {
                        if (!File.Exists(this.TempName))
                        {
                            break;
                        }
                    }
                    while (!this.TryToMoveFile(this.TempName, this.TempName + this.Extension));
                }

                this.FileClosed = true;
            }
        }

        /// <summary>
        ///     Close last recorded file and create new parameters for writing a new file
        /// </summary>
        /// <param name="fileName">set file name here</param>
        public void CreateNewFileToWrite(string fileName)
        {
            if (this.OutputStream != null && this.OutputStream.CanWrite && this.BufferedOutput != null)
            {
                this.CloseWriting();
            }

            this.FileClosed = false;
            this.Extension = Path.GetExtension(fileName);
            var tt = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(tt))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }

            this.TempName = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName));
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            this.OutputStream = new FileStream(
                this.TempName,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite);
            this.BufferedOutput = new BufferedStream(this.OutputStream);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        ///     Simple operations used to write data in file
        /// </summary>
        /// <param name="data">Data</param>
        public virtual void WriteToFile(byte[] data)
        {
            byte[] preparedData = this.PrepareDataToWrite(data);
            if (!this.FileClosed && this.BufferedOutput != null && this.BufferedOutput.CanWrite)
            {
                this.BufferedOutput.Write(preparedData, 0, preparedData.Length);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Prepare data according to special formats, what used in the child classes
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Data to write</returns>
        protected virtual byte[] PrepareDataToWrite(byte[] data)
        {
            return data;
        }

        protected virtual bool TryToMoveFile(string targetFile, string newFile)
        {
            try
            {
                if (File.Exists(targetFile))
                {
                    File.Move(targetFile, newFile);
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }

            return false;
        }

        private void Dispose(bool flag)
        {
            if (flag)
            {
                if (this.OutputStream != null)
                {
                    this.OutputStream.Dispose();
                }

                if (this.BufferedOutput != null)
                {
                    this.BufferedOutput.Dispose();
                }
            }
        }

        #endregion
    }
}