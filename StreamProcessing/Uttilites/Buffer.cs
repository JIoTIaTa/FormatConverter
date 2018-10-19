namespace Spark.Decoders.Utills
{
    /// <summary>
    ///     Buffer for binary stream.
    /// </summary>
    public sealed class Buffer
    {
        #region Fields

        private byte[] dataPacket = new byte[0];

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Add data to buffer.
        /// </summary>
        /// <param name="data">Data for add.</param>
        public void AddData(byte[] data)
        {
            var tempData = new byte[data.Length + this.dataPacket.Length];
            System.Buffer.BlockCopy(this.dataPacket, 0, tempData, 0, this.dataPacket.Length);
            System.Buffer.BlockCopy(data, 0, tempData, this.dataPacket.Length, data.Length);
            this.dataPacket = tempData;
        }

        /// <summary>
        ///     Gets data from buffer.
        /// </summary>
        /// <returns>Data.</returns>
        public byte[] GetData()
        {
            byte[] result = this.dataPacket;
            this.dataPacket = new byte[0];
            return result;
        }

        /// <summary>
        /// Gets length of buffer.
        /// </summary>
        public int Length
        {
            get
            {
                return this.dataPacket.Length;
            }
        }

        public override string ToString()
        {
            return string.Format("Length: {0}", this.Length);
        }

        #endregion
    }
}