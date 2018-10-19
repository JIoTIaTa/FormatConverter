using System;
using System.Collections.Generic;
using System.Text;

namespace Observer.Writer
{
    /// <summary>
    ///     Pcap format writer.
    /// </summary>
    [Serializable]
    public class PcapWriter : BaseFileWriter
    {
        #region Fields

        public List<byte> PcapHeaderAll = new List<byte>();

        private bool isFirst;

        private static DateTime olddate = new DateTime(1970, 1, 1, 2, 0, 1);
        #endregion

        #region Static Fields

        private List<byte> PcapHeaderSmall = new List<byte>
                                                                  {
                                                                      0xd4,
                                                                      0xc3,
                                                                      0xb2,
                                                                      0xa1,
                                                                      2,
                                                                      0,
                                                                      4,
                                                                      0,
                                                                      0,
                                                                      0,
                                                                      0,
                                                                      0,
                                                                      0,
                                                                      0,
                                                                      0,
                                                                      0,
                                                                      0xff,
                                                                      0xff,
                                                                      0,
                                                                      0
                                                                  };

        #endregion

        #region Constructors and Destructors

        public PcapWriter(string fileName, int mode)
            : base(fileName)
        {
            this.PcapHeaderAll.AddRange(this.PcapHeaderSmall);
            this.PcapHeaderAll.AddRange(BitConverter.GetBytes(1));
        }

        #endregion

        #region Methods

        protected override byte[] PrepareDataToWrite(byte[] data)
        {
            var resultData = new List<byte>();

            if (!this.isFirst)
            {
                resultData.AddRange(this.PcapHeaderAll);
                this.isFirst = true;
            }

            resultData.AddRange(GetPacketData(data));
            return resultData.ToArray();
        }

        private static IEnumerable<byte> GetPacketData(byte[] data)
        {
            if (data == null)
            {
                return new byte[0];
            }

            TimeSpan subtime = DateTime.Now.Subtract(olddate);
            byte[] secondsmas = BitConverter.GetBytes((int)subtime.TotalSeconds);
            var result = new byte[data.Length + 16 + 14];
            int length = data.Length + 14;
            byte[] byteLength = BitConverter.GetBytes(length);
            Array.Copy(byteLength, 0, result, 8, 2);
            Array.Copy(byteLength, 0, result, 12, 2);
            
            //real time 4 bytes from 1970 im seconds
            result[0] = secondsmas[0];
            result[1] = secondsmas[1];
            result[2] = secondsmas[2];
            result[3] = secondsmas[3];
            
            result[16] = 0x00;
            result[17] = 0xbc;
            result[18] = 0xaa;
            result[19] = 0xdd;
            result[20] = 0xcc;
            result[21] = 0xaa;

            result[22] = 0x00;
            result[23] = 0xbc;
            result[24] = 0xaa;
            result[25] = 0xff;
            result[26] = 0xee;
            result[27] = 0xad;

            result[28] = 0x08;
            result[29] = 0x00;

            Array.Copy(data, 0, result, 30, data.Length);
            
            return result;
        }

        #endregion
    }
}