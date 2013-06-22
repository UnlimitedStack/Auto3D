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
using System.IO.Ports;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    class EpsonBeamer : Auto3DBaseDevice
    {
        SerialPort _serialPort;

        public EpsonBeamer()
        {
        }

        public override String CompanyName
        {
            get { return "Epson"; }
        }

        public override String DeviceName
        {
            get { return "Epson Beamer"; }
        }

        public String PortName
        {
            get;
            set;
        }

        public override void Start()
        {
            if (_serialPort != null && _serialPort.IsOpen)
                _serialPort.Close();    

            _serialPort = new SerialPort(PortName, 9600, Parity.None, 8, StopBits.One);
            _serialPort.NewLine = "\r";
            _serialPort.DataReceived += _serialPort_DataReceived;

            try
            {
                if (_serialPort.PortName != "None")
                {
                    _serialPort.Open();                                           
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Auto3D");
                Log.Info("Auto3D: " + ex.Message);
            }            
        }

        public override void Stop()
        {
            _serialPort.Close();
        }

        public override void LoadSettings()
        {
            if (_serialPort != null && _serialPort.IsOpen)
                _serialPort.Close();     

            using (Settings reader = new MPSettings())
            {
                DeviceModelName = reader.GetValueAsString("Auto3DPlugin", "EpsonModel", "Default");
                PortName = reader.GetValueAsString("Auto3DPlugin", "EpsonPort", "None");
            }

            if (_serialPort != null)
            {
                _serialPort.PortName = PortName;

                try
                {
                    if (_serialPort.PortName != "None")
                        _serialPort.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Auto3D");
                    Log.Info("Auto3D: " + ex.Message);                    
                }      
            }
        }

        public override void SaveSettings()
        {
            using (Settings writer = new MPSettings())
            {
                writer.SetValue("Auto3DPlugin", "EpsonModel", SelectedDeviceModel.Name);
                writer.SetValue("Auto3DPlugin", "EpsonPort", PortName);
            }
        }

        public override bool SendCommand(String command)
        {
            switch (command)
            {
                case "3DDisplayOn":

                    if (!InternalSendCommand("3DIMENSION 01 01"))
                        return false;
                    break;

                case "3DDisplayOff":

                    if (!InternalSendCommand("3DIMENSION 01 00"))
                        return false;
                    break;

                case "3DFormatAuto":

                    if (!InternalSendCommand("3DIMENSION 03 00"))
                        return false;
                    break;

                case "3DFormat2D":

                    if (!InternalSendCommand("3DIMENSION 03 01"))
                        return false;
                    break;

                case "3DFormatSBS":

                    if (!InternalSendCommand("3DIMENSION 03 02"))
                        return false;
                    break;

                case "3DFormatTAB":

                    if (!InternalSendCommand("3DIMENSION 03 03"))
                        return false;
                    break;

                case "2D3DConversionOn":

                    if (!InternalSendCommand("3DIMENSION 02 00"))
                        return false;
                    break;

                case "2D3DConversionOff":

                    if (!InternalSendCommand("3DIMENSION 02 01"))
                        return false;
                    break;

            }

            return true;
        }

        private bool InternalSendCommand(String command)
        {
            try
            {
                _serialPort.WriteLine(command);
            }
            catch (System.Exception ex)
            {
                Log.Info("Auto3D: Error sending command: " + ex.Message);
                return false;
            }

            return true;
        }

        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            System.Threading.Thread.Sleep(100);
            string data = sp.ReadExisting();

            Log.Info("Auto3D: Command answer: \"" + data + "\"");
        }
    }
}
