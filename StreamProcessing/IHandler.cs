
using System;

namespace ObserverReaderWriter.StreamProcessing
{
    interface IHandler
    {
        byte[][] Process(byte[] inData);

        event Action<object> ProcessingInfo;
    }
}
