namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    partial class PhilipsTVSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhilipsTVSetup));
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.comboBoxModel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listBoxCompatibleModels = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxInterface = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCheckConnection = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-4, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = " IP-Address or Network-Name of the TV:";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(0, 111);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(236, 20);
            this.textBoxIP.TabIndex = 17;
            this.textBoxIP.TextChanged += new System.EventHandler(this.textBoxIP_TextChanged);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox.BackgroundImage")));
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox.InitialImage = null;
            this.pictureBox.Location = new System.Drawing.Point(72, 256);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(154, 109);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 23;
            this.pictureBox.TabStop = false;
            // 
            // comboBoxModel
            // 
            this.comboBoxModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxModel.FormattingEnabled = true;
            this.comboBoxModel.Location = new System.Drawing.Point(0, 21);
            this.comboBoxModel.Name = "comboBoxModel";
            this.comboBoxModel.Size = new System.Drawing.Size(238, 21);
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
            // listBoxCompatibleModels
            // 
            this.listBoxCompatibleModels.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.listBoxCompatibleModels.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxCompatibleModels.FormattingEnabled = true;
            this.listBoxCompatibleModels.Location = new System.Drawing.Point(4, 158);
            this.listBoxCompatibleModels.Name = "listBoxCompatibleModels";
            this.listBoxCompatibleModels.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBoxCompatibleModels.Size = new System.Drawing.Size(232, 91);
            this.listBoxCompatibleModels.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Compatible Models:";
            // 
            // comboBoxInterface
            // 
            this.comboBoxInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInterface.FormattingEnabled = true;
            this.comboBoxInterface.Items.AddRange(new object[] {
            "jointSpace\t",
            "DirectFB"});
            this.comboBoxInterface.Location = new System.Drawing.Point(0, 65);
            this.comboBoxInterface.Name = "comboBoxInterface";
            this.comboBoxInterface.Size = new System.Drawing.Size(124, 21);
            this.comboBoxInterface.TabIndex = 37;
            this.comboBoxInterface.SelectedIndexChanged += new System.EventHandler(this.comboBoxInterface_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-2, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Access Method";
            // 
            // btnCheckConnection
            // 
            this.btnCheckConnection.Location = new System.Drawing.Point(242, 110);
            this.btnCheckConnection.Name = "btnCheckConnection";
            this.btnCheckConnection.Size = new System.Drawing.Size(69, 22);
            this.btnCheckConnection.TabIndex = 39;
            this.btnCheckConnection.Text = "Check";
            this.btnCheckConnection.UseVisualStyleBackColor = true;
            this.btnCheckConnection.Click += new System.EventHandler(this.btnCheckConnection_Click);
            // 
            // PhilipsTVSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCheckConnection);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxInterface);
            this.Controls.Add(this.listBoxCompatibleModels);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxModel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.pictureBox);
            this.MinimumSize = new System.Drawing.Size(314, 286);
            this.Name = "PhilipsTVSetup";
            this.Size = new System.Drawing.Size(314, 368);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ComboBox comboBoxModel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBoxCompatibleModels;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxInterface;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCheckConnection;
    }
}
