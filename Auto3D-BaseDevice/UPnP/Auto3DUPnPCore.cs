using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Net.NetworkInformation;
using System.ComponentModel;
using MediaPortal.GUI.Library;

namespace MediaPortal.ProcessPlugins.Auto3D.UPnP
{
  public class Auto3DUPnPCore
  {
    static Auto3DUPnPCore _instance;
    
    IPAddress UpnpMulticastV4Addr;
    
    IPAddress UpnpMulticastV6Addr1;
    IPAddress UpnpMulticastV6Addr2;

    IPEndPoint UpnpMulticastV4EndPoint;
    IPEndPoint UpnpMulticastV6EndPoint1;
    IPEndPoint UpnpMulticastV6EndPoint2;
   
    List<UPnPDevice> _devices = new List<UPnPDevice>();
    Hashtable _sessions = new Hashtable();

    System.Timers.Timer _timer;

    public delegate void ServiceFoundHandler(object sender, ServiceEventArgs e);
    public event ServiceFoundHandler ServiceFound;

    public delegate void ServiceRemovedHandler(object sender, ServiceEventArgs e);
    public event ServiceRemovedHandler ServiceRemoved;

    public delegate void ScanFinishedHandler(object sender, EventArgs e);
    public event ScanFinishedHandler ScanFinished;
	
    public List<UPnPDevice> Devices
    {
      get
      {
        return _devices;
      }
    }

    Auto3DUPnPCore()
    {
      UpnpMulticastV4Addr = IPAddress.Parse("239.255.255.250");

      UpnpMulticastV6Addr1 = IPAddress.Parse("FF05::C");
      UpnpMulticastV6Addr2 = IPAddress.Parse("FF02::C");

      UpnpMulticastV4EndPoint = new IPEndPoint(UpnpMulticastV4Addr, 1900);
      UpnpMulticastV6EndPoint1 = new IPEndPoint(UpnpMulticastV6Addr1, 1900);
      UpnpMulticastV6EndPoint2 = new IPEndPoint(UpnpMulticastV6Addr2, 1900);

      _timer = new System.Timers.Timer();
      _timer.Interval = 5000; 
      _timer.AutoReset = false;
      _timer.Elapsed += _timer_Elapsed;
    }

    public static Auto3DUPnPCore Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new Auto3DUPnPCore();
        }
        return _instance;
      }
    }
	
    public bool Scanning
    {
      get 
      {
        return _timer.Enabled == true;
      }
    }
  
    public void StartSSDP()
    {
      if (_timer.Enabled)
        return;

      _timer.Start();

      InternalScan(UpnpMulticastV4EndPoint);
      //InternalScan(UpnpMulticastV6EndPoint1);
      //InternalScan(UpnpMulticastV6EndPoint2);
    }

    public void StopSSDP()
    {
      _timer.Stop();

      foreach (UdpClient session in _sessions.Values)
      {
        session.Close();
      }

      _sessions.Clear();
    }

    void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      _timer.Stop();

      List<UPnPDevice> devicesToRemove = new List<UPnPDevice>();

      foreach (UPnPDevice device in _devices)
      {
        if (device.Confirmed == false)
        {
          if (device.IsAvailable())
          {
            device.Confirmed = true;
          }
          else
          {
            devicesToRemove.Add(device);
          }
        }
      }

      if (ScanFinished != null)
        RaiseEventOnUIThread(ScanFinished, new object[] { _instance, new EventArgs() });

      foreach (UPnPDevice device in devicesToRemove)
      {
        foreach (UPnPService service in device.Services)
        {
          if (ServiceRemoved != null)
            RaiseEventOnUIThread(ServiceRemoved, new object[] { _instance, new ServiceEventArgs(service) });
        }

        _devices.Remove(device);
      }

      foreach (UPnPDevice device in _devices)
        device.Confirmed = false;

      InternalScan(UpnpMulticastV4EndPoint);

      _timer.Start();
    }

    void InternalScan(IPEndPoint RemoteEP)
    {
      IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());

      String message = "M-SEARCH * HTTP/1.1\r\n";
      message += "MX: 5\r\n";
      message += "ST: upnp:rootdevice\r\n";
      message += "MAN: \"ssdp:discover\"\r\n";

      if (RemoteEP.AddressFamily == AddressFamily.InterNetwork)
        message += "HOST: " + RemoteEP.ToString() + "\r\n";
        
      if (RemoteEP.AddressFamily == AddressFamily.InterNetworkV6)
        message += "HOST: " + String.Format("[{0}]:{1}\r\n", RemoteEP.Address.ToString(), RemoteEP.Port);

      message += "Content-Length: 0\r\n\r\n";      

      byte[] buffer = UTF8Encoding.UTF8.GetBytes(message);

      foreach (IPAddress localaddr in ipHost.AddressList)
      {
        try
        {
          UdpClient session = (UdpClient)_sessions[localaddr];
            
          if (session == null)
          {
            session = new UdpClient(new IPEndPoint(localaddr, 0));
            session.EnableBroadcast = true;
            session.BeginReceive(new AsyncCallback(OnReceiveSink), session);
            _sessions[localaddr] = session;
          }

          if (RemoteEP.AddressFamily != session.Client.AddressFamily) continue;
          if ((RemoteEP.AddressFamily == AddressFamily.InterNetworkV6) && ((IPEndPoint)session.Client.LocalEndPoint).Address.IsIPv6LinkLocal == true && RemoteEP != UpnpMulticastV6EndPoint2) continue;
          if ((RemoteEP.AddressFamily == AddressFamily.InterNetworkV6) && ((IPEndPoint)session.Client.LocalEndPoint).Address.IsIPv6LinkLocal == false && RemoteEP != UpnpMulticastV6EndPoint1) continue;

          IPEndPoint lep = (IPEndPoint)session.Client.LocalEndPoint;
            
          if (session.Client.AddressFamily == AddressFamily.InterNetwork)
          {
            session.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, localaddr.GetAddressBytes());
          }
          else if (session.Client.AddressFamily == AddressFamily.InterNetworkV6)
          {
            session.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface, BitConverter.GetBytes((int)localaddr.ScopeId));
          }

          session.Send(buffer, buffer.Length, RemoteEP);
          session.Send(buffer, buffer.Length, RemoteEP);
        }
        catch (Exception ex)
        {          
          Log.Error("Auto3D: " + ex.Message);
        }
      }
    }

    public void OnReceiveSink(IAsyncResult ar)
    {
      IPEndPoint ep = null;
      UdpClient client = (UdpClient)ar.AsyncState;
      try
      {
        byte[] buf = client.EndReceive(ar, ref ep);
        
        if (buf != null)
        {
          String stringData = Encoding.ASCII.GetString(buf, 0, buf.Length);

          String[] splitPacket = stringData.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

          foreach (String packetLine in splitPacket)
          {
            int pos = packetLine.ToUpper().IndexOf("LOCATION");

            if (pos > -1)
            {
              String splitString = packetLine.Substring(pos, 9);

              String[] location = packetLine.Split(new String[] { splitString }, StringSplitOptions.RemoveEmptyEntries);

              try
              {
                using (WebClient webClient = new WebClient())
                {
                  ParseSSDPResponse(new Uri(location[0]), webClient.DownloadString(location[0]));
                }
              }
              catch (WebException ex)
              {
              }
              break;
            }
          }

          client.BeginReceive(new AsyncCallback(OnReceiveSink), client);
          return;
        }
      }
      catch (ObjectDisposedException)
      {
        return; // ignore this, it means the end timer did shut down the sessions
      }
      catch (Exception ex)
      {
        Log.Error("Auto3D: " + ex.Message);
      }

      if (client.Client != null)
      {
        IPEndPoint local = (IPEndPoint)client.Client.LocalEndPoint;
        _sessions.Remove(local.Address);
      }
    }

    private static void RaiseEventOnUIThread(Delegate eventdelegate, object[] args)
    {
      foreach (Delegate d in eventdelegate.GetInvocationList())
      {
        ISynchronizeInvoke syncer = d.Target as ISynchronizeInvoke;
        
        if (syncer == null)
        {
          d.DynamicInvoke(args);
        }
        else
        {
          syncer.BeginInvoke(d, args);
        }
      }
    }

    private void ParseSSDPResponse(Uri webAddr, String response)
    {
      using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
      {
        using (XmlReader reader = XmlReader.Create(stream))
        {
          XDocument document = XDocument.Load(reader, LoadOptions.None);

          XNamespace ns = "urn:schemas-upnp-org:device-1-0";
          List<XElement> devices = document.Descendants(ns + "device").ToList();

          foreach (XElement device in devices)
          {
            UPnPDevice upnpDevice = new UPnPDevice(webAddr, ns, device);

            lock (_devices)
            {
              bool deviceExists = false;

              foreach (UPnPDevice deviceCompare in _devices)
              {
                if (deviceCompare.UDN == upnpDevice.UDN)
                {
                  deviceCompare.Confirmed = true;
                  deviceExists = true;
                  break;
                }
              }

              if (!deviceExists)
              {
                _devices.Add(upnpDevice);

                foreach (UPnPService service in upnpDevice.Services)
                {
                  if (ServiceFound != null)
                    RaiseEventOnUIThread(ServiceFound, new object [] { _instance, new ServiceEventArgs(service) });
                }
              }
            }
          }
        }
      }
    }
  }
}


