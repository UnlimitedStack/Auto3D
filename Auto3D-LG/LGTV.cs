using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using MediaPortal.ProcessPlugins.Auto3D.UPnP;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public class LGTV : Auto3DUPnPBaseDevice
  {
    public LGTV()
    {
    }

    public override String CompanyName
    {
      get { return "LG"; }
    }

    public override String DeviceName
    {
      get { return "LG TV"; }
    }

    public String IPAddress
    {
      get;
      set;
    }

    public override String UPnPServiceName
    {
      get { return "urn:schemas-upnp-org:service:ConnectionManager:1"; }
    }

    public override String UPnPManufacturer
    {
      get { return "LG Electronics"; } // name is necessary for service recognition
    }

    public String UDN
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
      SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
    }

    public override void Stop()
    {
      SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
    }

    public override void LoadSettings()
    {
      using (Settings reader = new MPSettings())
      {
        DeviceModelName = reader.GetValueAsString("Auto3DPlugin", "LGModel", "LG");
        UDN = reader.GetValueAsString("Auto3DPlugin", "LGAddress", "");
        PairingKey = reader.GetValueAsString("Auto3DPlugin", "LGPairingKey", "");

        switch (DeviceModelName)
        {
          case "LG TV 2011":

            UDAPnP.Protocol = UDAPnP.LGProtocol.LG2011;
            break;

          case "LG TV 2012/2013":

            UDAPnP.Protocol = UDAPnP.LGProtocol.LG2012x;
            break;
        }
      }
    }

    public override void SaveSettings()
    {
      using (Settings writer = new MPSettings())
      {
        writer.SetValue("Auto3DPlugin", "LGModel", SelectedDeviceModel.Name);
        writer.SetValue("Auto3DPlugin", "LGAddress", UDN);
        writer.SetValue("Auto3DPlugin", "LGPairingKey", PairingKey);
      }
    }

    public override void ServiceAdded(UPnPService service)
    {
      Log.Info("Auto3D: LG service found -> " + service.ParentDevice.Manufacturer + ", " + service.ParentDevice.WebAddress.Host + ", " + service.ParentDevice.UDN + ", " + UDAPnP.Protocol);

      base.ServiceAdded(service);

      if (!ConnectAndPair())
        return;

      if (service.ParentDevice.UDN == UDN)
      {
        Log.Info("Auto3D: LG service connected!");
      }
    }

    public override void ServiceRemoved(UPnPService service)
    {
      Log.Info("Auto3D: LG service removed");   
      base.ServiceRemoved(service);      
    }

    void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
      if (e.Mode == PowerModes.Resume)
      {
        Log.Info("Auto3D: LG resume from sleep");
        ConnectAndPair();
      }
    }

    public override void BeforeSequence()
    {
      // This is a fix for 2011 TV, because there is
      // sometimes a connection problems after a PC
      // suspend/resume. Sending an exit command before
      // every sequence seems to fix the problem.
      // Not nice, but effective :)

      if (UDAPnP.Protocol == UDAPnP.LGProtocol.LG2011)
      {
        SendCommand("Exit");
        RemoteCommand rc = GetRemoteCommandFromString("Exit");
        Thread.Sleep(rc.Delay);
      }
    }

    public override bool SendCommand(String command)
    {
      switch (command)
      {
        case "Home":

          if (UDAPnP.Protocol == UDAPnP.LGProtocol.LG2012x)
          {
            if (!InternalSendCommand("21"))
              return false;
          }
          else
          {
            if (!InternalSendCommand("67"))
              return false;
          }
          break;

        case "Back":

          if (UDAPnP.Protocol == UDAPnP.LGProtocol.LG2012x)
          {
            if (!InternalSendCommand("412"))
              return false;
          }
          else
          {
            if (!InternalSendCommand("40"))
              return false;
          }
          break;


        case "OK":

          if (UDAPnP.Protocol == UDAPnP.LGProtocol.LG2012x)
          {
            if (!InternalSendCommand("20"))
              return false;
          }
          else
          {
            if (!InternalSendCommand("68"))
              return false;
          }
          break;

        case "CursorLeft":

          if (UDAPnP.Protocol == UDAPnP.LGProtocol.LG2012x)
          {
            if (!InternalSendCommand("14"))
              return false;
          }
          else
          {
            if (!InternalSendCommand("7"))
              return false;
          }
          break;

        case "CursorRight":

          if (UDAPnP.Protocol == UDAPnP.LGProtocol.LG2012x)
          {
            if (!InternalSendCommand("15"))
              return false;
          }
          else
          {
            if (!InternalSendCommand("6"))
              return false;
          }
          break;

        case "CursorUp":

          if (UDAPnP.Protocol == UDAPnP.LGProtocol.LG2012x)
          {
            if (!InternalSendCommand("12"))
              return false;
          }
          else
          {
            if (!InternalSendCommand("64"))
              return false;
          }
          break;

        case "CursorDown":

          if (UDAPnP.Protocol == UDAPnP.LGProtocol.LG2012x)
          {
            if (!InternalSendCommand("13"))
              return false;
          }
          else
          {
            if (!InternalSendCommand("65"))
              return false;
          }
          break;

        case "Mode3D":

          if (UDAPnP.Protocol == UDAPnP.LGProtocol.LG2012x)
          {
            if (!InternalSendCommand("400"))
              return false;
          }
          else
          {
            if (!InternalSendCommand("220"))
              return false;
          }
          break;

        case "Exit":

          if (UDAPnP.Protocol == UDAPnP.LGProtocol.LG2012x)
          {
            if (!InternalSendCommand("412"))
              return false;
          }
          else
          {
            if (!InternalSendCommand("91"))
              return false;
          }
          break;

        case "Delay":

          // do nothing here
          break;

        default:

          Log.Info("Auto3D: Unknown command - " + command);
          break;
      }

      return true;
    }

    internal bool ConnectAndPair()
    {
      // LG UPnP stack is sometimes very slow in answering,
      // so wait up to 5 seconds before giving up pairing.

      int loops = 0;

      while (IPAddress == null || IPAddress.Length == 0)
      {
        Thread.Sleep(5);

        if (loops++ > 100)
        {
          Log.Error("Auto3D: LG service not connected withing additional 10 seconds!");
          return false;
        }
      }

      if (UDAPnP.UpdateServiceInformation(IPAddress))
      {
        Log.Info("Auto3D: UpdateServiceInfo OK");

        if (PairingKey.Length > 0)
        {
          if (UDAPnP.RequestPairing(PairingKey))
          {
            Log.Info("Auto3D: Pairing LG client with key {0} succeeded", PairingKey);
          }
          else
          {
            Log.Error("Auto3D: Pairing LG client with key {0} failed!", PairingKey);
            return false;
          }
        }
      }
      else
      {
        Log.Error("Auto3D: UpdateServiceInfo failed!");
        return false;
      }

      return true;
    }

    private bool InternalSendCommand(String command)
    {
      //if (IPAddress == null || IPAddress.Length == 0)
      //ConnectAndPair();

      if (!UDAPnP.HandleKeyInput(command))
      {
        // if for some reason connection was lost, try to reconnect

        if (!ConnectAndPair())
        {
            Auto3DHelpers.ShowAuto3DMessage("Connection to LG TV failed!", false, 0);             
            Log.Error("Auto3D: Connection to LG TV failed!");

          return false;
        }

        // second try to send the command

        if (!UDAPnP.HandleKeyInput(command))
        {
          Log.Error("Auto3D: HandleKeyInput failed for command: " + command);
          return false;
        }
      }

      return true;
    }
  }
}
