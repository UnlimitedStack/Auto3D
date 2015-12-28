using IrToyLibrary;
using MediaPortal.GUI.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Dispatcher;
using System.Windows.Forms;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public partial class Auto3DTimings : Form
  {
    Point _location;
    Auto3DBaseDevice _device;
    int _totalHeight;

	Timer _IrTimer;

    public Auto3DTimings(IAuto3D device)
    {
      InitializeComponent();

      _location = new Point(16, 60);
      _device = (Auto3DBaseDevice)device;
      _totalHeight = Height;

      Height = _totalHeight + 12;
	  CenterToParent();

	  foreach (RemoteCommand rc in _device.RemoteCommands)
	  {
		  RemoteCommand rcTemp = new RemoteCommand(rc.Command, rc.Delay, rc.IrCode);	
		  comboBoxCommands.Items.Add(rcTemp);
	  }

	  comboBoxCommands.SelectedIndex = 0;

	  panelRemoteInput.Visible = false;

	  _IrTimer = new Timer();
	  _IrTimer.Interval = 10000;
	  _IrTimer.Tick += _IrTimer_Tick;

	  if (_device.DeviceName == "Generic TV or Beamer (IR-Toy)")
	  {
		  labelIrCode.Enabled = Auto3DBaseDevice.IsIrConnected();
		  textBoxIrCode.Enabled = Auto3DBaseDevice.IsIrConnected();
		  buttonLearn.Enabled = Auto3DBaseDevice.IsIrConnected();
		  buttonSend.Enabled = Auto3DBaseDevice.IsIrConnected();
		  buttonClear.Enabled = Auto3DBaseDevice.IsIrConnected();
	  }
	  else
	  {
		 labelIrCode.Enabled = Auto3DBaseDevice.AllowIrCommandsForAllDevices && Auto3DBaseDevice.IsIrConnected();
		 textBoxIrCode.Enabled = Auto3DBaseDevice.AllowIrCommandsForAllDevices && Auto3DBaseDevice.IsIrConnected();
		 buttonLearn.Enabled = Auto3DBaseDevice.AllowIrCommandsForAllDevices && Auto3DBaseDevice.IsIrConnected();
		 buttonSend.Enabled = Auto3DBaseDevice.AllowIrCommandsForAllDevices && Auto3DBaseDevice.IsIrConnected();
		 buttonClear.Enabled = Auto3DBaseDevice.AllowIrCommandsForAllDevices && Auto3DBaseDevice.IsIrConnected();
	  }
    }

    private void SaveTimingsFromControls(Control control)
    {
		foreach (RemoteCommand rcTemp in comboBoxCommands.Items)
		{
			foreach (RemoteCommand rc in _device.RemoteCommands)
			{
				if (rcTemp.Command == rc.Command)
				{
					rc.Delay = rcTemp.Delay;
					rc.IrCode = rcTemp.IrCode;
					break;
				}
			}
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

	private void comboBoxCommands_SelectedIndexChanged(object sender, EventArgs e)
	{
		RemoteCommand rc = (RemoteCommand)comboBoxCommands.SelectedItem;

		textBoxDelay.Text = rc.Delay.ToString();
		textBoxIrCode.Text = rc.IrCode;
	}

	private void textBoxDelay_TextChanged(object sender, EventArgs e)
	{
		RemoteCommand rc = (RemoteCommand)comboBoxCommands.SelectedItem;
		rc.Delay = int.Parse(textBoxDelay.Text);
		_device.Modified = true;
	}

	private void textBoxIrCode_TextChanged(object sender, EventArgs e)
	{
		RemoteCommand rc = (RemoteCommand)comboBoxCommands.SelectedItem;
		rc.IrCode = textBoxIrCode.Text;
		_device.Modified = true;
	}

	private void buttonLearn_Click(object sender, EventArgs e)
	{
		labelStatus.Text = "Waiting for remote input. Please press the remote key you want to link with this command...";

		panelRemoteInput.Visible = true;
		buttonCancelInput.Visible = true;		
		
		_device.IrToy.Received += IrToy_Received;

		textBoxIrCode.Text = "";		
	}

	void IrToy_Received(object sender, string code)
	{
		this.Invoke(new MethodInvoker(delegate()
		{
			if (!panelRemoteInput.Visible)
				return;

			RemoteCommand rc = (RemoteCommand)comboBoxCommands.SelectedItem;
			
			if (textBoxIrCode.Text.Length > 0)
			{
				if (!textBoxIrCode.Text.EndsWith(" ") && !code.StartsWith(" "))
					textBoxIrCode.Text += " ";
			}

			textBoxIrCode.Text += code;
			rc.IrCode = textBoxIrCode.Text;

			System.Diagnostics.Debug.WriteLine(code);

			if (_IrTimer.Enabled == false)
			{
				labelStatus.Text = "Please wait - Processing remote input...";
				buttonCancelInput.Visible = false;
				_IrTimer.Interval = 3000;
				_IrTimer.Start();
			}		
			else
			{
				if (code.EndsWith("ff ff"))
				{
					_IrTimer_Tick(_device.IrToy, new EventArgs());
				}
			}
		}));
	}

	void _IrTimer_Tick(object sender, EventArgs e)
	{
		_IrTimer.Stop();
		_device.IrToy.Received -= IrToy_Received;		
		panelRemoteInput.Visible = false;
	}

	private void buttonSend_Click(object sender, EventArgs e)
	{
		RemoteCommand rc = (RemoteCommand)comboBoxCommands.SelectedItem;

		try
		{
			_device.IrToy.Send(rc.IrCode);
			Log.Info("Auto3D: Code sent: " + rc.IrCode);
		}
		catch (Exception ex)
		{
			Auto3DHelpers.ShowAuto3DMessage("Sending code failed: " + ex.Message, false, 0);
			Log.Error("Auto3D: Sending code " + rc.IrCode + " failed: " + ex.Message);
		}
	}

	private void buttonCancelInput_Click(object sender, EventArgs e)
	{
		_IrTimer.Stop();
		_device.IrToy.Received -= IrToy_Received;
		panelRemoteInput.Visible = false;
	}

	private void buttonClear_Click(object sender, EventArgs e)
	{
		textBoxIrCode.Text = "";
	}
  }
}


