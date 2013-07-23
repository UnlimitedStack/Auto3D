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
    public class SonyTV : Auto3DUPnPBaseDevice
    {
        public SonyTV()
        {
        }

        public override String CompanyName
        {
            get { return "Sony"; }
        }

        public override String DeviceName
        {
            get { return "Sony TV"; }
        }

        public override String UPnPServiceName
        {
            get { return "urn:schemas-sony-com:service:IRCC:1"; }
        }

        public override String UPnPManufacturer
        {
            get { return ""; } // name is not necessary for service recognition
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
                DeviceModelName = reader.GetValueAsString("Auto3DPlugin", "SonyModel", "BRAVIA");
                UDN = reader.GetValueAsString("Auto3DPlugin", "SonyAddress", "");  
            }
        }

        public override void SaveSettings()
        {
            using (Settings writer = new MPSettings())
            {
                writer.SetValue("Auto3DPlugin", "SonyModel", SelectedDeviceModel.Name);
                writer.SetValue("Auto3DPlugin", "SonyAddress", UDN);
            }
        }

        public override void ServiceAdded(Auto3DUPnPService service)
        {
            base.ServiceAdded(service);

            Log.Info("Auto3D: Sony service found -> " + service.Manufacturer + ", " + service.IP);

            if (service.UniqueDeviceName == UDN)
            {
                Log.Info("Auto3D: Sony service connected");
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

                    if (!InternalSendCommand("AAAAAgAAAHcAAABNAw=="))
                        return false;
                    break;

                case "Confirm":

                    if (!InternalSendCommand("AAAAAQAAAAEAAABlAw=="))
                        return false;
                    break;

                case "Return":

                    if (!InternalSendCommand("AAAAAgAAAJcAAAAjAw =="))
                        return false;
                    break;

                case "CursorUp":

                    if (!InternalSendCommand("AAAAAQAAAAEAAAB0Aw=="))
                        return false;
                    break;

                case "CursorDown":

                    if (!InternalSendCommand("AAAAAQAAAAEAAAB1Aw=="))
                        return false;
                    break;

                case "CursorLeft":

                    if (!InternalSendCommand("AAAAAQAAAAEAAAA0Aw=="))
                        return false;
                    break;

                case "CursorRight":

                    if (!InternalSendCommand("AAAAAQAAAAEAAAAzAw=="))
                        return false;
                    break;

                default:

                    Log.Info("Auto3D: Unknown command - " + command);
                    break;
            }

            return true;
        }

        private bool InternalSendCommand(String command)
        {
            UPnPService.InvokeAction("X_SendIRCC", "IRCCCode", command);
            return true;
        }

        public void RegisterClient(String ip)
        {
            String registrationUrl = "http://" + ip + "/cers/api/register?name=DeviceName&registrationType=initial&deviceId=MediaRemote";

            Log.Info("Auto3D: Register Sony client");

            HttpWebRequest request = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(registrationUrl);

                request.Headers.Add("X-CERS-DEVICE-INFO", "MediaPortal");
                request.KeepAlive = false;
                request.Proxy = null;
                request.Timeout = 30000;    // There is a 30 second countdown running on the TV side if client is registering for the first time.            
                                            // User needs to click OK and confirm before remote commands are accepted.
            }
            catch (System.Exception ex)
            {
                Log.Info("Auto3D: Register Sony client failed! Error: " + ex.Message);
                ShowMessageBoxFromNonUIThread("Registration at Sony TV failed!\nError: " + ex.Message);
                return;
            }

            try
            {
                using (WebResponse response = (HttpWebResponse)request.GetResponse()) { }
            }
            catch (Exception ex) // TV will respond with 40X code if registering fails
            {
                Log.Info("Auto3D: Register Sony client failed! Error: " + ex.Message);
                MessageBox.Show("Registration at Sony TV failed!\nError: " + ex.Message, "Auto3D");
                return;
            }

            ShowMessageBoxFromNonUIThread("Registration at Sony TV succeeded!");
        }

        /*private string Envelope(string command)
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            msg.AppendLine("<s:Envelope s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            msg.AppendLine("<s:Body>");
            msg.AppendLine("<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">");
            msg.AppendFormat("<IRCCCode>{0}</IRCCCode>", command);
            msg.AppendLine();
            msg.AppendLine("</u:X_SendIRCC>");
            msg.AppendLine("</s:Body>");
            msg.AppendLine("</s:Envelope>");

            return msg.ToString();
        }

        public bool SendCommand2(String command)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + UPnPService.IP + ":80/IRCC");

            request.Method = "POST";            

            request.Headers.Add("X-CERS-DEVICE-INFO", "MediaPortal");
            request.Headers.Add("X-CERS-DEVICE-ID", "MediaRemote");

            request.Headers.Add("SOAPAction", "urn:schemas-sony-com:service:IRCC:1#X_SendIRCC");
            request.ContentType = "text/xml; charset=UTF-8";
            request.KeepAlive = false;
            request.Proxy = null;
            request.Timeout = 5000;

            byte[] byteArray = Encoding.UTF8.GetBytes(Envelope(command));

            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            string result;

            try
            {
                using (WebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("Auto3D: Sending UPnP command failed! Error: " + ex.Message);
                return false;
            }

            return true;
        }*/
    }
}
