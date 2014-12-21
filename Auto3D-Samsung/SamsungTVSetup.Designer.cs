namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    partial class SamsungTVSetup
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SamsungTVSetup));
      this.label2 = new System.Windows.Forms.Label();
      this.comboBoxModel = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.pictureBox = new System.Windows.Forms.PictureBox();
      this.comboBoxTV = new System.Windows.Forms.ComboBox();
      this.listBoxCompatibleModels = new System.Windows.Forms.ListBox();
      this.label1 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(-3, 49);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(78, 13);
      this.label2.TabIndex = 18;
      this.label2.Text = "TVs in network";
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
      this.pictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox.BackgroundImage")));
      this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.pictureBox.InitialImage = null;
      this.pictureBox.Location = new System.Drawing.Point(295, 206);
      this.pictureBox.Name = "pictureBox";
      this.pictureBox.Size = new System.Drawing.Size(229, 159);
      this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox.TabIndex = 23;
      this.pictureBox.TabStop = false;
      // 
      // comboBoxTV
      // 
      this.comboBoxTV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxTV.FormattingEnabled = true;
      this.comboBoxTV.Location = new System.Drawing.Point(0, 65);
      this.comboBoxTV.Name = "comboBoxTV";
      this.comboBoxTV.Size = new System.Drawing.Size(280, 21);
      this.comboBoxTV.TabIndex = 27;
      this.comboBoxTV.SelectedIndexChanged += new System.EventHandler(this.comboBoxTV_SelectedIndexChanged);
      // 
      // listBoxCompatibleModels
      // 
      this.listBoxCompatibleModels.BackColor = System.Drawing.SystemColors.Window;
      this.listBoxCompatibleModels.FormattingEnabled = true;
      this.listBoxCompatibleModels.IntegralHeight = false;
      this.listBoxCompatibleModels.Location = new System.Drawing.Point(0, 116);
      this.listBoxCompatibleModels.Name = "listBoxCompatibleModels";
      this.listBoxCompatibleModels.SelectionMode = System.Windows.Forms.SelectionMode.None;
      this.listBoxCompatibleModels.Size = new System.Drawing.Size(276, 252);
      this.listBoxCompatibleModels.TabIndex = 34;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(-2, 98);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(99, 13);
      this.label1.TabIndex = 33;
      this.label1.Text = "Compatible Models:";
      // 
      // SamsungTVSetup
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.listBoxCompatibleModels);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.comboBoxTV);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.comboBoxModel);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.pictureBox);
      this.MinimumSize = new System.Drawing.Size(314, 286);
      this.Name = "SamsungTVSetup";
      this.Size = new System.Drawing.Size(524, 368);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ComboBox comboBoxModel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxTV;
        private System.Windows.Forms.ListBox listBoxCompatibleModels;
        private System.Windows.Forms.Label label1;
    }
}
