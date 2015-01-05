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
  public enum eConnectionMethod { jointSpace, DirectFB };

  public class PhilipsTV : Auto3DBaseDevice
  {
    eConnectionMethod _connectionMethod = eConnectionMethod.jointSpace;

    public PhilipsTV()
    {
    }

    public override String CompanyName
    {
      get { return "Philips"; }
    }

    public override String DeviceName
    {
      get { return "Philips TV"; }
    }

    public String IPAddress
    {
      get;
      set;
    }

    public override void Start()
    {
      if (_connectionMethod == eConnectionMethod.DirectFB)
      {
        DiVine.Init(IPAddress);
      }
    }

    public override void Stop()
    {
      if (_connectionMethod == eConnectionMethod.DirectFB)
      {
        DiVine.Exit();
      }
    }

    public eConnectionMethod ConnectionMethod
    {
      get { return _connectionMethod; }
      set
      {
        if (value != _connectionMethod)
        {
          if (value == eConnectionMethod.DirectFB)
          {
            DiVine.Init(IPAddress);
          }
          else
          {
            if (DiVine.IsConnected)
              DiVine.Exit();
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
        IPAddress = reader.GetValueAsString("Auto3DPlugin", "PhilipsAddress", "0.0.0.0");
        _connectionMethod = (eConnectionMethod)reader.GetValueAsInt("Auto3DPlugin", "PhilipsConnectionMethod", (int)eConnectionMethod.jointSpace);
      }
    }

    public override void SaveSettings()
    {
      using (Settings writer = new MPSettings())
      {
        writer.SetValue("Auto3DPlugin", "PhilipsModel", SelectedDeviceModel.Name);
        writer.SetValue("Auto3DPlugin", "PhilipsAddress", IPAddress);
        writer.SetValue("Auto3DPlugin", "PhilipsConnectionMethod", (int)_connectionMethod);
      }
    }

    public override bool SendCommand(String command)
    {
      String address = "http://" + IPAddress + ":1925/1/input/key";

      switch (command)
      {
        case "Home":

          if (_connectionMethod == eConnectionMethod.DirectFB)
            DiVine.SendKeyEx(DiVine.RC6Codes.rc6S0MenuOn);
          else
            if (!PostRequest(address, "{ \"key\": \"Home\" }"))
              return false;
          break;

        case "Adjust":

          if (_connectionMethod == eConnectionMethod.DirectFB)
            DiVine.SendKeyEx(DiVine.RC6Codes.rc6S0AmbLightMode);
          else
            if (!PostRequest(address, "{ \"key\": \"Adjust\" }"))
              return false;
          break;

        case "Back":

          if (_connectionMethod == eConnectionMethod.DirectFB)
            DiVine.SendKeyEx(DiVine.RC6Codes.rc6S0PreviousProgram);
          else
            if (!PostRequest(address, "{ \"key\": \"Back\" }"))
              return false;
          break;

        case "Options":

          if (_connectionMethod == eConnectionMethod.DirectFB)
            DiVine.SendKeyEx(DiVine.RC6Codes.rc6S0Options);
          else
            if (!PostRequest(address, "{ \"key\": \"Options\" }"))
              return false;
          break;

        case "OK":

          if (_connectionMethod == eConnectionMethod.DirectFB)
            DiVine.SendKeyEx(DiVine.RC6Codes.rc6S0Acknowledge);
          else
            if (!PostRequest(address, "{ \"key\": \"Confirm\" }"))
              return false;
          break;

        case "CursorLeft":

          if (_connectionMethod == eConnectionMethod.DirectFB)
            DiVine.SendKeyEx(DiVine.RC6Codes.rc6S0StepLeft);
          else
            if (!PostRequest(address, "{ \"key\": \"CursorLeft\" }"))
              return false;
          break;

        case "CursorRight":

          if (_connectionMethod == eConnectionMethod.DirectFB)
            DiVine.SendKeyEx(DiVine.RC6Codes.rc6S0StepRight);
          else
            if (!PostRequest(address, "{ \"key\": \"CursorRight\" }"))
              return false;
          break;

        case "CursorUp":

          if (_connectionMethod == eConnectionMethod.DirectFB)
            DiVine.SendKeyEx(DiVine.RC6Codes.rc6S0StepUp);
          else
            if (!PostRequest(address, "{ \"key\": \"CursorUp\" }"))
              return false;
          break;

        case "CursorDown":

          if (_connectionMethod == eConnectionMethod.DirectFB)
            DiVine.SendKeyEx(DiVine.RC6Codes.rc6S0StepDown);
          else
            if (!PostRequest(address, "{ \"key\": \"CursorDown\" }"))
              return false;
          break;

        case "3D":

          if (_connectionMethod == eConnectionMethod.DirectFB)
            DiVine.SendKeyEx(DiVine.RC6Codes.rc6S0Display3D);
          else
            return false;
          break;

        case "Delay":

          // do nothing here
          break;

        case "Off":

          if (_connectionMethod == eConnectionMethod.DirectFB)
              DiVine.SendKeyEx(DiVine.RC6Codes.rc6S0SystemStandby);
          else
            if (!PostRequest(address, "{ \"key\": \"Standby\" }"))
              return false;
          break;
              
        default:

          Log.Info("Auto3D: Unknown command - " + command);
          break;
      }

      return true;
    }

    public bool PostRequest(String url, String jsonString)
    {
      try
      {
        Log.Debug("Auto3D: PostRequest to URL = \"" + url + "\"");

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        request.Timeout = 3000;
        request.ContentType = "text/json";
        request.Method = "POST";

        Log.Debug("Auto3D: JSON-String = \"" + jsonString + "\"");

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
          streamWriter.Write(jsonString);
          streamWriter.Flush();
          streamWriter.Close();
        }

        Application.DoEvents();
        Thread.Sleep(50);

        var httpResponse = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
          var result = streamReader.ReadToEnd();
          Log.Debug(result);
        }

        httpResponse.Close();

        Application.DoEvents();
      }
      catch (Exception ex)
      {
        Log.Info("Auto3D: PostRequest: " + ex.Message);
        Auto3DHelpers.ShowAuto3DMessage("Command to TV could not be sent: " + ex.Message, false, 0);
        return false;
      }

      return true;
    }

    public override bool CanTurnOff()
    {
        return true;
    }
  }
}
