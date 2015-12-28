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
  class OptomaBeamer : Auto3DBaseDevice
  {
    SerialPort _serialPort;

    public OptomaBeamer()
    {
    }

    public override String CompanyName
    {
      get { return "Optoma"; }
    }

    public override String DeviceName
    {
      get { return "Optoma Beamer"; }
    }

    public String PortName
    {
      get;
      set;
    }

    private void StartSerial()
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


    public override void Start()
    {
	  base.Start();

      StartSerial();
    }

    public override void Stop()
    {
	  base.Stop();
      _serialPort.Close();
    }

    public override void Suspend()
    {
        _serialPort.Close();
    }

    public override void Resume()
    {
        StartSerial();
    }


    public override void LoadSettings()
    {
      if (_serialPort != null && _serialPort.IsOpen)
        _serialPort.Close();

      using (Settings reader = new MPSettings())
      {
        DeviceModelName = reader.GetValueAsString("Auto3DPlugin", "OptomaModel", "Default");
        PortName = reader.GetValueAsString("Auto3DPlugin", "OptomaPort", "None");
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
        writer.SetValue("Auto3DPlugin", "OptomaModel", SelectedDeviceModel.Name);
        writer.SetValue("Auto3DPlugin", "OptomaPort", PortName);
      }
    }

    public override bool SendCommand(RemoteCommand rc)
    {
      switch (rc.Command)
      {
        case "CursorLeft":

          if (!InternalSendCommand("~XX140 11"))
            return false;
          break;

        case "CursorRight":

          if (!InternalSendCommand("~XX140 13"))
            return false;
          break;

        case "CursorUp":

          if (!InternalSendCommand("~XX140 10"))
            return false;
          break;

        case "CursorDown":

          if (!InternalSendCommand("~XX140 14"))
            return false;
          break;

        case "Enter":

          if (!InternalSendCommand("~XX140 12"))
            return false;
          break;

        case "Menu":

          if (!InternalSendCommand("~XX140 20"))
            return false;
          break;

        case "3DFormatOff":

          if (!InternalSendCommand("~XX405 0"))
            return false;
          break;

        case "3DFormatSBS":

          if (!InternalSendCommand("~XX405 1"))
            return false;
          break;

        case "3DFormatTAB":

          if (!InternalSendCommand("~XX405 3"))
            return false;
          break;

        case "3D2DFormat3D":

          if (!InternalSendCommand("~XX400 1"))
            return false;
          break;

        case "3D2DFormatL":

          if (!InternalSendCommand("~XX400 2"))
            return false;
          break;

        case "3D2DFormatR":

          if (!InternalSendCommand("~XX400 3"))
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
