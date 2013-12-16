using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public partial class Auto3DTimings : Form
  {
    Point _location;
    Auto3DBaseDevice _device;
    int _totalHeight;

    public Auto3DTimings(IAuto3D device)
    {
      InitializeComponent();

      _location = new Point(16, 60);
      _device = (Auto3DBaseDevice)device;
      _totalHeight = Height;
      CreateTimingsControls();
      Height = _totalHeight + 12;
    }

    private void CreateTimingsControls()
    {
      foreach (RemoteCommand rc in _device.RemoteCommands)
      {
        Label label = new Label();
        label.Bounds = new Rectangle(_location.X, _location.Y + 3, 120, 20);
        label.Text = rc.Command;

        Controls.Add(label);

        TextBox textBox = new TextBox();
        textBox.Bounds = new Rectangle(_location.X + label.Bounds.Width + 4, _location.Y, 40, 20);
        textBox.Text = rc.Delay.ToString();
        textBox.Tag = rc.Command;

        Controls.Add(textBox);

        Label labelMS = new Label();
        labelMS.Bounds = new Rectangle(textBox.Bounds.Right + 4, textBox.Bounds.Y + 3, 20, textBox.Bounds.Height);
        labelMS.Text = "ms";

        Controls.Add(labelMS);

        _location.Y += 30;
        _totalHeight += 30;
      }
    }

    private void SaveTimingsFromControls(Control control)
    {
      foreach (Control textBox in control.Controls)
      {
        if (textBox is TextBox)
        {
          RemoteCommand rc = _device.GetRemoteCommandFromString(textBox.Tag.ToString());

          int delay = int.Parse(textBox.Text);

          if (rc.Delay != delay)
          {
            rc.Delay = delay;
            _device.Modified = true;
          }
        }
        else
          SaveTimingsFromControls(textBox);
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      SaveTimingsFromControls(this);
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}
