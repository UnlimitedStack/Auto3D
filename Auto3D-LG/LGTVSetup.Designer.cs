namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    partial class LGTVSetup
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LGTVSetup));
      this.comboBoxModel = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.pictureBox = new System.Windows.Forms.PictureBox();
      this.buttonShowKey = new System.Windows.Forms.Button();
      this.listBoxCompatibleModels = new System.Windows.Forms.ListBox();
      this.label1 = new System.Windows.Forms.Label();
      this.textBoxPairingKey = new System.Windows.Forms.TextBox();
      this.buttonSendKey = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.comboBoxTV = new System.Windows.Forms.ComboBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.labelRegister = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // comboBoxModel
      // 
      this.comboBoxModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxModel.FormattingEnabled = true;
      this.comboBoxModel.Location = new System.Drawing.Point(0, 21);
      this.comboBoxModel.Name = "comboBoxModel";
      this.comboBoxModel.Size = new System.Drawing.Size(280, 21);
      this.comboBoxModel.TabIndex = 24;
      this.comboBoxModel.SelectedIndexChanged += new System.EventHandler(this.comboBoxModel_SelectedIndexChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(-3, 4);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(67, 13);
      this.label3.TabIndex = 25;
      this.label3.Text = "Model Name";
      // 
      // pictureBox
      // 
      this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox.BackColor = System.Drawing.SystemColors.ButtonFace;
      this.pictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox.BackgroundImage")));
      this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.pictureBox.InitialImage = null;
      this.pictureBox.Location = new System.Drawing.Point(293, 215);
      this.pictureBox.Name = "pictureBox";
      this.pictureBox.Size = new System.Drawing.Size(228, 150);
      this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox.TabIndex = 23;
      this.pictureBox.TabStop = false;
      // 
      // buttonShowKey
      // 
      this.buttonShowKey.Location = new System.Drawing.Point(13, 70);
      this.buttonShowKey.Name = "buttonShowKey";
      this.buttonShowKey.Size = new System.Drawing.Size(110, 23);
      this.buttonShowKey.TabIndex = 28;
      this.buttonShowKey.Text = "Show key on TV";
      this.buttonShowKey.UseVisualStyleBackColor = true;
      this.buttonShowKey.Click += new System.EventHandler(this.buttonShowKey_Click);
      // 
      // listBoxCompatibleModels
      // 
      this.listBoxCompatibleModels.BackColor = System.Drawing.SystemColors.Window;
      this.listBoxCompatibleModels.FormattingEnabled = true;
      this.listBoxCompatibleModels.IntegralHeight = false;
      this.listBoxCompatibleModels.Location = new System.Drawing.Point(0, 119);
      this.listBoxCompatibleModels.Name = "listBoxCompatibleModels";
      this.listBoxCompatibleModels.SelectionMode = System.Windows.Forms.SelectionMode.None;
      this.listBoxCompatibleModels.Size = new System.Drawing.Size(280, 249);
      this.listBoxCompatibleModels.TabIndex = 32;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(-2, 102);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(99, 13);
      this.label1.TabIndex = 31;
      this.label1.Text = "Compatible Models:";
      // 
      // textBoxPairingKey
      // 
      this.textBoxPairingKey.Location = new System.Drawing.Point(12, 112);
      this.textBoxPairingKey.Name = "textBoxPairingKey";
      this.textBoxPairingKey.Size = new System.Drawing.Size(111, 20);
      this.textBoxPairingKey.TabIndex = 33;
      // 
      // buttonSendKey
      // 
      this.buttonSendKey.Location = new System.Drawing.Point(134, 110);
      this.buttonSendKey.Name = "buttonSendKey";
      this.buttonSendKey.Size = new System.Drawing.Size(70, 23);
      this.buttonSendKey.TabIndex = 35;
      this.buttonSendKey.Text = "Send key";
      this.buttonSendKey.UseVisualStyleBackColor = true;
      this.buttonSendKey.Click += new System.EventHandler(this.buttonSendKey_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(-3, 51);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(197, 13);
      this.label2.TabIndex = 37;
      this.label2.Text = " IP-Address or Network-Name of the TV:";
      // 
      // comboBoxTV
      // 
      this.comboBoxTV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxTV.FormattingEnabled = true;
      this.comboBoxTV.Location = new System.Drawing.Point(0, 68);
      this.comboBoxTV.Name = "comboBoxTV";
      this.comboBoxTV.Size = new System.Drawing.Size(280, 21);
      this.comboBoxTV.TabIndex = 38;
      this.comboBoxTV.SelectedIndexChanged += new System.EventHandler(this.comboBoxTV_SelectedIndexChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.labelRegister);
      this.groupBox1.Controls.Add(this.buttonShowKey);
      this.groupBox1.Controls.Add(this.textBoxPairingKey);
      this.groupBox1.Controls.Add(this.buttonSendKey);
      this.groupBox1.Location = new System.Drawing.Point(293, 7);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(231, 153);
      this.groupBox1.TabIndex = 39;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = " Pairing ";
      // 
      // labelRegister
      // 
      this.labelRegister.Location = new System.Drawing.Point(10, 21);
      this.labelRegister.Name = "labelRegister";
      this.labelRegister.Size = new System.Drawing.Size(189, 41);
      this.labelRegister.TabIndex = 40;
      this.labelRegister.Text = "Every client has to be paired once with the TV before sending commands to the TV." +
    "";
      // 
      // LGTVSetup
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.comboBoxTV);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.listBoxCompatibleModels);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.comboBoxModel);
      this.Controls.Add(this.pictureBox);
      this.MinimumSize = new System.Drawing.Size(314, 286);
      this.Name = "LGTVSetup";
      this.Size = new System.Drawing.Size(524, 368);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ComboBox comboBoxModel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonShowKey;
        private System.Windows.Forms.ListBox listBoxCompatibleModels;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPairingKey;
        private System.Windows.Forms.Button buttonSendKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxTV;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelRegister;
    }
}
