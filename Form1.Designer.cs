namespace ObserverReaderWriter
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_Read = new System.Windows.Forms.Button();
            this.textBox_Read = new System.Windows.Forms.TextBox();
            this.button_Convert = new System.Windows.Forms.Button();
            this.textBox_Write = new System.Windows.Forms.TextBox();
            this.button_Write = new System.Windows.Forms.Button();
            this.textBox_readExt = new System.Windows.Forms.TextBox();
            this.comboBox_writeExt = new System.Windows.Forms.ComboBox();
            this.toolStrip_convertProgress = new System.Windows.Forms.ToolStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStrip_convertProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_Read
            // 
            this.button_Read.Location = new System.Drawing.Point(13, 5);
            this.button_Read.Name = "button_Read";
            this.button_Read.Size = new System.Drawing.Size(75, 23);
            this.button_Read.TabIndex = 0;
            this.button_Read.Text = "Read";
            this.button_Read.UseVisualStyleBackColor = true;
            this.button_Read.Click += new System.EventHandler(this.button_Read_Click);
            // 
            // textBox_Read
            // 
            this.textBox_Read.AllowDrop = true;
            this.textBox_Read.Location = new System.Drawing.Point(94, 7);
            this.textBox_Read.Name = "textBox_Read";
            this.textBox_Read.ReadOnly = true;
            this.textBox_Read.Size = new System.Drawing.Size(315, 20);
            this.textBox_Read.TabIndex = 1;
            this.textBox_Read.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox_Read_DragDrop);
            this.textBox_Read.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBox_Read_DragEnter);
            // 
            // button_Convert
            // 
            this.button_Convert.Location = new System.Drawing.Point(13, 59);
            this.button_Convert.Name = "button_Convert";
            this.button_Convert.Size = new System.Drawing.Size(460, 23);
            this.button_Convert.TabIndex = 3;
            this.button_Convert.Text = "Convert";
            this.button_Convert.UseVisualStyleBackColor = true;
            this.button_Convert.Click += new System.EventHandler(this.button_Convert_Click);
            // 
            // textBox_Write
            // 
            this.textBox_Write.Location = new System.Drawing.Point(94, 33);
            this.textBox_Write.Name = "textBox_Write";
            this.textBox_Write.Size = new System.Drawing.Size(315, 20);
            this.textBox_Write.TabIndex = 5;
            this.textBox_Write.TextChanged += new System.EventHandler(this.textBox_Write_TextChanged);
            // 
            // button_Write
            // 
            this.button_Write.Location = new System.Drawing.Point(13, 31);
            this.button_Write.Name = "button_Write";
            this.button_Write.Size = new System.Drawing.Size(75, 23);
            this.button_Write.TabIndex = 4;
            this.button_Write.Text = "Write";
            this.button_Write.UseVisualStyleBackColor = true;
            this.button_Write.Click += new System.EventHandler(this.button_Write_Click);
            // 
            // textBox_readExt
            // 
            this.textBox_readExt.Location = new System.Drawing.Point(421, 8);
            this.textBox_readExt.Name = "textBox_readExt";
            this.textBox_readExt.ReadOnly = true;
            this.textBox_readExt.Size = new System.Drawing.Size(52, 20);
            this.textBox_readExt.TabIndex = 6;
            // 
            // comboBox_writeExt
            // 
            this.comboBox_writeExt.FormattingEnabled = true;
            this.comboBox_writeExt.Location = new System.Drawing.Point(421, 32);
            this.comboBox_writeExt.Name = "comboBox_writeExt";
            this.comboBox_writeExt.Size = new System.Drawing.Size(52, 21);
            this.comboBox_writeExt.TabIndex = 7;
            this.comboBox_writeExt.SelectedIndexChanged += new System.EventHandler(this.comboBox_writeExt_SelectedIndexChanged);
            // 
            // toolStrip_convertProgress
            // 
            this.toolStrip_convertProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip_convertProgress.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.toolStripComboBox1});
            this.toolStrip_convertProgress.Location = new System.Drawing.Point(0, 84);
            this.toolStrip_convertProgress.Name = "toolStrip_convertProgress";
            this.toolStrip_convertProgress.Size = new System.Drawing.Size(484, 27);
            this.toolStrip_convertProgress.TabIndex = 9;
            this.toolStrip_convertProgress.Text = "toolStrip2";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(320, 24);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(0, 24);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 27);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "icons8-checked-48.png");
            this.imageList1.Images.SetKeyName(1, "dead.png");
            // 
            // pictureBox1
            // 
            this.pictureBox1.InitialImage = global::ObserverReaderWriter.Properties.Resources.icons8_checked_48;
            this.pictureBox1.Location = new System.Drawing.Point(460, 84);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(484, 111);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.toolStrip_convertProgress);
            this.Controls.Add(this.comboBox_writeExt);
            this.Controls.Add(this.textBox_readExt);
            this.Controls.Add(this.textBox_Write);
            this.Controls.Add(this.button_Write);
            this.Controls.Add(this.button_Convert);
            this.Controls.Add(this.textBox_Read);
            this.Controls.Add(this.button_Read);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Format Convertor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip_convertProgress.ResumeLayout(false);
            this.toolStrip_convertProgress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_Read;
        private System.Windows.Forms.TextBox textBox_Read;
        private System.Windows.Forms.Button button_Convert;
        private System.Windows.Forms.TextBox textBox_Write;
        private System.Windows.Forms.Button button_Write;
        private System.Windows.Forms.TextBox textBox_readExt;
        private System.Windows.Forms.ComboBox comboBox_writeExt;
        private System.Windows.Forms.ToolStrip toolStrip_convertProgress;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

