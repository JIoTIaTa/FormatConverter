using System;

namespace Protocols.OSI.Network
{
    public sealed class PPPmlp
    {
#region Fields

        private readonly byte controlField;

        private readonly byte firmwareType;

        private readonly byte fraqtype;

        private readonly byte[] indata;

        private readonly short? nextProtocol;

        private readonly byte[] payloadData;

        private readonly short protocolType;

        private readonly int seqNumber;

#endregion

#region Constructors and Destructors

        public PPPmlp(byte[] indata)
        {
            if (indata != null && indata.Length > 10)
            {
                this.indata = indata;
                this.firmwareType = indata[0];
                this.controlField = indata[1];

                int temp = 0;
                temp += indata[2] << 8;
                temp += indata[3];
                this.protocolType = (short)temp;

                temp = 0;
                temp += indata[5] << 16;
                temp += indata[6] << 8;
                temp += indata[7];
                this.seqNumber = temp;

                this.fraqtype = (byte)(this.indata[4] >> 6);

                if (this.fraqtype > 1)
                {
                    temp = 0;
                    temp += indata[8] << 8;
                    temp += indata[9];
                    this.nextProtocol = (short)temp;
                    this.payloadData = new byte[indata.Length - 10];
                    Array.Copy(indata, 10, this.payloadData, 0, this.payloadData.Length);
                }
                else
                {
                    this.nextProtocol = null;
                    this.payloadData = new byte[indata.Length - 8];
                    Array.Copy(indata, 8, this.payloadData, 0, this.payloadData.Length);
                }
            }
        }

#endregion

#region Public Properties

        public byte ControlField
        {
            get
            {
                return this.controlField;
            }
        }

        /// <summary>
        ///     Gets Protocol Type
        /// </summary>
        /// <returns>
        ///     protocol type 255 (FFh)
        /// </returns>
        public byte FirwareType
        {
            get
            {
                return this.firmwareType;
            }
        }

        public byte FragmentType
        {
            get
            {
                return this.fraqtype;
            }
        }

        public short? NextProtocol
        {
            get
            {
                return this.nextProtocol;
            }
        }

        public byte[] PayloadData
        {
            get
            {
                return this.payloadData;
            }
        }

        public short ProtocolType
        {
            get
            {
                return this.protocolType;
            }
        }

        public int SequenseNumber
        {
            get
            {
                return this.seqNumber;
            }
        }

#endregion
    }
}