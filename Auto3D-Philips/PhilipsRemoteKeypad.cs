using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    public partial class PhilipsRemoteKeypad : UserControl, IAuto3DKeypad
    {
        PhilipsTV _device;

        public PhilipsRemoteKeypad()
        {
            InitializeComponent();
        }

        public void SetDevice(IAuto3D device)
        {
            _device = (PhilipsTV)device;            
        }

        public void UpdateState()
        {
            button3D.Visible = _device.ConnectionMethod == eConnectionMethod.DirectFB;
        }
    }
}
