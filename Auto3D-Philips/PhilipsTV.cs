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

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public enum eConnectionMethod { jointSpaceV1, jointSpaceV5, DirectFB }; 

  public class PhilipsTV : Auto3DBaseDevice
  {
    private eConnectionMethod _connectionMethod = eConnectionMethod.jointSpaceV1;
    private IPhilipsTVAdapter _currentAdapter;
    private static readonly IPhilipsTVAdapter _divineAdapter = new DiVineAdapter();
    private static readonly IPhilipsTVAdapter _jointSpaceV1Adapter = new JointSpaceV1Adapter();
    private static readonly IPhilipsTVAdapter _jointSpaceV5Adapter = new JointSpaceV5Adapter();

    public PhilipsTV()
    {
      if (_currentAdapter == null)
      {
        _currentAdapter = _jointSpaceV1Adapter;
      }
    }

    public override String CompanyName
    {
      get { return "Philips"; }
    }

    public override String DeviceName
    {
      get { return "Philips TV"; }
    }

    public string IpAddress
    {
      get;
      set;
    }

    public string Mac
    {
      get;
      internal set;
    }

    public SystemBase Test()
    {
      if (_currentAdapter != null)
      {
	try
	{
	  return _currentAdapter.TestConnection(IpAddress);
	}
	catch (Exception)
	{
	  return null;
	}
      }

      return null;
    }

    private void Connect()
    {
      if (_currentAdapter != null) 
      {
	_currentAdapter.Connect(IpAddress); 
      }
    }

    private void Disconnect()
    {
      if (_currentAdapter != null) 
      {
	_currentAdapter.Disconnect(); 
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

    public eConnectionMethod ConnectionMethod
    {
      get { return _connectionMethod; }
      set
      {
        if (value != _connectionMethod)
        {
	  Disconnect();

	  switch (value)
	  {
	  case eConnectionMethod.DirectFB:
	    _currentAdapter = _divineAdapter;
	    break;
	  case eConnectionMethod.jointSpaceV1:
	    _currentAdapter = _jointSpaceV1Adapter;
	    break;
	  case eConnectionMethod.jointSpaceV5:
	    _currentAdapter = _jointSpaceV5Adapter;
	    break;
	  }

          _connectionMethod = value;
        }
      }
    }

    public override void LoadSettings()
    {
      using (Settings reader = new MPSettings())
      {
        DeviceModelName = reader.GetValueAsString("Auto3DPlugin", CompanyName + "Model", "55PFL7606K-02");
        IpAddress = reader.GetValueAsString("Auto3DPlugin", "PhilipsAddress", "0.0.0.0");
	Mac = reader.GetValueAsString("Auto3DPlugin", "PhilipsMAC", "00-00-00-00-00-00");
	ConnectionMethod = (eConnectionMethod)reader.GetValueAsInt("Auto3DPlugin", "PhilipsConnectionMethod", (int)eConnectionMethod.jointSpaceV1);
      }
    }

    public override void SaveSettings()
    {
      using (Settings writer = new MPSettings())
      {
        writer.SetValue("Auto3DPlugin", "PhilipsModel", SelectedDeviceModel.Name);
        writer.SetValue("Auto3DPlugin", "PhilipsAddress", IpAddress);
        writer.SetValue("Auto3DPlugin", "PhilipsConnectionMethod", (int)_connectionMethod);
	writer.SetValue("Auto3DPlugin", "PhilipsMAC", Mac);
      }
    }

    public override bool SendCommand(RemoteCommand rc)
    {
      switch (rc.Command)
      {
      case "Power (IR)":

	base.SendCommand(rc);
	break;

      case "On":

	if (!IsOn())
	  Auto3DHelpers.WakeOnLan(Auto3DHelpers.RequestMACAddress(IpAddress));
	break;

      default:

	if (_currentAdapter != null)
	{
	  return _currentAdapter.SendCommand(rc.Command);
	}
	break;
      }

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

	  Auto3DHelpers.WakeOnLan(Mac);
	  break;
	}
      }
      else
	Log.Debug("Auto3D: TV is already on");
    }

    public override bool IsOn()
    {
      return Auto3DHelpers.Ping(IpAddress);		
    }

    public override String GetMacAddress()
    {
      return Mac;
    }
  }
}
