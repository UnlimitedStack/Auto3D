namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    partial class Auto3DTimings
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Auto3DTimings));
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.comboBoxCommands = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panelRemoteInput = new System.Windows.Forms.Panel();
			this.buttonCancelInput = new System.Windows.Forms.Button();
			this.labelStatus = new System.Windows.Forms.Label();
			this.buttonClear = new System.Windows.Forms.Button();
			this.buttonLearn = new System.Windows.Forms.Button();
			this.buttonSend = new System.Windows.Forms.Button();
			this.labelIrCode = new System.Windows.Forms.Label();
			this.textBoxIrCode = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxDelay = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panelRemoteInput.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.Location = new System.Drawing.Point(560, 12);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.Location = new System.Drawing.Point(641, 12);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
			this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.panel1.Controls.Add(this.label5);
			this.panel1.Location = new System.Drawing.Point(0, 1);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(728, 47);
			this.panel1.TabIndex = 3;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.ForeColor = System.Drawing.Color.White;
			this.label5.Location = new System.Drawing.Point(12, 18);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(166, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Modfy settings for each command";
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
			this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
			this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.panel2.Controls.Add(this.buttonCancel);
			this.panel2.Controls.Add(this.buttonOK);
			this.panel2.Location = new System.Drawing.Point(0, 203);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(728, 47);
			this.panel2.TabIndex = 4;
			// 
			// comboBoxCommands
			// 
			this.comboBoxCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCommands.FormattingEnabled = true;
			this.comboBoxCommands.Location = new System.Drawing.Point(26, 82);
			this.comboBoxCommands.Name = "comboBoxCommands";
			this.comboBoxCommands.Size = new System.Drawing.Size(237, 21);
			this.comboBoxCommands.TabIndex = 5;
			this.comboBoxCommands.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommands_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(24, 62);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 13);
			this.label1.TabIndex = 17;
			this.label1.Text = "Commands";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.panelRemoteInput);
			this.groupBox1.Controls.Add(this.buttonClear);
			this.groupBox1.Controls.Add(this.buttonLearn);
			this.groupBox1.Controls.Add(this.buttonSend);
			this.groupBox1.Controls.Add(this.labelIrCode);
			this.groupBox1.Controls.Add(this.textBoxIrCode);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textBoxDelay);
			this.groupBox1.Location = new System.Drawing.Point(12, 86);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(704, 101);
			this.groupBox1.TabIndex = 18;
			this.groupBox1.TabStop = false;
			// 
			// panelRemoteInput
			// 
			this.panelRemoteInput.Controls.Add(this.buttonCancelInput);
			this.panelRemoteInput.Controls.Add(this.labelStatus);
			this.panelRemoteInput.Location = new System.Drawing.Point(68, 54);
			this.panelRemoteInput.Name = "panelRemoteInput";
			this.panelRemoteInput.Size = new System.Drawing.Size(621, 25);
			this.panelRemoteInput.TabIndex = 19;
			// 
			// buttonCancelInput
			// 
			this.buttonCancelInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancelInput.Location = new System.Drawing.Point(524, 1);
			this.buttonCancelInput.Name = "buttonCancelInput";
			this.buttonCancelInput.Size = new System.Drawing.Size(95, 23);
			this.buttonCancelInput.TabIndex = 2;
			this.buttonCancelInput.Text = "Cancel Input";
			this.buttonCancelInput.UseVisualStyleBackColor = true;
			this.buttonCancelInput.Click += new System.EventHandler(this.buttonCancelInput_Click);
			// 
			// labelStatus
			// 
			this.labelStatus.AutoSize = true;
			this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelStatus.ForeColor = System.Drawing.Color.DarkRed;
			this.labelStatus.Location = new System.Drawing.Point(6, 5);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(434, 13);
			this.labelStatus.TabIndex = 0;
			this.labelStatus.Text = "Waiting for remote input. Please press the remote key you want to link with this " +
    "command...";
			// 
			// buttonClear
			// 
			this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClear.Location = new System.Drawing.Point(636, 56);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(51, 23);
			this.buttonClear.TabIndex = 23;
			this.buttonClear.Text = "Clear";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// buttonLearn
			// 
			this.buttonLearn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonLearn.Location = new System.Drawing.Point(522, 56);
			this.buttonLearn.Name = "buttonLearn";
			this.buttonLearn.Size = new System.Drawing.Size(52, 23);
			this.buttonLearn.TabIndex = 23;
			this.buttonLearn.Text = "Learn";
			this.buttonLearn.UseVisualStyleBackColor = true;
			this.buttonLearn.Click += new System.EventHandler(this.buttonLearn_Click);
			// 
			// buttonSend
			// 
			this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSend.Location = new System.Drawing.Point(580, 56);
			this.buttonSend.Name = "buttonSend";
			this.buttonSend.Size = new System.Drawing.Size(51, 23);
			this.buttonSend.TabIndex = 22;
			this.buttonSend.Text = "Send";
			this.buttonSend.UseVisualStyleBackColor = true;
			this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
			// 
			// labelIrCode
			// 
			this.labelIrCode.AutoSize = true;
			this.labelIrCode.Location = new System.Drawing.Point(14, 61);
			this.labelIrCode.Name = "labelIrCode";
			this.labelIrCode.Size = new System.Drawing.Size(49, 13);
			this.labelIrCode.TabIndex = 21;
			this.labelIrCode.Text = "IR-Code:";
			// 
			// textBoxIrCode
			// 
			this.textBoxIrCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxIrCode.Location = new System.Drawing.Point(68, 58);
			this.textBoxIrCode.Name = "textBoxIrCode";
			this.textBoxIrCode.ReadOnly = true;
			this.textBoxIrCode.Size = new System.Drawing.Size(448, 20);
			this.textBoxIrCode.TabIndex = 20;
			this.textBoxIrCode.TextChanged += new System.EventHandler(this.textBoxIrCode_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(134, 36);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(20, 13);
			this.label3.TabIndex = 19;
			this.label3.Text = "ms";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(37, 13);
			this.label2.TabIndex = 18;
			this.label2.Text = "Delay:";
			// 
			// textBoxDelay
			// 
			this.textBoxDelay.Location = new System.Drawing.Point(68, 33);
			this.textBoxDelay.Name = "textBoxDelay";
			this.textBoxDelay.Size = new System.Drawing.Size(60, 20);
			this.textBoxDelay.TabIndex = 17;
			this.textBoxDelay.TextChanged += new System.EventHandler(this.textBoxDelay_TextChanged);
			// 
			// Auto3DTimings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(728, 251);
			this.Controls.Add(this.comboBoxCommands);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Auto3DTimings";
			this.Text = "Auto3D Command Settings";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panelRemoteInput.ResumeLayout(false);
			this.panelRemoteInput.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ComboBox comboBoxCommands;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonLearn;
		private System.Windows.Forms.Button buttonSend;
		private System.Windows.Forms.Label labelIrCode;
		private System.Windows.Forms.TextBox textBoxIrCode;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxDelay;
		private System.Windows.Forms.Panel panelRemoteInput;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.Button buttonCancelInput;
		private System.Windows.Forms.Button buttonClear;
    }
}