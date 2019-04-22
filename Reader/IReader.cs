using System;

namespace ObserverReaderWriter.Reader
{
    interface IReader
    {
        /// <summary>
        /// Процес читання файлу, прочитано, загальна довжина
        /// </summary>
        event Action<long, long> ReadProgress;
        /// <summary>
        /// Івент завершення асинхронного зчитування
        /// </summary>
        event Action<byte[]> ReadedDataAsync;

        void ReadNextBuffAsync();
    }
}
