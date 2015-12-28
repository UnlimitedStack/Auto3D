using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaPortal.Profile;
using System.IO;
using System.Reflection;
using MediaPortal.Configuration;
using System.IO.Ports;
using System.Xml;
using System.Text.RegularExpressions;
using System.Management;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public partial class GenericeDeviceSetup : UserControl, IAuto3DSetup
  {
    GenericDevice _device;

	public GenericeDeviceSetup(IAuto3D device)
    {
      InitializeComponent();

	  if (!(device is GenericDevice))
        throw new Exception("Auto3D: Device is no Generic Device");

	  _device = (GenericDevice)device;

	  comboBoxPort.Items.Add("None");
	  comboBoxPort.Items.AddRange(SerialPort.GetPortNames());

	  checkAllowIRCommandsForOtherDevices.Checked = Auto3DBaseDevice.AllowIrCommandsForAllDevices;
    }

    public IAuto3D GetDevice()
    {
      return _device;
    }

    public void LoadSettings()
    {
	   _device.SelectedDeviceModel = _device.DeviceModels[0];

      foreach (object port in comboBoxPort.Items)
      {
		  if (port.ToString() == Auto3DBaseDevice.IrPortName)
        {
          comboBoxPort.SelectedItem = port;
          break;
        }
      }

	  if (comboBoxPort.SelectedIndex == -1)
		  comboBoxPort.SelectedIndex = 0;

	  checkBoxPingCheck.Checked = _device.PingCheck;
	  textBoxGenericIP.Enabled = _device.PingCheck;
	  textBoxGenericIP.Text = _device.IPAddress;
    }

    public void SaveSettings()
    {
		_device.SaveSettings();
    }

    private void comboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
    {
		_device.Stop();
		Auto3DBaseDevice.IrPortName = comboBoxPort.SelectedItem.ToString();
		_device.Start();
    }

	private void checkBoxOther_CheckedChanged(object sender, EventArgs e)
	{
		Auto3DBaseDevice.AllowIrCommandsForAllDevices = checkAllowIRCommandsForOtherDevices.Checked;
	}

	private void checkBoxPingCheck_CheckedChanged(object sender, EventArgs e)
	{
		_device.PingCheck = checkBoxPingCheck.Checked;
		textBoxGenericIP.Enabled = _device.PingCheck;
	}

	private void textBoxGenericIP_TextChanged(object sender, EventArgs e)
	{
		_device.IPAddress = textBoxGenericIP.Text;
	}

	private void buttonPingGenericDevice_Click(object sender, EventArgs e)
	{
		if (_device.IsOn())
		{
			Auto3DHelpers.ShowAuto3DMessage("Ping was returned. TV seems to be on.", false, 0);
		}
		else
			Auto3DHelpers.ShowAuto3DMessage("Ping was not returned. TV seems to be off.", false, 0);
	}
  }
}
