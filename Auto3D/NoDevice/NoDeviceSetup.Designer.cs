namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    partial class NoDeviceSetup
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
      this.label3 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(0, 16);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(521, 24);
      this.label3.TabIndex = 25;
      this.label3.Text = "No device selected!";
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(1, 64);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(520, 43);
      this.label1.TabIndex = 26;
      this.label1.Text = "In this case the plugin switches only the mediaPortal GUI into 3D mode if possibl" +
    "e, the TV has to be switched manually or with a remote controller via EventGhost" +
    " (see Options-Tab).";
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(0, 41);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(521, 24);
      this.label2.TabIndex = 27;
      this.label2.Text = "Use this setting if your TV is not supported by the Auto3D Plugin.";
      // 
      // NoDeviceSetup
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label3);
      this.MinimumSize = new System.Drawing.Size(314, 368);
      this.Name = "NoDeviceSetup";
      this.Size = new System.Drawing.Size(524, 368);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

    }
}
