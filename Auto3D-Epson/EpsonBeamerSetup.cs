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
using System.IO.Ports;
using System.Xml;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    public partial class EpsonBeamerSetup : UserControl, IAuto3DSetup
    {
        EpsonBeamer _device;

        public EpsonBeamerSetup(IAuto3D device)
        {
            InitializeComponent();

            if (!(device is EpsonBeamer))
                throw new Exception("Auto3D: Device is no EpsonBeamer");

            _device = (EpsonBeamer)device;

            comboBoxPort.Items.Add("None");
            comboBoxPort.Items.AddRange(SerialPort.GetPortNames());
        }

        public IAuto3D GetDevice()
        {
            return _device;
        }

        public void LoadSettings()
        {
            comboBoxModel.Items.Clear();

            foreach (Auto3DDeviceModel model in _device.DeviceModels)
            {
                comboBoxModel.Items.Add(model);
            }

            comboBoxModel.SelectedItem = _device.SelectedDeviceModel;            
            
            foreach (object port in comboBoxPort.Items)
            {
                if (port.ToString() == _device.PortName)
                {
                    comboBoxPort.SelectedItem = port;
                    break;
                }
            }

            listBoxCompatibleModels.Items.Clear();

            foreach (String model in _device.SelectedDeviceModel.CompatibleModels)
            {
                listBoxCompatibleModels.Items.Add("- " + model);
            }
        }

        public void SaveSettings()
        {            
            _device.SaveSettings();
        }
     
        private void comboBoxModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            _device.SelectedDeviceModel = (Auto3DDeviceModel)comboBoxModel.SelectedItem;
        }

        private void comboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            _device.PortName = comboBoxPort.SelectedItem.ToString();            
        }
    }
}
