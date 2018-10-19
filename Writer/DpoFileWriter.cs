using System;

namespace Observer.Writer
{
    [Serializable]
    internal sealed class DpoFileWriter : BaseFileWriter
    {
        #region Constructors and Destructors

        public DpoFileWriter(string fileName)
            : base(fileName)
        {
        }

        #endregion

        #region Methods

        protected override byte[] PrepareDataToWrite(byte[] data)
        {
            var low = (byte)data.Length;
            var hight = (byte)(data.Length >> 8);
            var dpo = new byte[data.Length + 2];
            dpo[0] = low;
            dpo[1] = hight;
            for (int i = 2; i < dpo.Length; i++)
            {
                dpo[i] = data[i - 2];
            }

            return dpo;
        }

        #endregion
    }
}