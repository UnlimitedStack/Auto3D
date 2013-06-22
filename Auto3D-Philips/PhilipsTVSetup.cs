using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class PhilipsTVSetup : UserControl, IAuto3DSetup
    {
        PhilipsTV _device;

        public PhilipsTVSetup(IAuto3D device)
        {
            InitializeComponent();

            if (!(device is PhilipsTV))
                throw new Exception("Auto3D: Device is no PhilipsTV");

            _device = (PhilipsTV)device;
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

            textBoxIP.Text = _device.IPAddress;

            listBoxCompatibleModels.Items.Clear();

            foreach (String model in _device.SelectedDeviceModel.CompatibleModels)
            {
                listBoxCompatibleModels.Items.Add("- " + model);
            }

            comboBoxInterface.SelectedIndex = (int)_device.ConnectionMethod;
        }

        public void SaveSettings()
        {
            _device.IPAddress = textBoxIP.Text;
            _device.SaveSettings();
        }       

        private void comboBoxModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            _device.SelectedDeviceModel = (Auto3DDeviceModel)comboBoxModel.SelectedItem;
        }

        private void comboBoxInterface_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxInterface.SelectedIndex == 0)
            {
                btnCheckConnection.Visible = false;
                _device.ConnectionMethod = eConnectionMethod.jointSpace;
            }
            else
            {
                btnCheckConnection.Visible = true;
                _device.ConnectionMethod = eConnectionMethod.DirectFB;
            }

            ((IAuto3DKeypad)_device.GetRemoteControl()).UpdateState();
        }

        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            if (comboBoxInterface.SelectedIndex == 1)
            {
                if (DiVine.IsConnected)
                    DiVine.Exit();
                    
                if (DiVine.Init(_device.IPAddress))
                    MessageBox.Show("Connection successful!");
                else
                    MessageBox.Show("Connection failed!");
            }
        }

        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            _device.IPAddress = textBoxIP.Text;
        }
    }
}
