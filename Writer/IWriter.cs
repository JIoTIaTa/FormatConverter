using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverReaderWriter.Writer
{
    interface IWriter
    {
        /// <summary>
        /// Процес запису файлу, записано даних
        /// </summary>
        event Action<long> WriteProgress;
    }
}
