using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using MediaPortal.Profile;
using MediaPortal.GUI.Library;
using System.Threading;
using System.Reflection;
using MediaPortal.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public abstract class Auto3DBaseDevice : IAuto3D
  {
    String _docName;
    XmlDocument _deviceDoc = new XmlDocument();

    List<RemoteCommand> _remoteCommands = new List<RemoteCommand>();
    List<Auto3DDeviceModel> _deviceModels = new List<Auto3DDeviceModel>();

    Auto3DDeviceModel _deviceModel = null;

    UserControl _setupControl = null;
    UserControl _remoteKeyPad = null;

    bool bSendEventGhostEvents = false;
    bool bShowMessageOnModeChange = false;

    public Auto3DBaseDevice()
    {
      using (Settings reader = new MPSettings())
      {
        bSendEventGhostEvents = reader.GetValueAsBool("Auto3DPlugin", "EventGhostEvents", false);
        bShowMessageOnModeChange = reader.GetValueAsBool("Auto3DPlugin", "ShowMessageOnModeChange", false);
      }

      if (DeviceName != "No device")
      {
        String baseDir = Config.GetFolder(Config.Dir.Config);
        _docName = Path.Combine(baseDir, "Auto3D\\" + CompanyName + ".xml");

        _deviceDoc.Load(_docName);

        // get device node

        XmlNode deviceNode = _deviceDoc.GetElementsByTagName("Device").Item(0);

        // read commands

        XmlNode commandsNode = _deviceDoc.GetElementsByTagName("Commands").Item(0);

        foreach (XmlNode node in commandsNode.ChildNodes)
        {
          _remoteCommands.Add(new RemoteCommand(node));
        }

        // read models

        XmlNodeList deviceModels = _deviceDoc.GetElementsByTagName("Model");

        foreach (XmlNode node in deviceModels)
        {
          _deviceModels.Add(new Auto3DDeviceModel(node));
        }
      }
      else
      {
        _deviceModel = new Auto3DDeviceModel();
      }

      LoadSettings();

      Type[] types = this.GetType().Assembly.GetTypes();

      foreach (Type type in types)
      {
        if (type.GetInterface("IAuto3DSetup") != null || type.GetInterface("IAuto3DUPnPSetup") != null)
        {
          _setupControl = (UserControl)Activator.CreateInstance(type, this);
        }
        else
          if (type.BaseType != null && type.BaseType.Name == "UserControl")
          {
            _remoteKeyPad = (UserControl)Activator.CreateInstance(type);

            IAuto3DKeypad iKeyPad = _remoteKeyPad as IAuto3DKeypad;

            if (iKeyPad != null)
            {
              iKeyPad.SetDevice(this);
            }
          }
      }
    }

    public bool Modified
    {
      get;
      set;
    }

    public List<RemoteCommand> RemoteCommands
    {
      get
      {
        return _remoteCommands;
      }
    }

    public List<Auto3DDeviceModel> DeviceModels
    {
      get { return _deviceModels; }
    }

    public abstract String CompanyName
    {
      get;
    }

    public abstract String DeviceName
    {
      get;
    }

    public Auto3DDeviceModel SelectedDeviceModel
    {
      get
      {
        return _deviceModel;
      }

      set
      {
        _deviceModel = value;
      }
    }

    public String DeviceModelName
    {
      get
      {
        return _deviceModel.Name;
      }

      set
      {
        foreach (Auto3DDeviceModel model in _deviceModels)
        {
          if (model.Name == value)
          {
            SelectedDeviceModel = model;
            return;
          }
        }

        SelectedDeviceModel = _deviceModels[0];
      }
    }

    public bool IsDefined(VideoFormat fmt)
    {
      switch (fmt)
      {
        case VideoFormat.Fmt3DSBS:

          return SelectedDeviceModel.RemoteCommandSequences.Commands2D3DSBS.Count > 0;

        case VideoFormat.Fmt3DTAB:

          return SelectedDeviceModel.RemoteCommandSequences.Commands2D3DTAB.Count > 0;

        case VideoFormat.Fmt2D3D:

          return SelectedDeviceModel.RemoteCommandSequences.Commands2D3D.Count > 0;
      }

      return false;
    }

    public UserControl GetSetupControl()
    {
      return _setupControl;
    }

    public UserControl GetRemoteControl()
    {
      return _remoteKeyPad;
    }

    public virtual void Start()
    {
    }

    public virtual void Stop()
    {
    }

    public virtual void Suspend()
    {
    }

    public virtual void Resume()
    {
    }

    public virtual void LoadSettings()
    {
    }

    public virtual void SaveSettings()
    {
    }

    public void SaveDocument()
    {
      // write back command timings to document

      XmlNode commandsNode = _deviceDoc.GetElementsByTagName("Commands").Item(0);

      foreach (XmlNode node in commandsNode.ChildNodes)
      {
        RemoteCommand rc = GetRemoteCommandFromString(node.ChildNodes.Item(0).InnerText);
        node.ChildNodes.Item(1).InnerText = rc.Delay.ToString();
      }

      // write document to file

      _deviceDoc.Save(_docName);
    }

    private void SendEventGhostEvent(VideoFormat format)
    {
      var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
      using (var searcher = new ManagementObjectSearcher(wmiQueryString))
      using (var results = searcher.Get())
      {
        var query = from p in Process.GetProcesses()
                    join mo in results.Cast<ManagementObject>()
                    on p.Id equals (int)(uint)mo["ProcessId"]
                    select new
                    {
                      Process = p,
                      Path = (string)mo["ExecutablePath"],
                      CommandLine = (string)mo["CommandLine"],
                    };
        foreach (var item in query)
        {
          if (item.Process.ProcessName.Contains("EventGhost"))
          {
            Process p = new Process();
            p.StartInfo.FileName = item.Path;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.Arguments = "-event Auto3D." + format.ToString();
            p.Start();
            return;
          }
        }
      }
    }

    public virtual void BeforeSequence()
    {
    }

    public bool SwitchFormat(VideoFormat fmtOld, VideoFormat fmtNew)
    {
      if (GUIGraphicsContext.IsFullScreenVideo && bShowMessageOnModeChange)
      {

        String format = "";

        switch (fmtNew)
        {
          case VideoFormat.Fmt2D:

            switch (GUIGraphicsContext.Render3DMode)
            {
              case GUIGraphicsContext.eRender3DMode.None:
              case GUIGraphicsContext.eRender3DMode.SideBySide:
              case GUIGraphicsContext.eRender3DMode.TopAndBottom:

                format = "2D";
                break;

              case GUIGraphicsContext.eRender3DMode.SideBySideTo2D:

                format = "3D SBS -> 2D via MediaPortal";
                break;

              case GUIGraphicsContext.eRender3DMode.TopAndBottomTo2D:

                format = "3D TAB -> 2D via MediaPortal";
                break;
            }
            break;

          case VideoFormat.Fmt3DSBS:

            format = "3D Side by Side";

            if (GUIGraphicsContext.Switch3DSides)
              format += " Reverse";
            break;

          case VideoFormat.Fmt3DTAB:

            format = "3D Top and Bottom";

            if (GUIGraphicsContext.Switch3DSides)
              format += " Reverse";
            break;

          case VideoFormat.Fmt2D3D:

            format = "2D -> 3D via TV";
            break;
        }

        Auto3DHelpers.ShowAuto3DMessage("VideoFormat: " + format, true, 4);
      }   

      Log.Info("Auto3D: Begin SwitchToFormat");

      try
      {
        switch (fmtNew)
        {
          case VideoFormat.Fmt3DSBS:

            if (!SendCommandList(SelectedDeviceModel.RemoteCommandSequences.Commands2D3DSBS))
              return false;
            break;

          case VideoFormat.Fmt3DTAB:

            if (!SendCommandList(SelectedDeviceModel.RemoteCommandSequences.Commands2D3DTAB))
              return false;
            break;

          case VideoFormat.Fmt2D3D:

            if (!SendCommandList(SelectedDeviceModel.RemoteCommandSequences.Commands2D3D))
              return false;
            break;

          case VideoFormat.Fmt2D:

            switch (fmtOld)
            {
              case VideoFormat.Fmt3DSBS:

                if (!SendCommandList(SelectedDeviceModel.RemoteCommandSequences.Commands3DSBS2D))
                  return false;
                break;

              case VideoFormat.Fmt3DTAB:

                if (!SendCommandList(SelectedDeviceModel.RemoteCommandSequences.Commands3DTAB2D))
                  return false;
                break;

              case VideoFormat.Fmt2D3D:

                if (!SendCommandList(SelectedDeviceModel.RemoteCommandSequences.Commands3D2D))
                  return false;
                break;
            }

            break;
        }

        if (bSendEventGhostEvents)
          SendEventGhostEvent(fmtNew);

        return true;
      }
      catch (Exception ex)
      {
        Log.Info("Auto3D: " + ex.Message);
        return false;
      }
      finally
      {
        Log.Info("Auto3D: End SwitchToFormat");
      }
    }

    private bool SendCommandList(List<String> list)
    {
      BeforeSequence();

      foreach (String command in list)
      {
        Log.Info("Auto3D: Send Command " + command);

        if (!SendCommand(command))
          return false;

        RemoteCommand rc = GetRemoteCommandFromString(command);
        Thread.Sleep(rc.Delay);
      }

      return true;
    }

    public RemoteCommand GetRemoteCommandFromString(String command)
    {
      foreach (RemoteCommand rc in _remoteCommands)
      {
        if (rc.Command == command)
        {
          return rc;
        }
      }

      return null;
    }

    public virtual bool SendCommand(String command)
    {
      return false;
    }

    public virtual bool CanTurnOff()
    {
        return false;
    }

    public virtual void TurnOff()
    {
        SendCommand("Off");
    }

    public override String ToString()
    {
      return DeviceName;
    }
  }
}


