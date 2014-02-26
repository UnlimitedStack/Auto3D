using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using MediaPortal.GUI.Library;

namespace MediaPortal.ProcessPlugins.Auto3D.UPnP
{
  public class UPnPService
  {
    public UPnPService(UPnPDevice parent, XNamespace ns, XElement service)
    {
      ParentDevice = parent;
      ServiceType = service.Elements().First(e => e.Name.LocalName == "serviceType").Value;
      ServiceID = service.Elements().First(e => e.Name.LocalName == "serviceId").Value;
      ControlUrl = service.Elements().First(e => e.Name.LocalName == "controlURL").Value;

      Log.Debug("Auto3D: Device = " + parent.FriendlyName);
      Log.Debug("Auto3D: ServiceType = " + ServiceType);
      Log.Debug("Auto3D: ServiceID = " + ServiceID);
      Log.Debug("Auto3D: ControlUrl = " + ControlUrl);
    }

    public UPnPDevice ParentDevice
    {
      get;
      private set;
    }

    public String ServiceType
    {
      get;
      private set;
    }

    public String ServiceID
    {
      get;
      private set;
    }

    public String ControlUrl
    {
      get;
      set;
    }

    private string Envelope(String nameSpace, String functionName, String parameterName, String parameterValue)
    {
        StringBuilder msg = new StringBuilder();
        msg.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        msg.AppendLine("<s:Envelope s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">");
        msg.AppendLine("   <s:Body>");
        msg.AppendLine("      <u:" + functionName + " xmlns:u=\"" + nameSpace + "\">");
        msg.AppendFormat("         <" + parameterName + ">{0}</" + parameterName + ">", parameterValue);
        msg.AppendLine();
        msg.AppendLine("      </u:" + functionName + ">");
        msg.AppendLine("   </s:Body>");
        msg.AppendLine("</s:Envelope>");

        return msg.ToString();
    }

    public bool InvokeAction(String functionName, String parameterName, String parameterValue)
    {
        String webAddr = "http://" + ParentDevice.WebAddress.Host + ":" + ParentDevice.WebAddress.Port;

        String requestUrl;

        if (ControlUrl.StartsWith("http"))
        {
          requestUrl = ControlUrl;
        }
        else
        {
          if (ControlUrl.StartsWith("/"))
            requestUrl = webAddr + ControlUrl;
          else
            requestUrl = webAddr + "/" + ControlUrl;
        }

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

        Log.Info("Auto3D: HttpRequest = " + requestUrl);

        request.Method = "POST";            

        request.Headers.Add("X-CERS-DEVICE-INFO", "MediaPortal");
        request.Headers.Add("X-CERS-DEVICE-ID", "MediaRemote");

        request.ContentType = "text/xml; charset=\"utf-8\"";
        request.Headers.Add("SoapAction", "\"" + ServiceType + "#" + functionName + "\"");
        request.KeepAlive = false;
        request.Proxy = null;      
        request.Timeout = 5000;
              
        String envelope = Envelope(ServiceType, functionName, parameterName, parameterValue);

        byte[] byteArray = Encoding.UTF8.GetBytes(envelope);

        request.ContentLength = byteArray.Length;

        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        try
        {
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {              
                using (Stream responseStream = response.GetResponseStream())
                {                  
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        String result = sr.ReadToEnd();
                        Log.Info("Auto3D: InvokeAction = " + result);
                    }                 
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error("Auto3D: " + ex.Message);
            return false;
        }

        return true;
    }

    public override String ToString()
    {
      return ParentDevice.WebAddress.Host;
    }
  }
}
