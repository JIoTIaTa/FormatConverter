using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnumAtributes;
using Ninject.Modules;
using Observer.Reader;
using Observer.Writer;
using ObserverReaderWriter.Reader;
using ObserverReaderWriter.StreamProcessing;
using ObserverReaderWriter.StreamProcessing.Protocols.OSI.Network;
using Serialization;

namespace ObserverReaderWriter.IoC
{
    class NinjectLoadConfig : NinjectModule
    {
        private string userDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private string formSettingsFileName = "parameters.dat";
        private string formSettingsDirectoryName = "Format Converter";
        private string fullFilePathWithExtentions = null;

        public NinjectLoadConfig()
        {
            
        }

        public NinjectLoadConfig(string fullFilePathWithExtentions)
        {
            this.fullFilePathWithExtentions = fullFilePathWithExtentions;
        }
        public override void Load()
        {
            Bind<ConvertorParams>().ToMethod(context => readCofigFile());
            Bind<Form1>().ToSelf();
        }
        private ConvertorParams readCofigFile()
        {
            string formSettingsFullPath = Path.Combine(userDocumentsPath, formSettingsDirectoryName, formSettingsFileName);
            ConvertorParams convertorParams;
            if (Serializator.Read<ConvertorParams>(formSettingsFullPath) != null)
            {
                convertorParams = Serializator.Read<ConvertorParams>(formSettingsFullPath);
            }
            else
            {
                convertorParams =  new ConvertorParams();
            }

            if (fullFilePathWithExtentions != null)
            {
                string fileFormat = Path.GetExtension(fullFilePathWithExtentions);
                convertorParams.ReadFilePath = $"{Path.GetDirectoryName(fullFilePathWithExtentions)}\\{Path.GetFileNameWithoutExtension(fullFilePathWithExtentions)}";
                foreach (FileExtention value in Enum.GetValues(typeof(FileExtention)))
                {
                    if (value.GetFileExtension() == fileFormat)
                    {
                        convertorParams.ReadFileExtention = value;
                        break;
                    }
                    convertorParams.ReadFileExtention = FileExtention.uknown;
                }
            }
            return convertorParams;
        }
    }

    class NinjectReaderConfig : NinjectModule
    {
        private string _readFilePath;
        private FileExtention _readFileExtention = FileExtention.uknown;
        private string _writeFilePath;
        private FileExtention _writeFileExtention = FileExtention.uknown;
        private HandlerType _handlerType = HandlerType.none;

        public NinjectReaderConfig(ConvertorParams convertorParams)
        {
            _readFileExtention = convertorParams.ReadFileExtention;
            _readFilePath = convertorParams.ReadFilePath;

            _writeFileExtention = convertorParams.WriteFileExtention;
            _writeFilePath = convertorParams.WriteFilePath;

            _handlerType = convertorParams.HandlerType;
        }
        public override void Load()
        {
            Bind<BaseFileReader>().ToMethod(context => readerType(_readFileExtention, _readFilePath));
            Bind<BaseFileWriter>().ToMethod(context => writerType(_writeFileExtention, _writeFilePath));
            Bind<IHandler>().ToMethod(context => handlerType(_handlerType));
        }
        private BaseFileReader readerType(FileExtention fileExtention, string filePath)
        {
            string fullFileNameWithExtentions = $"{filePath}{fileExtention.GetFileExtension()}";
            switch (fileExtention)
            {
                case FileExtention.dat:
                    return new BaseFileReader(fullFileNameWithExtentions);
                case FileExtention.dpo:
                    return new DpoFileReader(fullFileNameWithExtentions);
                case FileExtention.ipf:
                    return new IpfReader(fullFileNameWithExtentions);
                case FileExtention.pcap:
                    return new PcapReader(fullFileNameWithExtentions);
                case FileExtention.sig:
                    return new SigFileReader(fullFileNameWithExtentions);
                default:
                    return new BaseFileReader(fullFileNameWithExtentions);

            }
        }
        private BaseFileWriter writerType(FileExtention fileExtention, string FilePath)
        {
            switch (fileExtention)
            {
                case FileExtention.dat:
                    return new BaseFileWriter(FilePath);
                case FileExtention.dpo:
                    return new DpoFileWriter(FilePath);
                case FileExtention.ipf:
                    return new IpfFileWriter(FilePath);
                case FileExtention.pcap:
                    return new PcapWriter(FilePath,0);
                case FileExtention.sig:
                    return new SigFileWriter(FilePath);
                default:
                    return new BaseFileWriter(FilePath);

            }
        }

        private IHandler handlerType(HandlerType handlerType)
        {
            switch (handlerType)
            {
                case HandlerType.none:
                    return new HandlerNoProcessing();
                case HandlerType.seachIP:
                    return new NetworkHandler();
                default:
                    return new HandlerNoProcessing();
            }
        }
    }
}
