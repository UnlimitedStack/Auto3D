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
using MediaPortal.GUI.Library;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    public partial class SonyTVSetup : UserControl, IAuto3DUPnPSetup
    {
        SonyTV _device;

        public SonyTVSetup(IAuto3D device)
        {
            InitializeComponent();

            if (!(device is SonyTV))
                throw new Exception("Auto3D: Device is no SonyTV");

            _device = (SonyTV)device;

            buttonRegister.Enabled = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void ServiceAdded(Auto3DUPnPService service)
        {
            comboBoxTV.Items.Add(service);

            foreach (Auto3DUPnPService item in comboBoxTV.Items)
            {
                if (item.UniqueDeviceName == _device.UDN)
                {
                    comboBoxTV.SelectedItem = item;
                    break;
                }
            }

            if (comboBoxTV.SelectedIndex == -1)
                comboBoxTV.SelectedItem = service;

            listBoxCompatibleModels.Items.Clear();

            foreach (String model in _device.SelectedDeviceModel.CompatibleModels)
            {
                listBoxCompatibleModels.Items.Add("- " + model);
            }
        }

        public void ServiceRemoved(Auto3DUPnPService service)
        {
            foreach (Auto3DUPnPService srv in comboBoxTV.Items)
            {
                if (srv.IP == service.IP)
                {
                    comboBoxTV.Items.Remove(srv);
                }
            }
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
     
        private void comboBoxTV_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonRegister.Enabled = true;
            _device.UDN = ((Auto3DUPnPService)comboBoxTV.SelectedItem).UniqueDeviceName;
        }

        private void comboBoxModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            _device.SelectedDeviceModel = (Auto3DDeviceModel)comboBoxModel.SelectedItem;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            Auto3DUPnPService service = (Auto3DUPnPService)comboBoxTV.SelectedItem;
            _device.RegisterClient(service.IP + ":80");     
        }
    }
}
