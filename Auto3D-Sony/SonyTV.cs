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
    public SonyTV()
    {
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

    /*public String PairingKey
    {
      get;
      set;
    }*/

    public override void Start()
    {
    }

    public override void Stop()
    {
    }

    public override void LoadSettings()
    {
      using (Settings reader = new MPSettings())
      {
        DeviceModelName = reader.GetValueAsString("Auto3DPlugin", "SonyModel", "BRAVIA");
        UDN = reader.GetValueAsString("Auto3DPlugin", "SonyAddress", "");
        //PairingKey = reader.GetValueAsString("Auto3DPlugin", "SonyPairingKey", "");
      }
    }

    public override void SaveSettings()
    {
      using (Settings writer = new MPSettings())
      {
        writer.SetValue("Auto3DPlugin", "SonyModel", SelectedDeviceModel.Name);
        writer.SetValue("Auto3DPlugin", "SonyAddress", UDN);
        //writer.SetValue("Auto3DPlugin", "SonyPairingKey", PairingKey);
      }
    }

    public override void ServiceAdded(UPnPService service)
    {
      base.ServiceAdded(service);

      Log.Info("Auto3D: Sony service found -> " + service.ParentDevice.Manufacturer + ", " + service.ParentDevice.WebAddress.Host);

      if (service.ParentDevice.UDN == UDN)
      {
        Log.Info("Auto3D: Sony service connected");
      }
    }

    public override void ServiceRemoved(UPnPService service)
    {
      base.ServiceRemoved(service);
    }

    public override bool SendCommand(String command)
    {
      switch (command)
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

        default:

          Log.Info("Auto3D: Unknown command - " + command);
          break;
      }

      return true;
    }

    private bool InternalSendCommand(String command)
    {
      UPnPService.InvokeAction("X_SendIRCC", "IRCCCode", command);
      return true;
    }

    /*public void RegisterClient(String ip)
    {
      String registrationUrl = "http://" + ip + "/cers/api/register?name=DeviceName&registrationType=initial&deviceId=MediaRemote";

      Log.Info("Auto3D: Register Sony client");
      Log.Info("Auto3D: " + registrationUrl);

      HttpWebRequest request = null;

      try
      {
        request = (HttpWebRequest)WebRequest.Create(registrationUrl);

        request.Headers.Add("X-CERS-DEVICE-INFO", "MediaPortal");
        request.KeepAlive = false;
        request.Proxy = null;
        request.Timeout = 30000;    // There is a 30 second countdown running on the TV side if client is registering for the first time.            
        // User needs to click OK and confirm before remote commands are accepted.
      }
      catch (System.Exception ex)
      {
        Log.Info("Auto3D: Register Sony client failed! Error: " + ex.Message);
        ShowMessageBoxFromNonUIThread("Registration at Sony TV failed!\nError: " + ex.Message);
        return;
      }

      try
      {
        using (WebResponse response = (HttpWebResponse)request.GetResponse()) { }
      }
      catch (Exception ex) // TV will respond with 40X code if registering fails
      {
        Log.Info("Auto3D: Register Sony client failed! Error: " + ex.Message);
        MessageBox.Show("Registration at Sony TV failed!\nError: " + ex.Message, "Auto3D");
        return;
      }

      ShowMessageBoxFromNonUIThread("Registration at Sony TV succeeded!");
    }

    public void RequestPin(String ip)
    {
      string hostname = System.Windows.Forms.SystemInformation.ComputerName;
      string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":11c43119-af3d-40e7-b1b2-743311375322c\",\"nickname\":\"" + hostname + " (Auto3D)\"},[{\"clientid\":\"" + hostname + ":11c43119-af3d-40e7-b1b2-743311375322c\",\"value\":\"yes\",\"nickname\":\"" + hostname + " (Auto3D)\",\"function\":\"WOL\"}]]}";

      PostRequest("http://" + ip + "/sony/accessControl", jsontosend, null);
    }

    public void RegisterClient2(String ip, String pinCode)
    {
      string hostname = System.Windows.Forms.SystemInformation.ComputerName;
      string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":11c43119-af3d-40e7-b1b2-743311375322c\",\"nickname\":\"" + hostname + " (Auto3D)\"},[{\"clientid\":\"" + hostname + ":11c43119-af3d-40e7-b1b2-743311375322c\",\"value\":\"yes\",\"nickname\":\"" + hostname + " (Auto3D)\",\"function\":\"WOL\"}]]}";

      PostRequest("http://" + ip + "/sony/accessControl", jsontosend, pinCode);
    }

    public bool PostRequest(String url, String jsonString, String pinCode, CookieContainer cc)
    {
      try
      {
        Log.Debug("Auto3D: PostRequest to URL = \"" + url + "\"");

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        request.Timeout = 3000;
        request.ContentType = "text/json";
        request.Method = "POST";
        request.AllowAutoRedirect = true;

        if (cc != null)
          request.CookieContainer = cc;

        if (pinCode != null)
        {
          string authInfo = "" + ":" + pinCode;
          authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
          request.Headers["Authorization"] = "Basic " + authInfo;
        }

        Log.Debug("Auto3D: JSON-String = \"" + jsonString + "\"");

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
          streamWriter.Write(jsonString);
          streamWriter.Flush();
          streamWriter.Close();
        }

        Application.DoEvents();
        Thread.Sleep(50);

        HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();


        // serialize cookies of httpresponse
         
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
        ShowMessageBoxFromNonUIThread("Command could not be sent. The error message is: " + ex.Message);
        return false;
      }

      return true;
    }

    /*private string Envelope(string command)
    {
        StringBuilder msg = new StringBuilder();
        msg.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        msg.AppendLine("<s:Envelope s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">");
        msg.AppendLine("<s:Body>");
        msg.AppendLine("<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">");
        msg.AppendFormat("<IRCCCode>{0}</IRCCCode>", command);
        msg.AppendLine();
        msg.AppendLine("</u:X_SendIRCC>");
        msg.AppendLine("</s:Body>");
        msg.AppendLine("</s:Envelope>");

        return msg.ToString();
    }

    public bool SendCommand2(String command)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + UPnPService.IP + ":80/IRCC");

        request.Method = "POST";            

        request.Headers.Add("X-CERS-DEVICE-INFO", "MediaPortal");
        request.Headers.Add("X-CERS-DEVICE-ID", "MediaRemote");

        request.Headers.Add("SOAPAction", "urn:schemas-sony-com:service:IRCC:1#X_SendIRCC");
        request.ContentType = "text/xml; charset=UTF-8";
        request.KeepAlive = false;
        request.Proxy = null;
        request.Timeout = 5000;

        byte[] byteArray = Encoding.UTF8.GetBytes(Envelope(command));

        request.ContentLength = byteArray.Length;

        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        string result;

        try
        {
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Info("Auto3D: Sending UPnP command failed! Error: " + ex.Message);
            return false;
        }

        return true;
    }*/
  }
}
