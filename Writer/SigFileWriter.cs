using System;

namespace Observer.Writer
{
    [Serializable]
    internal sealed class SigFileWriter : BaseFileWriter
    {
        #region Fields

        private readonly byte[] ethernetIipp = { 0x00, 0x1D, 0xA1, 0x6F, 0x6A, 0x98, 0x00, 0x25, 0x45, 0x3E, 0xDF, 0xC2, 0x08, 0x00 };
        #endregion

        #region Constructors and Destructors

        public SigFileWriter(string fileName)
            : base(fileName)
        {
        }

        #endregion

        #region Methods

        protected override byte[] PrepareDataToWrite(byte[] data)
        {
            int newlength = data.Length + 14 + 2;
            var low = (byte)newlength;
            var hight = (byte)(newlength >> 8);
            var sig = new byte[newlength];
            sig[0] = low;
            sig[1] = hight;
            for (int i = 0; i < this.ethernetIipp.Length; i++)
            {
                sig[i + 4] = this.ethernetIipp[i];
            }

            for (int i = 18; i < sig.Length; i++)
            {
                sig[i] = data[i - 18];
            }

            return sig;
        }

        #endregion
    }
}