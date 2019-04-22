using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverReaderWriter.StreamProcessing
{
    class HandlerNoProcessing : IHandler
    {
        public event Action<object> ProcessingInfo;

        public byte[][] Process(byte[] inData)
        {
            byte[][] result = new byte[1][];
            result[0] = inData;
            return result;
        }
    }
}
