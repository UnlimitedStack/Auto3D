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

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    class NoDevice : Auto3DBaseDevice
    {
        public NoDevice()
        {
        }

        public override String CompanyName
        {
            get { return "No company"; }
        }

        public override String DeviceName
        {
            get { return "No device"; }
        }
    }
}
