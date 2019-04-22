using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Observer.Reader;
using Observer.Writer;
using ObserverReaderWriter.StreamProcessing;

namespace ObserverReaderWriter
{
    internal class ConvetrerWorker : IDisposable
    {
        private BaseFileReader fileReader;
        private IHandler fileHandler;
        private BaseFileWriter fileWriter;

        /// <summary>
        /// 1 - конвертовано даних, 2 - Загальна кількість даних
        /// </summary>
        public  event  Action<long, long> ConvertProgress;
        /// <summary>
        /// Вибраний алгоритм обробки видає результат
        /// </summary>
        public event Action<bool> HandlerSucces;
        public ConvetrerWorker(BaseFileReader fileReader, IHandler fileHandler, BaseFileWriter fileWriter)
        {
            this.fileReader = fileReader ?? throw new ArgumentException("fileReader load ERROR");
            this.fileHandler = fileHandler ?? throw new ArgumentException("fileHandler load ERROR");
            this.fileWriter = fileWriter ?? throw new ArgumentException("fileWriter load ERROR");

            fileReader.ReadProgress += ReadingProgress;
            fileReader.ReadedDataAsync += FileReaderOnReadedDataAsync;
            fileWriter.BufferWrited += FileWriterOnBufferWrited;
        }

        public void TestConvert()
        {
            Task converTask = Task.Run(() =>
            {
                Convert();
            });
        }
        public void Convert()
        {
            byte[] readedData;
            do
            {
                if (!fileReader.closeFile)
                {
                    readedData = fileReader.ReadNextBuff();
                    if (readedData != null)
                    {
                        byte[][] processedData = fileHandler.Process(readedData);
                        if (processedData != null)
                        {
                            HandlerSucces?.Invoke(true);
                            foreach (byte[] frame in processedData)
                            {
                                fileWriter.WriteToFile(frame);
                            }
                        }
                        else
                        {
                            HandlerSucces?.Invoke(false);
                            fileWriter.WriteToFile(readedData);
                        }
                    }
                }
                else
                {
                    fileReader.CloseReading();
                    fileWriter.CloseWriting();
                    readedData = null;
                }
            } while (readedData != null);
            fileReader.CloseReading();
            fileWriter.CloseWriting();
        }

        public void ConvertAsync()
        {
            fileReader.ReadNextBuffAsync();
        }

        private void FileWriterOnBufferWrited()
        {
            fileReader.ReadNextBuffAsync();
        }


        private void ReadingProgress(long arg1, long arg2)
        {
            ConvertProgress?.Invoke(arg1,arg2);
        }



        private void FileReaderOnReadedDataAsync(byte[] readedData)
        {
            if (readedData != null)
            {
                byte[][] processedData = null;
                Task handlerTask = Task.Run(() =>
                {
                    processedData = fileHandler.Process(readedData);
                });
                handlerTask.Wait();
                if (processedData != null)
                {
                    HandlerSucces?.Invoke(true);
                    foreach (byte[] frame in processedData)
                    {
                        fileWriter.WriteToFileAsync(frame);
                    }
                }
                else
                {
                    HandlerSucces?.Invoke(false);
                    fileWriter.WriteToFileAsync(readedData);
                }
            }
            else
            {
                fileReader.CloseReading();
                fileWriter.CloseWriting();
            }
        }

        public void Dispose()
        {
            fileReader?.CloseReading();
            fileWriter?.CloseWriting();
        }
    }
}
