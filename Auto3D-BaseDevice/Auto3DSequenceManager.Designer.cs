namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    partial class Auto3DSequenceManager
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Auto3DSequenceManager));
			this.label4 = new System.Windows.Forms.Label();
			this.panelKeyPad = new System.Windows.Forms.Panel();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.listBox2D3DSBS = new System.Windows.Forms.ListBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label3 = new System.Windows.Forms.Label();
			this.listBox3DSBS2D = new System.Windows.Forms.ListBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.label5 = new System.Windows.Forms.Label();
			this.listBox2D3DTAB = new System.Windows.Forms.ListBox();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.label6 = new System.Windows.Forms.Label();
			this.listBox3DTAB2D = new System.Windows.Forms.ListBox();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.listBox2D3D = new System.Windows.Forms.ListBox();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.label8 = new System.Windows.Forms.Label();
			this.listBox3D2D = new System.Windows.Forms.ListBox();
			this.imageListTabs = new System.Windows.Forms.ImageList(this.components);
			this.checkBoxSendOnAdd = new System.Windows.Forms.CheckBox();
			this.buttonDelay = new System.Windows.Forms.Button();
			this.buttonListDown = new System.Windows.Forms.Button();
			this.buttonListUp = new System.Windows.Forms.Button();
			this.buttonTest = new System.Windows.Forms.Button();
			this.buttonDELETE = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.labelDeviceName = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.buttonCommandTimings = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.tabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.tabPage6.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.ForeColor = System.Drawing.Color.Black;
			this.label4.Location = new System.Drawing.Point(11, 87);
			this.label4.Margin = new System.Windows.Forms.Padding(0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(151, 13);
			this.label4.TabIndex = 50;
			this.label4.Text = "Remote Command Sequences";
			// 
			// panelKeyPad
			// 
			this.panelKeyPad.BackColor = System.Drawing.Color.Transparent;
			this.panelKeyPad.Location = new System.Drawing.Point(507, 128);
			this.panelKeyPad.Name = "panelKeyPad";
			this.panelKeyPad.Size = new System.Drawing.Size(256, 256);
			this.panelKeyPad.TabIndex = 70;
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.tabPage2);
			this.tabControl.Controls.Add(this.tabPage3);
			this.tabControl.Controls.Add(this.tabPage4);
			this.tabControl.Controls.Add(this.tabPage5);
			this.tabControl.Controls.Add(this.tabPage6);
			this.tabControl.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabControl.Location = new System.Drawing.Point(14, 105);
			this.tabControl.Multiline = true;
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(414, 281);
			this.tabControl.TabIndex = 71;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.listBox2D3DSBS);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(406, 255);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "2D>3DSBS";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(153, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Switch TV from 2D to 3D SBS";
			// 
			// listBox2D3DSBS
			// 
			this.listBox2D3DSBS.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.listBox2D3DSBS.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listBox2D3DSBS.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listBox2D3DSBS.FormattingEnabled = true;
			this.listBox2D3DSBS.IntegralHeight = false;
			this.listBox2D3DSBS.ItemHeight = 15;
			this.listBox2D3DSBS.Location = new System.Drawing.Point(3, 36);
			this.listBox2D3DSBS.Name = "listBox2D3DSBS";
			this.listBox2D3DSBS.Size = new System.Drawing.Size(400, 216);
			this.listBox2D3DSBS.TabIndex = 2;
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.listBox3DSBS2D);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(406, 255);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "3DSBS>2D";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(153, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Switch TV from 3D SBS to 2D";
			// 
			// listBox3DSBS2D
			// 
			this.listBox3DSBS2D.BackColor = System.Drawing.SystemColors.Window;
			this.listBox3DSBS2D.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.listBox3DSBS2D.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listBox3DSBS2D.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.listBox3DSBS2D.ForeColor = System.Drawing.Color.Black;
			this.listBox3DSBS2D.FormattingEnabled = true;
			this.listBox3DSBS2D.IntegralHeight = false;
			this.listBox3DSBS2D.ItemHeight = 15;
			this.listBox3DSBS2D.Location = new System.Drawing.Point(3, 36);
			this.listBox3DSBS2D.Name = "listBox3DSBS2D";
			this.listBox3DSBS2D.Size = new System.Drawing.Size(400, 216);
			this.listBox3DSBS2D.TabIndex = 2;
			// 
			// tabPage3
			// 
			this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage3.Controls.Add(this.label5);
			this.tabPage3.Controls.Add(this.listBox2D3DTAB);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(406, 255);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "2D>3DTAB";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 12);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(152, 13);
			this.label5.TabIndex = 5;
			this.label5.Text = "Switch TV from 2D to 3D TAB";
			// 
			// listBox2D3DTAB
			// 
			this.listBox2D3DTAB.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.listBox2D3DTAB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listBox2D3DTAB.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.listBox2D3DTAB.FormattingEnabled = true;
			this.listBox2D3DTAB.IntegralHeight = false;
			this.listBox2D3DTAB.ItemHeight = 15;
			this.listBox2D3DTAB.Location = new System.Drawing.Point(3, 36);
			this.listBox2D3DTAB.Name = "listBox2D3DTAB";
			this.listBox2D3DTAB.Size = new System.Drawing.Size(400, 216);
			this.listBox2D3DTAB.TabIndex = 4;
			// 
			// tabPage4
			// 
			this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage4.Controls.Add(this.label6);
			this.tabPage4.Controls.Add(this.listBox3DTAB2D);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(406, 255);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "3DTAB>2D";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 12);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(152, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "Switch TV from 3D TAB to 2D";
			// 
			// listBox3DTAB2D
			// 
			this.listBox3DTAB2D.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.listBox3DTAB2D.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listBox3DTAB2D.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.listBox3DTAB2D.FormattingEnabled = true;
			this.listBox3DTAB2D.IntegralHeight = false;
			this.listBox3DTAB2D.ItemHeight = 15;
			this.listBox3DTAB2D.Location = new System.Drawing.Point(3, 36);
			this.listBox3DTAB2D.Name = "listBox3DTAB2D";
			this.listBox3DTAB2D.Size = new System.Drawing.Size(400, 216);
			this.listBox3DTAB2D.TabIndex = 4;
			// 
			// tabPage5
			// 
			this.tabPage5.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage5.Controls.Add(this.label7);
			this.tabPage5.Controls.Add(this.listBox2D3D);
			this.tabPage5.Location = new System.Drawing.Point(4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage5.Size = new System.Drawing.Size(406, 255);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "2D>3D(TV)";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 12);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(199, 13);
			this.label7.TabIndex = 5;
			this.label7.Text = "Switch TV from 2D to 3D - Conversion";
			// 
			// listBox2D3D
			// 
			this.listBox2D3D.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.listBox2D3D.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listBox2D3D.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.listBox2D3D.FormattingEnabled = true;
			this.listBox2D3D.IntegralHeight = false;
			this.listBox2D3D.ItemHeight = 15;
			this.listBox2D3D.Location = new System.Drawing.Point(3, 36);
			this.listBox2D3D.Name = "listBox2D3D";
			this.listBox2D3D.Size = new System.Drawing.Size(400, 216);
			this.listBox2D3D.TabIndex = 4;
			// 
			// tabPage6
			// 
			this.tabPage6.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage6.Controls.Add(this.label8);
			this.tabPage6.Controls.Add(this.listBox3D2D);
			this.tabPage6.Location = new System.Drawing.Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage6.Size = new System.Drawing.Size(406, 255);
			this.tabPage6.TabIndex = 5;
			this.tabPage6.Text = "3D(TV)>2D";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 12);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(199, 13);
			this.label8.TabIndex = 5;
			this.label8.Text = "Switch TV from 3D - Conversion to 2D";
			// 
			// listBox3D2D
			// 
			this.listBox3D2D.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.listBox3D2D.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listBox3D2D.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.listBox3D2D.FormattingEnabled = true;
			this.listBox3D2D.IntegralHeight = false;
			this.listBox3D2D.ItemHeight = 15;
			this.listBox3D2D.Location = new System.Drawing.Point(3, 36);
			this.listBox3D2D.Name = "listBox3D2D";
			this.listBox3D2D.Size = new System.Drawing.Size(400, 216);
			this.listBox3D2D.TabIndex = 4;
			// 
			// imageListTabs
			// 
			this.imageListTabs.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageListTabs.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListTabs.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// checkBoxSendOnAdd
			// 
			this.checkBoxSendOnAdd.AutoSize = true;
			this.checkBoxSendOnAdd.Location = new System.Drawing.Point(21, 390);
			this.checkBoxSendOnAdd.Name = "checkBoxSendOnAdd";
			this.checkBoxSendOnAdd.Size = new System.Drawing.Size(163, 17);
			this.checkBoxSendOnAdd.TabIndex = 76;
			this.checkBoxSendOnAdd.Text = "Send command on add to list";
			this.checkBoxSendOnAdd.UseVisualStyleBackColor = true;
			// 
			// buttonDelay
			// 
			this.buttonDelay.Font = new System.Drawing.Font("Wingdings", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.buttonDelay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.buttonDelay.Image = global::MediaPortal.ProcessPlugins.Auto3D.Devices.Properties.Resources.Delay;
			this.buttonDelay.Location = new System.Drawing.Point(432, 294);
			this.buttonDelay.Name = "buttonDelay";
			this.buttonDelay.Size = new System.Drawing.Size(57, 43);
			this.buttonDelay.TabIndex = 75;
			this.buttonDelay.UseVisualStyleBackColor = true;
			this.buttonDelay.Click += new System.EventHandler(this.buttonDelay_Click);
			// 
			// buttonListDown
			// 
			this.buttonListDown.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.buttonListDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.buttonListDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonListDown.Image")));
			this.buttonListDown.Location = new System.Drawing.Point(431, 225);
			this.buttonListDown.Name = "buttonListDown";
			this.buttonListDown.Size = new System.Drawing.Size(57, 43);
			this.buttonListDown.TabIndex = 32;
			this.buttonListDown.UseVisualStyleBackColor = true;
			this.buttonListDown.Click += new System.EventHandler(this.buttonListDown_Click);
			// 
			// buttonListUp
			// 
			this.buttonListUp.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.buttonListUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.buttonListUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonListUp.Image")));
			this.buttonListUp.Location = new System.Drawing.Point(431, 176);
			this.buttonListUp.Name = "buttonListUp";
			this.buttonListUp.Size = new System.Drawing.Size(57, 43);
			this.buttonListUp.TabIndex = 31;
			this.buttonListUp.UseVisualStyleBackColor = true;
			this.buttonListUp.Click += new System.EventHandler(this.buttonListUp_Click);
			// 
			// buttonTest
			// 
			this.buttonTest.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.buttonTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.buttonTest.Image = global::MediaPortal.ProcessPlugins.Auto3D.Devices.Properties.Resources.Test;
			this.buttonTest.Location = new System.Drawing.Point(432, 343);
			this.buttonTest.Name = "buttonTest";
			this.buttonTest.Size = new System.Drawing.Size(57, 43);
			this.buttonTest.TabIndex = 34;
			this.buttonTest.UseVisualStyleBackColor = true;
			this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
			// 
			// buttonDELETE
			// 
			this.buttonDELETE.Font = new System.Drawing.Font("Wingdings", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.buttonDELETE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.buttonDELETE.Image = global::MediaPortal.ProcessPlugins.Auto3D.Devices.Properties.Resources.Cut;
			this.buttonDELETE.Location = new System.Drawing.Point(431, 127);
			this.buttonDELETE.Name = "buttonDELETE";
			this.buttonDELETE.Size = new System.Drawing.Size(58, 43);
			this.buttonDELETE.TabIndex = 33;
			this.buttonDELETE.UseVisualStyleBackColor = true;
			this.buttonDELETE.Click += new System.EventHandler(this.buttonDELETE_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
			this.panel1.Controls.Add(this.labelDeviceName);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(779, 80);
			this.panel1.TabIndex = 73;
			// 
			// labelDeviceName
			// 
			this.labelDeviceName.BackColor = System.Drawing.Color.Transparent;
			this.labelDeviceName.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDeviceName.ForeColor = System.Drawing.Color.White;
			this.labelDeviceName.Location = new System.Drawing.Point(123, 39);
			this.labelDeviceName.Name = "labelDeviceName";
			this.labelDeviceName.Size = new System.Drawing.Size(371, 37);
			this.labelDeviceName.TabIndex = 30;
			this.labelDeviceName.Text = "Device";
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
			this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
			this.panel2.Controls.Add(this.buttonCommandTimings);
			this.panel2.Controls.Add(this.buttonCancel);
			this.panel2.Controls.Add(this.buttonOK);
			this.panel2.Location = new System.Drawing.Point(0, 413);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(777, 59);
			this.panel2.TabIndex = 74;
			// 
			// buttonCommandTimings
			// 
			this.buttonCommandTimings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonCommandTimings.ForeColor = System.Drawing.Color.Black;
			this.buttonCommandTimings.Location = new System.Drawing.Point(14, 17);
			this.buttonCommandTimings.Name = "buttonCommandTimings";
			this.buttonCommandTimings.Size = new System.Drawing.Size(121, 26);
			this.buttonCommandTimings.TabIndex = 73;
			this.buttonCommandTimings.Text = "Command Settings...";
			this.buttonCommandTimings.UseVisualStyleBackColor = true;
			this.buttonCommandTimings.Click += new System.EventHandler(this.buttonCommandTimings_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonCancel.ForeColor = System.Drawing.Color.Black;
			this.buttonCancel.Location = new System.Drawing.Point(690, 17);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(73, 26);
			this.buttonCancel.TabIndex = 72;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonOK.ForeColor = System.Drawing.Color.Black;
			this.buttonOK.Location = new System.Drawing.Point(611, 17);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(73, 26);
			this.buttonOK.TabIndex = 67;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// Auto3DSequenceManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.ClientSize = new System.Drawing.Size(777, 469);
			this.Controls.Add(this.checkBoxSendOnAdd);
			this.Controls.Add(this.buttonDelay);
			this.Controls.Add(this.buttonListDown);
			this.Controls.Add(this.buttonListUp);
			this.Controls.Add(this.panelKeyPad);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.buttonTest);
			this.Controls.Add(this.buttonDELETE);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Auto3DSequenceManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Auto3D Sequence Manager";
			this.tabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.tabPage4.ResumeLayout(false);
			this.tabPage4.PerformLayout();
			this.tabPage5.ResumeLayout(false);
			this.tabPage5.PerformLayout();
			this.tabPage6.ResumeLayout(false);
			this.tabPage6.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonListUp;
        private System.Windows.Forms.Button buttonListDown;
        private System.Windows.Forms.Button buttonDELETE;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelKeyPad;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.ListBox listBox2D3DSBS;
        private System.Windows.Forms.ListBox listBox3DTAB2D;
        private System.Windows.Forms.ListBox listBox2D3D;
        private System.Windows.Forms.ListBox listBox3D2D;
        private System.Windows.Forms.ListBox listBox2D3DTAB;
        private System.Windows.Forms.ListBox listBox3DSBS2D;
        private System.Windows.Forms.ImageList imageListTabs;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonCommandTimings;
        private System.Windows.Forms.Button buttonDelay;
        private System.Windows.Forms.CheckBox checkBoxSendOnAdd;
        private System.Windows.Forms.Label labelDeviceName;
    }
}