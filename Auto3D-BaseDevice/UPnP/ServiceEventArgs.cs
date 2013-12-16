using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.ProcessPlugins.Auto3D.UPnP
{
  public class ServiceEventArgs : EventArgs
  {
    public readonly UPnPService Service;

    public ServiceEventArgs(UPnPService service)
    {
      Service = service;
    }
  }
}
