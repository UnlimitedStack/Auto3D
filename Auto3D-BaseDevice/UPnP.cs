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
                return _service.ParentDevice.RemoteEndPoint.Address.ToString();
            }
        }

        public String Port
        {
            get
            {
                return _service.ParentDevice.RemoteEndPoint.Port.ToString();
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
        String UPnPServiceName { get; }
        String UPnPManufacturer { get; }
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
                    System.Diagnostics.Debug.WriteLine(service.ParentDevice.Manufacturer + " - " + service.ServiceURN);

                    bool bNameCheck = true;

                    if (scb.Callback.UPnPManufacturer != "")
                    {
                        bNameCheck = service.ParentDevice.Manufacturer.StartsWith(scb.Callback.UPnPManufacturer);
                    }

                    if (((scb.Callback.UPnPServiceName == service.ServiceURN) || (service.ServiceURN.Contains(scb.Callback.UPnPServiceName))) && bNameCheck && !scb.ClientNotified)
                    {
                        System.Diagnostics.Debug.WriteLine("Invoke: " + service.ParentDevice.Manufacturer + " - " + service.ServiceURN);

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
                    bool bNameCheck = true;

                    if (scb.Callback.UPnPManufacturer != "")
                    {
                        bNameCheck = service.ParentDevice.Manufacturer.StartsWith(scb.Callback.UPnPManufacturer);
                    }

                    if (((scb.Callback.UPnPServiceName == service.ServiceURN) || (service.ServiceURN.Contains(scb.Callback.UPnPServiceName))) && bNameCheck)
                    {
                        // call back on GUI thread

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
