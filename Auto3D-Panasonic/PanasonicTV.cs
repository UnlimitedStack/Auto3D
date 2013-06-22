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
using System.Diagnostics;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    class PanasonicTV : Auto3DUPnPBaseDevice
    {
        public PanasonicTV()
        {
        }

        public override String CompanyName
        {
            get { return "Panasonic"; }
        }

        public override String DeviceName
        {
            get { return "Panasonic TV"; }
        }

        public override String ServiceName
        {
            get { return ":p00NetworkControl";  }
        }

        public String UDN
        {
            get;
            set;
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }

        public override void LoadSettings()
        {
            using (Settings reader = new MPSettings())
            {
                DeviceModelName = reader.GetValueAsString("Auto3DPlugin", "PanasonicModel", "VIERA");
                UDN = reader.GetValueAsString("Auto3DPlugin", "PanasonicAddress", "");
            }
        }

        public override void SaveSettings()
        {
            using (Settings writer = new MPSettings())
            {
                writer.SetValue("Auto3DPlugin", "PanasonicModel", SelectedDeviceModel.Name);
                writer.SetValue("Auto3DPlugin", "PanasonicAddress", UDN);
            }
        }

        public override void ServiceAdded(Auto3DUPnPService service)
        {
            base.ServiceAdded(service);

            Log.Info("Auto3D: Panasonic service found -> " + service.Manufacturer + ", " + service.IP);

            if (service.UniqueDeviceName == UDN)
            {
                Log.Info("Auto3D: Panasonic service connected");
            }
        }

        public override void ServiceRemoved(Auto3DUPnPService service)
        {
            base.ServiceRemoved(service);
        }

        public override bool SendCommand(String command)
        {
            switch (command)
            {
                case "Mode3D":

                    if (!InternalSendCommand("NRC_3D-ONOFF"))
                        return false;
                    break;

                case "Confirm":

                    if (!InternalSendCommand("NRC_ENTER-ONOFF"))
                        return false;
                    break;

                case "CursorUp":

                    if (!InternalSendCommand("NRC_UP-ONOFF"))
                        return false;
                    break;

                case "CursorDown":

                    if (!InternalSendCommand("NRC_DOWN-ONOFF"))
                        return false;
                    break;

                case "CursorLeft":

                    if (!InternalSendCommand("NRC_LEFT-ONOFF"))
                        return false;
                    break;

                case "CursorRight":

                    if (!InternalSendCommand("NRC_RIGHT-ONOFF"))
                        return false;
                    break;

                case "Delay":

                    // do nothing here
                    break;

                default:

                    Log.Info("Auto3D: Unknown command - " + command);
                    break;
            }

            return true;
        }

        private bool InternalSendCommand(String command)
        {
            if (UPnPService != null)
            {
                UPnPService.InvokeAction("X_SendKey", "X_KeyEvent", command);
                return true;
            }
            else
                return false;
        }
    }
}
