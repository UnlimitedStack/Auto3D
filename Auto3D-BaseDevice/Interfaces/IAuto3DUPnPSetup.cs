using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.ProcessPlugins.Auto3D;
using System.Windows.Forms;
using MediaPortal.ProcessPlugins.Auto3D.UPnP;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public interface IAuto3DUPnPSetup : IAuto3DSetup
  {
    void ServiceAdded(UPnPService service);
    void ServiceRemoved(UPnPService service);
  }
}
