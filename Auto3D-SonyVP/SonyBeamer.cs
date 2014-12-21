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
  class SonyBeamer : Auto3DBaseDevice
  {
    SerialPort _serialPort;

    public SonyBeamer()
    {
    }

    public override String CompanyName
    {
      get { return "SonyVP"; }
    }

    public override String DeviceName
    {
      get { return "Sony Beamer"; }
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

      _serialPort = new SerialPort(PortName, 38400, Parity.Even, 8, StopBits.One);
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
        DeviceModelName = reader.GetValueAsString("Auto3DPlugin", "SonyModel", "Default");
        PortName = reader.GetValueAsString("Auto3DPlugin", "SonyPort", "None");
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
        writer.SetValue("Auto3DPlugin", "SonyModel", SelectedDeviceModel.Name);
        writer.SetValue("Auto3DPlugin", "SonyPort", PortName);
      }
    }

    public override bool SendCommand(String command)
    {
      switch (command)
      {
        case "3DFormatAuto":

          if (!InternalSendCommand("A9,00,60,00,00,00,60,9A"))
            return false;
          break;

        case "3DFormat3D":

          if (!InternalSendCommand("A9,00,60,00,00,01,61,9A"))
            return false;
          break;

        case "3DFormat2D":

          if (!InternalSendCommand("A9,00,60,00,00,02,62,9A"))
            return false;
          break;

        case "3DFormatSBS":

          if (!InternalSendCommand("A9,00,61,00,00,01,61,9A"))
            return false;
          break;

        case "3DFormatTAB":

          if (!InternalSendCommand("A9,00,61,00,00,02,63,9A"))
            return false;
          break;

        case "3DDisplaySimulated":

          if (!InternalSendCommand("A9,00,60,00,00,02,62,9A"))
            return false;
          break;
      }

      return true;
    }

    private bool InternalSendCommand(String command)
    {
      String[] byteStrings = command.Split(",".ToCharArray());
      Byte[] buffer = new Byte[byteStrings.GetLength(0)];

      for (int i = 0; i < byteStrings.GetLength(0); i++)
      {
        buffer[i] = (Byte)Convert.ToInt32(byteStrings[i], 16);
      }

      try
      {
        _serialPort.Write(buffer, 0, buffer.GetLength(0));
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
