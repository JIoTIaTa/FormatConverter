﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using EnumAtributes;
using Ninject;
using Observer.Reader;
using Observer.Writer;
using ObserverReaderWriter.IoC;
using ObserverReaderWriter.Reader;
using ObserverReaderWriter.StreamProcessing;
using ObserverReaderWriter.Writer;
using Serialization;

namespace ObserverReaderWriter
{
    public partial class Form1 : Form
    {
        private IKernel iocConvertorKernel;
        private IKernel iocWriterKernel;
        private ConvertorParams _convertorParams;
        private string userDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private string formSettingsFileName = "parameters.dat";
        private string formSettingsDirectoryName = "Format Converter";
        private bool isHandleWork = false;

        //test
        private BaseFileReader fileReader;
        private IHandler fileHandler;
        private BaseFileWriter fileWriter;
        

        public Form1(ConvertorParams convertorParams)
        {
            InitializeComponent();

            _convertorParams = convertorParams ?? throw new ArgumentException("serialazeble Parametrs load ERROR");


            #region Десеріалізація
            foreach (FileExtention value in Enum.GetValues(typeof(FileExtention)))
            {
                comboBox_writeExt.Items.Add(value);
            }
            foreach (HandlerType value in Enum.GetValues(typeof(HandlerType)))
            {
                toolStripComboBox1.Items.Add(value);
            }
            textBox_Read.Text = _convertorParams.ReadFilePath;
            textBox_Write.Text = _convertorParams.WriteFilePath;
            textBox_readExt.Text = _convertorParams.ReadFileExtention.ToString();
            comboBox_writeExt.SelectedItem = _convertorParams.WriteFileExtention;
            Size = _convertorParams.WindowSize;
            Location = _convertorParams.WindowLocationPoint;
            toolStripComboBox1.SelectedItem = _convertorParams.HandlerType;
            #endregion

        }

        private void button_Read_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            string formatsToFilter = "";
            foreach (FileExtention value in Enum.GetValues(typeof(FileExtention)))
            {
                formatsToFilter += $"{value.GetDescription()} ({value.GetFileExtension()})|*{value.GetFileExtension()}|";
            }
            formatsToFilter = formatsToFilter.Remove(formatsToFilter.Length - 1);
            openFileDialog.Filter = formatsToFilter;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
               changeInterfaceToNewFile(openFileDialog.FileName);
            }
        }

        private void button_Convert_Click(object sender, EventArgs e)
        {
            iocConvertorKernel = new StandardKernel(new NinjectConverterConfig(_convertorParams));

            ConvetrerWorker convetrerWorker = iocConvertorKernel.Get<ConvetrerWorker>();
            convetrerWorker.ConvertProgress += ReadingProgress;
            convetrerWorker.HandlerSucces += ConvetrerOnHandlerSucces;
           
            convetrerWorker.Convert();
        }

        private void ConvetrerOnHandlerSucces(bool obj)
        {
            isHandleWork = obj;
        }

        private void ReadingProgress(long progress, long length)
        {
            
            toolStripProgressBar1.Maximum = (int)length;
            toolStripProgressBar1.Value = (int) progress;
            if (progress == length)
            {
                pictureBox1.Image = (isHandleWork) ? imageList1.Images[0] : imageList1.Images[1];
            }
        }

        /// <summary>
        /// Запис в файл з конфігами
        /// </summary>
        private void writeConfigFile()
        {
            _convertorParams.ReadFilePath = textBox_Read.Text;
            _convertorParams.WriteFilePath = textBox_Write.Text;
            _convertorParams.WriteFileExtention = (FileExtention)comboBox_writeExt.SelectedItem;
            _convertorParams.WindowSize = Size;
            _convertorParams.WindowLocationPoint = Location;
            _convertorParams.HandlerType = (HandlerType)toolStripComboBox1.SelectedItem;
            string formSettingsFullPath = Path.Combine(userDocumentsPath, formSettingsDirectoryName, formSettingsFileName);
            Serializator.Write(_convertorParams, formSettingsFullPath);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            writeConfigFile();
        }

        private void button_Write_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            string formatsToFilter = "";
            foreach (FileExtention value in Enum.GetValues(typeof(FileExtention)))
            {
                if (value.ToString() == comboBox_writeExt.SelectedItem.ToString())
                {
                    formatsToFilter = $"{value.GetDescription()} ({value.GetFileExtension()})|*{value.GetFileExtension()}";
                }
            }
            saveFileDialog.Filter = formatsToFilter;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox_Write.Text = $"{Path.GetDirectoryName(saveFileDialog.FileName)}\\{Path.GetFileNameWithoutExtension(saveFileDialog.FileName)}"; ;
            }
        }

        private void changeInterfaceToNewFile(string fullFileNameWithExtention)
        {
            string fileFormat = Path.GetExtension(fullFileNameWithExtention);
            _convertorParams.ReadFilePath = textBox_Read.Text = $"{Path.GetDirectoryName(fullFileNameWithExtention)}\\{Path.GetFileNameWithoutExtension(fullFileNameWithExtention)}";
            foreach (FileExtention value in Enum.GetValues(typeof(FileExtention)))
            {
                if (value.GetFileExtension() == fileFormat)
                {
                    _convertorParams.ReadFileExtention = value;
                    break;
                }
                _convertorParams.ReadFileExtention = FileExtention.uknown;
            }
            textBox_readExt.Text = _convertorParams.ReadFileExtention.ToString();
        }

        private void comboBox_writeExt_SelectedIndexChanged(object sender, EventArgs e)
        {
            _convertorParams.WriteFileExtention = (FileExtention)comboBox_writeExt.SelectedItem;
        }

        private void textBox_Read_DragDrop(object sender, DragEventArgs e)
        {
            string[] str = (string[])e.Data.GetData(DataFormats.FileDrop);
            changeInterfaceToNewFile(str[0]);
        }

        private void textBox_Read_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _convertorParams.HandlerType = (HandlerType)toolStripComboBox1.SelectedItem;
        }

        private void textBox_Write_TextChanged(object sender, EventArgs e)
        {
            _convertorParams.WriteFilePath = textBox_Write.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ToolTip tip1 = new ToolTip();
            tip1.AutoPopDelay = 10000;
            tip1.InitialDelay = 1000;
            tip1.ReshowDelay = 500;
            tip1.ShowAlways = true;
            tip1.SetToolTip(pictureBox1, "The success of the processing algorithm");
        }
    }
}
