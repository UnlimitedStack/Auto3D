using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MediaPortal.ProcessPlugins.Auto3D.UPnP
{
  internal class ServiceCallBack
  {
    public ServiceCallBack(IAuto3DUPnPServiceCallBack callback)
    {
      Callback = callback;
      ClientNotified = false;
    }

    public IAuto3DUPnPServiceCallBack Callback
    {
      set;
      get;
    }

    public bool ClientNotified
    {
      set;
      get;
    }
  }

  public interface IAuto3DUPnPServiceCallBack
  {
    String UPnPServiceName { get; }
    String UPnPManufacturer { get; }
    void ServiceAdded(UPnPService service);
    void ServiceRemoved(UPnPService service);
  }

  public static class Auto3DUPnP
  {
    static List<ServiceCallBack> _serviceCallbacks = new List<ServiceCallBack>();

    public static void Init()
    {
      Auto3DUPnPCore.Instance.ServiceFound += Auto3DUPnP_ServiceFound;
      Auto3DUPnPCore.Instance.ServiceRemoved += Auto3DUPnP_ServiceRemoved;
    }

    public static void StartSSDP()
    {
      Auto3DUPnPCore.Instance.StartSSDP();
    }

    public static void StopSSDP()
    {
      Auto3DUPnPCore.Instance.StopSSDP();
    }

    public static void RegisterForCallbacks(IAuto3DUPnPServiceCallBack callback)
    {
      ServiceCallBack scb = new ServiceCallBack(callback);
      _serviceCallbacks.Add(scb);
    }


    static void Auto3DUPnP_ServiceFound(object sender, ServiceEventArgs e)
    {
      foreach (ServiceCallBack scb in _serviceCallbacks)
      {
        System.Diagnostics.Debug.WriteLine(e.Service.ParentDevice.Manufacturer + " - " + e.Service.ServiceID);

        bool bNameCheck = true;

        if (scb.Callback.UPnPManufacturer != "")
        {
          bNameCheck = e.Service.ParentDevice.Manufacturer.StartsWith(scb.Callback.UPnPManufacturer);
        }

        if (((scb.Callback.UPnPServiceName == e.Service.ServiceType) || (e.Service.ServiceType.Contains(scb.Callback.UPnPServiceName))) && bNameCheck && !scb.ClientNotified)
        {
          scb.Callback.ServiceAdded(e.Service);
          scb.ClientNotified = true;
        }
      }
    }

    static void Auto3DUPnP_ServiceRemoved(object sender, ServiceEventArgs e)
    {
       foreach (ServiceCallBack scb in _serviceCallbacks)
      {
        System.Diagnostics.Debug.WriteLine(e.Service.ParentDevice.Manufacturer + " - " + e.Service.ServiceID);

        bool bNameCheck = true;

        if (scb.Callback.UPnPManufacturer != "")
        {
          bNameCheck = e.Service.ParentDevice.Manufacturer.StartsWith(scb.Callback.UPnPManufacturer);
        }

        if (((scb.Callback.UPnPServiceName == e.Service.ServiceType) || (e.Service.ServiceType.Contains(scb.Callback.UPnPServiceName))) && bNameCheck && scb.ClientNotified)
        {
          scb.Callback.ServiceRemoved(e.Service);
          scb.ClientNotified = false;
        }
      }
    }
  }
}
