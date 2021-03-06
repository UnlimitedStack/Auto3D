﻿using System;
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
using MediaPortal.ProcessPlugins.Auto3D.Samsung.iRemoteWrapper;
using MediaPortal.ProcessPlugins.Auto3D.Devices.Samsung.iRemoteWrapper;
using MediaPortal.Configuration;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  class SamsungTV : Auto3DBaseDevice
  {
    iRemote _iRemote = null;

    public SamsungTV()
    {
    }

    public override String CompanyName
    {
      get { return "Samsung"; }
    }

    public override String DeviceName
    {
      get { return "Samsung TV - (Series 6 and above)"; }
    }

    public String IPAddress
    {
      get;
      set;
    }

	public String MAC
	{
		get;
		set;
	}

    private void Connect()
    {
        if (_iRemote == null)
        {
            _iRemote = new iRemote();
            _iRemote.addTVEvent += new Auto3D.Samsung.iRemoteWrapper.iRemote.AddTVEventHandler(_iRemote_addTVEvent);
            _iRemote.removeTVEvent += new Auto3D.Samsung.iRemoteWrapper.iRemote.RemoveTVEventHandler(iRemote_removeTVEvent);
        }
    }

    private void Disconnect()
    {
        if (_iRemote != null)
        {
            _iRemote.addTVEvent -= _iRemote_addTVEvent;
            _iRemote.removeTVEvent -= iRemote_removeTVEvent;
            _iRemote.disconnect();
            _iRemote = null;
        }
    }

    public override void Start()
    {
	  base.Start();
      Connect();  
    }

    public override void Stop()
    {
	   base.Stop();
       Disconnect();     
    }

    public override void Suspend()
    {
        Disconnect();
    }

    public override void Resume()
    {
        Connect();
    }

    public override void LoadSettings()
    {
      using (Settings reader = new MPSettings())
      {
        DeviceModelName = reader.GetValueAsString("Auto3DPlugin", "SamsungModel", "UE55D6500");
        IPAddress = reader.GetValueAsString("Auto3DPlugin", "SamsungAddress", "");
		MAC = reader.GetValueAsString("Auto3DPlugin", "SamsungMAC", "00-00-00-00-00-00");
      }
    }

    public override void SaveSettings()
    {
      using (Settings writer = new MPSettings())
      {
        writer.SetValue("Auto3DPlugin", "SamsungModel", SelectedDeviceModel.Name);
        writer.SetValue("Auto3DPlugin", "SamsungAddress", IPAddress);
		writer.SetValue("Auto3DPlugin", "SamsungMAC", MAC);
      }
    }

    void _iRemote_addTVEvent(TVInfo info)
    {
      ((SamsungTVSetup)GetSetupControl()).TVAdded(ref info);

      if (info.ToString() == IPAddress)
      {
		MAC = Auto3DHelpers.RequestMACAddress(IPAddress);

        if (iRemote.ToString() != info.ToString())
          _iRemote.ConnectTo(info);
      }
    }

    void iRemote_removeTVEvent(TVInfo info)
    {
      ((SamsungTVSetup)GetSetupControl()).TVRemoved(ref info);
    }

    public iRemote iRemote
    {
      get { return _iRemote; }
    }

    public override bool SendCommand(RemoteCommand rc)
    {
      switch (rc.Command)
      {
        case "REMOCON_MENU":

          iRemote.SendRemocon(REMOCONCODE.REMOCON_MENU, REMOCON_TYPE.REMOCON_TYPE_NORMAL);
          break;

        case "REMOCON_3D":

          iRemote.SendRemocon(REMOCONCODE.REMOCON_3D, REMOCON_TYPE.REMOCON_TYPE_NORMAL);
          break;

        case "REMOCON_RETURN":

          iRemote.SendRemocon(REMOCONCODE.REMOCON_RETURN, REMOCON_TYPE.REMOCON_TYPE_NORMAL);
          break;

        case "REMOCON_EXIT":

          iRemote.SendRemocon(REMOCONCODE.REMOCON_EXIT, REMOCON_TYPE.REMOCON_TYPE_NORMAL);
          break;

        case "REMOCON_ENTER":

          iRemote.SendRemocon(REMOCONCODE.REMOCON_ENTER, REMOCON_TYPE.REMOCON_TYPE_NORMAL);
          break;

        case "REMOCON_LEFT":

          iRemote.SendRemocon(REMOCONCODE.REMOCON_LEFT, REMOCON_TYPE.REMOCON_TYPE_NORMAL);
          break;

        case "REMOCON_RIGHT":

          iRemote.SendRemocon(REMOCONCODE.REMOCON_RIGHT, REMOCON_TYPE.REMOCON_TYPE_NORMAL);
          break;

        case "REMOCON_UP":

          iRemote.SendRemocon(REMOCONCODE.REMOCON_UP, REMOCON_TYPE.REMOCON_TYPE_NORMAL);
          break;

        case "REMOCON_DOWN":

          iRemote.SendRemocon(REMOCONCODE.REMOCON_DOWN, REMOCON_TYPE.REMOCON_TYPE_NORMAL);
          break;

        case "Off":

          iRemote.SendRemocon(REMOCONCODE.REMOCON_POWER, REMOCON_TYPE.REMOCON_TYPE_NORMAL);
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
		return Auto3DHelpers.Ping(_iRemote.GetCurrentTV().Ip);		
	}

	public override String GetMacAddress()
	{
		return MAC;
	}
  }
}
