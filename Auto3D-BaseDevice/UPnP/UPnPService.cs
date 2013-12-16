using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
      ServiceID = service.Elements().First(e => e.Name.LocalName == "serviceId").Value;
      ControlUrl = service.Elements().First(e => e.Name.LocalName == "controlURL").Value;
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
        msg.AppendLine("<s:Body>");
        msg.AppendLine("<u:" + functionName + " xmlns:u=\"" + nameSpace + "\">");
        msg.AppendFormat("<" + parameterName + ">{0}</" + parameterName + ">", parameterValue);
        msg.AppendLine();
        msg.AppendLine("</u:" + functionName + ">");
        msg.AppendLine("</s:Body>");
        msg.AppendLine("</s:Envelope>");

        return msg.ToString();
    }

    public bool InvokeAction(String functionName, String parameterName, String parameterValue)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ParentDevice.WebAddress + ControlUrl);

        request.Method = "POST";            

        request.Headers.Add("X-CERS-DEVICE-INFO", "MediaPortal");
        request.Headers.Add("X-CERS-DEVICE-ID", "MediaRemote");

        request.Headers.Add("SOAPAction", ServiceID + "#" + functionName);
        request.ContentType = "text/xml; charset=\"utf-8\"";
        request.KeepAlive = false;
        request.Proxy = null;
        request.Timeout = 5000;

        byte[] byteArray = Encoding.UTF8.GetBytes(Envelope(ServiceID, functionName, parameterName, parameterValue));

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
                    }                 
                }
            }
        }
        catch (Exception ex)
        {
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
