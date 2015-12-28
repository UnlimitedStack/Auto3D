namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
	partial class GenericeDeviceSetup
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericeDeviceSetup));
			this.comboBoxPort = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.checkAllowIRCommandsForOtherDevices = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.checkBoxPingCheck = new System.Windows.Forms.CheckBox();
			this.textBoxGenericIP = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.buttonPingGenericDevice = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// comboBoxPort
			// 
			this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxPort.FormattingEnabled = true;
			this.comboBoxPort.Location = new System.Drawing.Point(1, 21);
			this.comboBoxPort.Name = "comboBoxPort";
			this.comboBoxPort.Size = new System.Drawing.Size(70, 21);
			this.comboBoxPort.TabIndex = 26;
			this.comboBoxPort.SelectedIndexChanged += new System.EventHandler(this.comboBoxPort_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(-1, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(26, 13);
			this.label1.TabIndex = 27;
			this.label1.Text = "Port";
			// 
			// pictureBox
			// 
			this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox.BackgroundImage")));
			this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.pictureBox.InitialImage = null;
			this.pictureBox.Location = new System.Drawing.Point(290, 158);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(232, 208);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox.TabIndex = 23;
			this.pictureBox.TabStop = false;
			// 
			// checkAllowIRCommandsForOtherDevices
			// 
			this.checkAllowIRCommandsForOtherDevices.AutoSize = true;
			this.checkAllowIRCommandsForOtherDevices.Location = new System.Drawing.Point(0, 172);
			this.checkAllowIRCommandsForOtherDevices.Name = "checkAllowIRCommandsForOtherDevices";
			this.checkAllowIRCommandsForOtherDevices.Size = new System.Drawing.Size(188, 17);
			this.checkAllowIRCommandsForOtherDevices.TabIndex = 29;
			this.checkAllowIRCommandsForOtherDevices.Text = "Allow IR Commands for all devices";
			this.checkAllowIRCommandsForOtherDevices.UseVisualStyleBackColor = true;
			this.checkAllowIRCommandsForOtherDevices.CheckedChanged += new System.EventHandler(this.checkBoxOther_CheckedChanged);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.SystemColors.Control;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(17, 192);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(257, 37);
			this.label3.TabIndex = 30;
			this.label3.Text = "Select this option if you want to combine IR commands with the commands of the ot" +
    "her devices.";
			// 
			// checkBoxPingCheck
			// 
			this.checkBoxPingCheck.AutoSize = true;
			this.checkBoxPingCheck.Location = new System.Drawing.Point(0, 57);
			this.checkBoxPingCheck.Name = "checkBoxPingCheck";
			this.checkBoxPingCheck.Size = new System.Drawing.Size(159, 17);
			this.checkBoxPingCheck.TabIndex = 80;
			this.checkBoxPingCheck.Text = "Check device state via Ping";
			this.checkBoxPingCheck.UseVisualStyleBackColor = true;
			this.checkBoxPingCheck.CheckedChanged += new System.EventHandler(this.checkBoxPingCheck_CheckedChanged);
			// 
			// textBoxGenericIP
			// 
			this.textBoxGenericIP.Location = new System.Drawing.Point(18, 96);
			this.textBoxGenericIP.Name = "textBoxGenericIP";
			this.textBoxGenericIP.Size = new System.Drawing.Size(190, 20);
			this.textBoxGenericIP.TabIndex = 81;
			this.textBoxGenericIP.TextChanged += new System.EventHandler(this.textBoxGenericIP_TextChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(13, 79);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(197, 13);
			this.label7.TabIndex = 82;
			this.label7.Text = " IP-Address or Network-Name of the TV:";
			// 
			// buttonPingGenericDevice
			// 
			this.buttonPingGenericDevice.Location = new System.Drawing.Point(215, 94);
			this.buttonPingGenericDevice.Name = "buttonPingGenericDevice";
			this.buttonPingGenericDevice.Size = new System.Drawing.Size(61, 23);
			this.buttonPingGenericDevice.TabIndex = 83;
			this.buttonPingGenericDevice.Text = "Test";
			this.buttonPingGenericDevice.UseVisualStyleBackColor = true;
			this.buttonPingGenericDevice.Click += new System.EventHandler(this.buttonPingGenericDevice_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 122);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(260, 32);
			this.label2.TabIndex = 84;
			this.label2.Text = "This is helpful when using \"Power\" options. See Power tab for more information";
			// 
			// GenericeDeviceSetup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.buttonPingGenericDevice);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textBoxGenericIP);
			this.Controls.Add(this.checkBoxPingCheck);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.checkAllowIRCommandsForOtherDevices);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBoxPort);
			this.Controls.Add(this.pictureBox);
			this.MinimumSize = new System.Drawing.Size(314, 368);
			this.Name = "GenericeDeviceSetup";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(524, 368);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.ComboBox comboBoxPort;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.CheckBox checkAllowIRCommandsForOtherDevices;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox checkBoxPingCheck;
		private System.Windows.Forms.TextBox textBoxGenericIP;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button buttonPingGenericDevice;
		private System.Windows.Forms.Label label2;
    }
}
