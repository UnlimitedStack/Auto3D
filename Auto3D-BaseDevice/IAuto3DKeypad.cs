using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.ProcessPlugins.Auto3D;
using System.Windows.Forms;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    public interface IAuto3DKeypad
    {
        void SetDevice(IAuto3D device);               // Set device for keypad
        void UpdateState();                                         // Update after device state has changed
    }
}
