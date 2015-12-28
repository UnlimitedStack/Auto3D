using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.ProcessPlugins.Auto3D;
using MediaPortal.Profile;
using System.Net;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using MediaPortal.GUI.Library;
using System.Reflection;
using MediaPortal.Configuration;
using System.IO.Ports;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public class GenericDevice : Auto3DBaseDevice
  {
	public GenericDevice()
    {
    }

    public override String CompanyName
    {
      get { return "Generic-IR"; }
    }

    public override String DeviceName
    {
		get { return "Generic TV or Beamer (IR-Toy)"; }
    }

	private String _ipAddress;

	public String IPAddress
	{
		get
		{
			return _ipAddress;
		}
		set
		{
			_ipAddress = value;

			String mac = Auto3DHelpers.RequestMACAddress(value);

			if (!mac.StartsWith("00-00-00"))
				MAC = mac;
		}
	}

	public String MAC
	{
		get;
		private set;
	}

	public bool PingCheck
	{
		get;
		set;
	}

    public override void Start()
    {
		base.Start();	
    }

    public override void Stop()
    {
		base.Stop();
    }

    public override void LoadSettings()
    {
		using (Settings reader = new MPSettings())
		{		
			IrPortName = reader.GetValueAsString("Auto3DPlugin", "GenericDevicePort", "None");
			AllowIrCommandsForAllDevices = reader.GetValueAsBool("Auto3DPlugin", "AllowIrCommandsForAllDevices", false);
			IPAddress = reader.GetValueAsString("Auto3DPlugin", "GenericDeviceIPAddress", "");
			MAC = reader.GetValueAsString("Auto3DPlugin", "GenericDeviceMAC", "00-00-00-00-00-00");
			PingCheck = reader.GetValueAsBool("Auto3DPlugin", "GenericDevicePingCheck", false);
		}
    }

    public override void SaveSettings()
    {
		using (Settings writer = new MPSettings())
		{
			writer.SetValue("Auto3DPlugin", "GenericDevicePort", IrPortName);
			writer.SetValueAsBool("Auto3DPlugin", "AllowIrCommandsForAllDevices", AllowIrCommandsForAllDevices);
			writer.SetValue("Auto3DPlugin", "GenericDeviceIPAddress", IPAddress);
			writer.SetValue("Auto3DPlugin", "GenericDeviceMAC", MAC);
			writer.SetValueAsBool("Auto3DPlugin", "GenericDevicePingCheck", PingCheck);
		}
    }

    public override bool SendCommand(RemoteCommand rc)
    {
		try
		{
			IrToy.Send(rc.IrCode);
			Log.Info("Auto3D: Code sent: " + rc.IrCode);
		}
		catch (Exception ex)
		{
			Auto3DHelpers.ShowAuto3DMessage("Sending code failed: " + ex.Message, false, 0);
			Log.Error("Auto3D: Sending code " + rc.IrCode + " failed: " + ex.Message);

			return false;
		}

      return true;
    }

	public override DeviceInterface GetTurnOffInterfaces()
	{
		return DeviceInterface.IR;
	}

	public override void TurnOff(DeviceInterface type)
	{
		if (PingCheck && !IsOn())
		{
			Log.Debug("Auto3D: TV is already off");
			return;
		}

		if (type == DeviceInterface.IR)
		{
			RemoteCommand rc = GetRemoteCommandFromString("Power (IR)");

			try
			{
				IrToy.Send(rc.IrCode);
			}
			catch (Exception ex)
			{
				Log.Error("Auto3D: IR Toy Send failed: " + ex.Message);
			}
		}
	}

	public override DeviceInterface GetTurnOnInterfaces()
	{
		return DeviceInterface.IR | DeviceInterface.Network;
	}

	public override void TurnOn(DeviceInterface type)
	{
		if (PingCheck && IsOn())
		{
			Log.Debug("Auto3D: TV is already on");
			return;
		}

		switch (type)
		{
			case DeviceInterface.IR:

				RemoteCommand rc = GetRemoteCommandFromString("Power (IR)");

				try
				{
					IrToy.Send(rc.IrCode);
				}
				catch (Exception ex)
				{
					Log.Error("Auto3D: IR Toy Send failed: " + ex.Message);
				}
				break;

			case DeviceInterface.Network:

				Auto3DHelpers.WakeOnLan(MAC);
				break;

			default:

				break;
		}
	}

	public override bool IsOn()
	{
		return Auto3DHelpers.Ping(IPAddress);
	}

	public override String GetMacAddress()
	{
		return MAC;
	}
  }
}
