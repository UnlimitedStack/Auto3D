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
  public class SonyTV : Auto3DUPnPBaseDevice
  {
    SonyAPI_Lib.SonyDevice sonyDevice;

    public SonyTV()
    {
        sonyDevice = new SonyAPI_Lib.SonyDevice();
    }

    public override String CompanyName
    {
      get { return "Sony"; }
    }

    public override String DeviceName
    {
      get { return "Sony TV"; }
    }

    public override String UPnPServiceName
    {
      get { return "urn:schemas-sony-com:service:IRCC:1"; }
    }

    public override String UPnPManufacturer
    {
      get { return ""; } // name is not necessary for service recognition
    }

    public String UDN
    {
      get;
      set;
    }

	public String MAC
	{
		get;
		set;
	}

    public String PairingKey
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
        DeviceModelName = reader.GetValueAsString("Auto3DPlugin", "SonyModel", "BRAVIA");
        UDN = reader.GetValueAsString("Auto3DPlugin", "SonyAddress", "");
        //PairingKey = reader.GetValueAsString("Auto3DPlugin", "SonyPairingKey", "");
		MAC = reader.GetValueAsString("Auto3DPlugin", "SonyMAC", "00-00-00-00-00-00");
      }
    }

    public override void SaveSettings()
    {
      using (Settings writer = new MPSettings())
      {
        writer.SetValue("Auto3DPlugin", "SonyModel", SelectedDeviceModel.Name);
        writer.SetValue("Auto3DPlugin", "SonyAddress", UDN);
        //writer.SetValue("Auto3DPlugin", "SonyPairingKey", PairingKey);
		writer.SetValue("Auto3DPlugin", "SonyMAC", MAC);
      }
    }

    public override void ServiceAdded(UPnPService service)
    {
      base.ServiceAdded(service);

      Log.Info("Auto3D: Sony service found -> " + service.ParentDevice.Manufacturer + ", " + service.ParentDevice.WebAddress.Host);

      if (service.ParentDevice.UDN == UDN)
      {
		MAC = Auto3DHelpers.RequestMACAddress(service.ParentDevice.WebAddress.Host);
        Log.Info("Auto3D: Sony service connected");
      }

      try
      {
          sonyDevice.initialize(service);
      }
      catch (Exception ex)
      {
          Log.Error("Auto3D: Initialize failed: " + ex.Message);        
      }

      // show on GUI that device is not registered!)
      
      if (!sonyDevice.Registered)
      {
        Log.Error("Auto3D: Device " + service.ParentDevice.FriendlyName + " is not registered");        
      }
      else
      {
         String CmdList = sonyDevice.get_remote_command_list();
         Log.Debug("Auto3D: Device " + service.ParentDevice.FriendlyName + " CmdList = " + CmdList);
      }

      ((SonyTVSetup)GetSetupControl()).SetRegisterButtonState(!sonyDevice.Registered);
    }

    public bool Register()
    {
        sonyDevice.register();

        if (sonyDevice.Registered)
        {
            String CmdList = sonyDevice.get_remote_command_list();
            Log.Debug("Auto3D: Device " + UPnPService.ParentDevice.FriendlyName + " CmdList = " + CmdList);
        }

        return sonyDevice.Registered;
    }

    public void SendPairingKey(String key)
    {
        if (sonyDevice.Generation == 3)
        {
            // Send PIN code to TV to create Authorization cookie            
           
            if (!sonyDevice.sendAuth(key))
            {                
                Log.Error("Auto3D: Device registration for " + UPnPService.ParentDevice.FriendlyName + " failed");
                MessageBox.Show("See log file for more information", "Device registration failed!");
            }
        }
    }

    public override void ServiceRemoved(UPnPService service)
    {
        Log.Info("Auto3D: Sony service removed");        
        base.ServiceRemoved(service);
    }

    public override bool SendCommand(RemoteCommand rc)
    {
      switch (rc.Command)
      {
        case "Mode3D":

          if (!InternalSendCommand("AAAAAgAAAHcAAABNAw=="))
            return false;
          break;

        case "Confirm":

          if (!InternalSendCommand("AAAAAQAAAAEAAABlAw=="))
            return false;
          break;

        case "Return":

          if (!InternalSendCommand("AAAAAgAAAJcAAAAjAw=="))
            return false;
          break;

        case "CursorUp":

          if (!InternalSendCommand("AAAAAQAAAAEAAAB0Aw=="))
            return false;
          break;

        case "CursorDown":

          if (!InternalSendCommand("AAAAAQAAAAEAAAB1Aw=="))
            return false;
          break;

        case "CursorLeft":

          if (!InternalSendCommand("AAAAAQAAAAEAAAA0Aw=="))
            return false;
          break;

        case "CursorRight":

          if (!InternalSendCommand("AAAAAQAAAAEAAAAzAw=="))
            return false;
          break;

        case "Off":

          if (!InternalSendCommand("AAAAAQAAAAEAAAAvAw=="))
            return false;
          break;	

        case "Delay":

          // do nothing here
          break;

        default:

          Log.Info("Auto3D: Unknown command - " + rc.Command);
          break;
      }

      return true;
    }

    private bool InternalSendCommand(String command)
    {
      // we must use special send command instead of UPnPLib.Invoke
      // 

        try
        {
            sonyDevice.send_ircc(command);
        }
        catch (Exception ex)
        {
            Log.Info("Auto3D: InternalSendCommand - " + ex.Message);
        }

        return true;
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
			Log.Debug("Auto3D: TV is already off");
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
