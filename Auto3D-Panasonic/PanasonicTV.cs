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
using System.Diagnostics;
using MediaPortal.ProcessPlugins.Auto3D.UPnP;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  class PanasonicTV : Auto3DUPnPBaseDevice
  {
    public PanasonicTV()
    {
    }

    public override String CompanyName
    {
      get { return "Panasonic"; }
    }

    public override String DeviceName
    {
      get { return "Panasonic TV"; }
    }

    public override String UPnPServiceName
    {
      get { return ":p00NetworkControl"; }
    }

    public override String UPnPManufacturer
    {
      get { return "Panasonic"; } 
    }

    public String UDN
    {
      get;
      set;
    }

	public String MAC
	{
		get;
		private set;
	}

    public override void Start()
    {
		base.Start();
    }

    public override void Stop()
    {
		base.Stop();
    }

    public override void Suspend()
    {
        
    }

    public override void Resume()
    {
     
    }

    public override void LoadSettings()
    {
      using (Settings reader = new MPSettings())
      {
        DeviceModelName = reader.GetValueAsString("Auto3DPlugin", "PanasonicModel", "VIERA");
        UDN = reader.GetValueAsString("Auto3DPlugin", "PanasonicAddress", "");
		MAC = reader.GetValueAsString("Auto3DPlugin", "PanasonicMAC", "00-00-00-00-00-00");
      }
    }

    public override void SaveSettings()
    {
      using (Settings writer = new MPSettings())
      {
        writer.SetValue("Auto3DPlugin", "PanasonicModel", SelectedDeviceModel.Name);
        writer.SetValue("Auto3DPlugin", "PanasonicAddress", UDN);
		writer.SetValue("Auto3DPlugin", "PanasonicMAC", MAC);
      }
    }

    public override void ServiceAdded(UPnPService service)
    {
      base.ServiceAdded(service);

      Log.Info("Auto3D: Panasonic service found -> " + service.ParentDevice.Manufacturer + ", " + service.ParentDevice.WebAddress.Host);

      if (service.ParentDevice.UDN == UDN)
      {
		  MAC = Auto3DHelpers.RequestMACAddress(service.ParentDevice.WebAddress.Host);
		  Log.Info("Auto3D: Panasonic service connected");
      }		
    }

    public override void ServiceRemoved(UPnPService service)
    {
      Log.Info("Auto3D: Panasonic service removed");
      base.ServiceRemoved(service);
    }

    public override bool SendCommand(RemoteCommand rc)
    {
      switch (rc.Command)
      {
        case "Mode3D":

          if (!InternalSendCommand("NRC_3D-ONOFF"))
            return false;
          break;

        case "Confirm":

          if (!InternalSendCommand("NRC_ENTER-ONOFF"))
            return false;
          break;

        case "CursorUp":

          if (!InternalSendCommand("NRC_UP-ONOFF"))
            return false;
          break;

        case "CursorDown":

          if (!InternalSendCommand("NRC_DOWN-ONOFF"))
            return false;
          break;

        case "CursorLeft":

          if (!InternalSendCommand("NRC_LEFT-ONOFF"))
            return false;
          break;

        case "CursorRight":

          if (!InternalSendCommand("NRC_RIGHT-ONOFF"))
            return false;
          break;

        case "Red":

          if (!InternalSendCommand("NRC_RED-ONOFF"))
            return false;
          break;

        case "Delay":

          // do nothing here
          break;

        case "Off":

          if (!InternalSendCommand("NRC_POWER-ONOFF"))
            return false;
          break;
	
        default:

          Log.Info("Auto3D: Unknown command - " + rc.Command);
          break;
      }

      return true;
    }

    private bool InternalSendCommand(String command)
    {
      if (UPnPService != null)
      {
        UPnPService.InvokeAction("X_SendKey", "X_KeyEvent", command);
        return true;
      }
      else
        return false;
    }

	public override DeviceInterface GetTurnOffInterfaces()
	{
		DeviceInterface irDevice = (AllowIrCommandsForAllDevices && Auto3DBaseDevice.IsIrConnected()) ? DeviceInterface.IR : DeviceInterface.None;
		return irDevice | DeviceInterface.Network;
	}

	public override void TurnOff(DeviceInterface type)
	{
		if (IsOn())
		{
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

					SendCommand(new RemoteCommand("Off", 0, null));
					break;

				default:

					break;
			}
		}
		else
			Log.Debug("Auto3D: TV is already off");
	}

	public override DeviceInterface GetTurnOnInterfaces()
	{
		DeviceInterface irDevice = (AllowIrCommandsForAllDevices && Auto3DBaseDevice.IsIrConnected()) ? DeviceInterface.IR : DeviceInterface.None;		
		return irDevice | DeviceInterface.Network;
	}

	public override void TurnOn(DeviceInterface type)
	{
		if (!IsOn())
		{
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
		else
			Log.Debug("Auto3D: TV is already on");
	}

	public override bool IsOn()
	{
		return Auto3DHelpers.Ping(UPnPService.ParentDevice.WebAddress.Host);		
	}

	public override String GetMacAddress()
	{
		return MAC;
	}
  }
}
