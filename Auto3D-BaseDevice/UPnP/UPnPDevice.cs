using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace MediaPortal.ProcessPlugins.Auto3D.UPnP
{
  public class UPnPDevice
  {    
    public UPnPDevice(Uri webAddr, XNamespace ns, XElement device)
    {
      Confirmed = true;
      Services = new List<UPnPService>();
      WebAddress = webAddr;

      DeviceType = device.Elements().First(e => e.Name.LocalName == "deviceType").Value;
      FriendlyName = device.Elements().First(e => e.Name.LocalName == "friendlyName").Value;
      Manufacturer = device.Elements().First(e => e.Name.LocalName == "manufacturer").Value;
      ModelName = device.Elements().First(e => e.Name.LocalName == "modelName").Value;
      UDN = device.Elements().First(e => e.Name.LocalName == "UDN").Value;

      XElement serviceList = device.Descendants(ns + "serviceList").First();
      List<XElement> services = serviceList.Descendants(ns + "service").ToList();

      foreach (XElement service in services)
      {
        UPnPService upnpService = new UPnPService(this, ns, service);
        Services.Add(upnpService);
      }     
    }

    internal bool Confirmed
    {
      get;
      set;
    }

    internal bool IsAvailable()
    {
      try
      {
        using (WebClient client = new WebClient())
        {
          client.DownloadString(WebAddress);
        }
      }
      catch (WebException)
      {
        return false;
      }

      return true;
    }

    public List<UPnPService> Services
    {
      get;
      private set;
    }

    public Uri WebAddress
    {
      get;
      private set;
    }

    public String DeviceType
    {
      get;
      private set;
    }

    public String FriendlyName
    {
      get;
      private set;
    }
    
    public String Manufacturer
    {
      get;
      private set;
    }

    public String ModelDescription
    {
      get;
      private set;
    }

    public String ModelName
    {
      get;
      private set;
    }

    public String UDN
    {
      get;
      private set;
    }
  }
}
