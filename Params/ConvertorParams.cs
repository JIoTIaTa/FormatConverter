using EnumAtributes;
using System;
using System.Drawing;

namespace ObserverReaderWriter.Reader
{
    public enum FileExtention
    {
        [BaseAttribute.FileExtension("")]
        [BaseAttribute.Description("Всі формати")]
        uknown,
        [BaseAttribute.FileExtension(".dat")]
        [BaseAttribute.Description("Дані без форматування")]
        dat,
        [BaseAttribute.FileExtension(".dpo")]
        [BaseAttribute.Description("DPO структура")]
        dpo,
        [BaseAttribute.FileExtension(".sig")]
        [BaseAttribute.Description("SIG структура")]
        sig,
        [BaseAttribute.FileExtension(".pcap")]
        [BaseAttribute.Description("PCAD структура")]
        pcap,
        [BaseAttribute.FileExtension(".ipf")]
        [BaseAttribute.Description("IPF структура")]
        ipf
    }

    public enum HandlerType
    {
        [BaseAttribute.Description("Без обробки потоку")]
        none,
        [BaseAttribute.Description("Пошук IP пакетів")]
        seachIP
    }

    [Serializable]
    public class ConvertorParams
    {
        private string _readReadFilePath = "";
        private string _writeFilePath = "";
        private FileExtention _readReadFileExtention = FileExtention.uknown;
        private FileExtention _writeFileExtention = FileExtention.uknown;
        private Size windowSize = new Size(500,150);
        private Point windowLocation = Point.Empty;
        private HandlerType handlerType = HandlerType.none;


        public string ReadFilePath { get => _readReadFilePath; set => _readReadFilePath = value; }
        public string WriteFilePath { get => _writeFilePath; set => _writeFilePath = value; }
        public FileExtention ReadFileExtention { get => _readReadFileExtention; set => _readReadFileExtention = value; }
        public FileExtention WriteFileExtention { get => _writeFileExtention; set => _writeFileExtention = value; }
        public Size WindowSize { get => windowSize; set => windowSize = value; }
        public Point WindowLocationPoint { get => windowLocation; set => windowLocation = value; }
        public HandlerType HandlerType { get => handlerType; set => handlerType = value; }
    }
}
