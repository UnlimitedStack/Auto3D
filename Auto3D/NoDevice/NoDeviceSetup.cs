using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaPortal.Profile;
using System.IO;
using System.Reflection;
using MediaPortal.Configuration;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    public partial class NoDeviceSetup : UserControl, IAuto3DSetup
    {
        IAuto3D _device;

        public NoDeviceSetup(IAuto3D device)
        {
            InitializeComponent();
            _device = device;
        }

        public IAuto3D GetDevice()
        {
            return _device;
        }

        public void LoadSettings()
        {
        }

        public void SaveSettings()
        {         
        }
    }
}
