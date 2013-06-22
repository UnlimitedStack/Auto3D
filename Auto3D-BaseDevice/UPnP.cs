using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MediaPortal.GUI.Library;
using OpenSource.UPnP;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    public class Auto3DUPnPService
    {
        UPnPService _service;

        public Auto3DUPnPService(UPnPService service)
        {
            _service = service;
        }

        public String Manufacturer
        {
            get { return _service.ParentDevice.Manufacturer; }            
        }

        public String UniqueDeviceName
        {
            get { return _service.ParentDevice.UniqueDeviceName; }
        }

        public String IP
        {
            get 
            {
                String ip = _service.ParentDevice.RemoteEndPoint.ToString();
                return ip.Substring(0, ip.LastIndexOf(':'));
            }
        }

        public override string ToString()
        {
            return _service.ParentDevice.ModelName + " - " + IP;
        }

        public object InvokeAction(String actionName, String argumentName, String value)
        {
            UPnPArgument[] arg = new OpenSource.UPnP.UPnPArgument[1];
            arg[0] = new OpenSource.UPnP.UPnPArgument(argumentName, value);
            return _service.InvokeSync(actionName, arg);
        }
    }    

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
        String ServiceName { get; }  
        void ServiceAdded(Auto3DUPnPService service);
        void ServiceRemoved(Auto3DUPnPService service);        
    }

    public static class A3DUPnP
    {
        static UPnPSmartControlPoint _uPnPSmartControlPoint = new OpenSource.UPnP.UPnPSmartControlPoint();
        static List<UPnPDevice> _devices = new List<UPnPDevice>();
        static List<Auto3DUPnPService> _services = new List<Auto3DUPnPService>();
        static List<ServiceCallBack> _serviceCallbacks = new List<ServiceCallBack>();

        public static void Init()
        {            
            _uPnPSmartControlPoint.OnAddedDevice += _uPnPSmartControlPoint_OnAddedDevice;
            _uPnPSmartControlPoint.OnRemovedDevice += _uPnPSmartControlPoint_OnRemovedDevice;            
        }

        public static void Scan()
        {            
            _uPnPSmartControlPoint.Rescan();
        }

        public static void RegisterForCallbacks(IAuto3DUPnPServiceCallBack callback)
        {
            ServiceCallBack scb = new ServiceCallBack(callback);
            _serviceCallbacks.Add(scb);
        }

        static void _uPnPSmartControlPoint_OnAddedDevice(UPnPSmartControlPoint sender, UPnPDevice device)
        {
            _devices.Add(device);
            
            foreach (UPnPService service in device.Services)
            {
                foreach (ServiceCallBack scb in _serviceCallbacks)
                {
                    if (((scb.Callback.ServiceName == service.ServiceURN) ||
                        (service.ServiceURN.Contains(scb.Callback.ServiceName))) && !scb.ClientNotified)
                    {
                        // call back on GUI thread

                        Auto3DHelpers.GetMainForm().Invoke((System.Windows.Forms.MethodInvoker)delegate 
                        {
                            scb.Callback.ServiceAdded(new Auto3DUPnPService(service));                            
                        });

                        scb.ClientNotified = true;
                    }
                }
            }
        }

        static void _uPnPSmartControlPoint_OnRemovedDevice(UPnPSmartControlPoint sender, UPnPDevice device)
        {
            _devices.Remove(device);

            foreach (UPnPService service in device.Services)
            {
                foreach (ServiceCallBack scb in _serviceCallbacks)
                {
                    // call back on GUI thread

                    if (scb.Callback.ServiceName == service.ServiceURN)
                    {
                        Auto3DHelpers.GetMainForm().Invoke((System.Windows.Forms.MethodInvoker)delegate
                        {
                            scb.Callback.ServiceRemoved(new Auto3DUPnPService(service));
                        });
                    }
                }
            }            
        }
    }
}
