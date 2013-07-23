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
    public partial class LGTVSetup : UserControl, IAuto3DUPnPSetup
    {
        LGTV _device;

        public LGTVSetup(IAuto3D device)
        {
            InitializeComponent();

            if (!(device is LGTV))
                throw new Exception("Auto3D: Device is no LGTV");

            _device = (LGTV)device;
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
                    Log.Info("Auto3D: LG service selected -> " + service.UniqueDeviceName);
                    comboBoxTV.SelectedItem = item;
                    break;
                }
            }

            if (comboBoxTV.SelectedIndex == -1)
            {
                Log.Info("Auto3D: LG service selected as default -> " + service.UniqueDeviceName);
                comboBoxTV.SelectedItem = service;
            }

            listBoxCompatibleModels.Items.Clear();

            foreach (String model in _device.SelectedDeviceModel.CompatibleModels)
            {
                listBoxCompatibleModels.Items.Add("- " + model);
            }
        }

        public void ServiceRemoved(Auto3DUPnPService service)
        {
            for (int i = 0; i < comboBoxTV.Items.Count; i++)
            {
                Auto3DUPnPService srv = (Auto3DUPnPService)comboBoxTV.Items[i];

                if (srv.IP == service.IP)
                {
                    comboBoxTV.Items.RemoveAt(i);
                    break;
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

            textBoxPairingKey.Text = _device.PairingKey;
        }

        public void SaveSettings()
        {
            _device.PairingKey = textBoxPairingKey.Text;
            _device.SaveSettings();
        }

        private void comboBoxModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            _device.SelectedDeviceModel = (Auto3DDeviceModel)comboBoxModel.SelectedItem;

            if (comboBoxModel.SelectedIndex == 0)
                UDAPnP.Protocol = UDAPnP.LGProtocol.LG2011;
            else
                UDAPnP.Protocol = UDAPnP.LGProtocol.LG2012x;

            _device.SaveSettings();
            _device.ConnectAndPair();
        }

        private void comboBoxTV_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonShowKey.Enabled = true;

            // as we do not send our commands via the Intel UPnP library we have to
            // store the IP for our own calls from the device

            _device.IPAddress = ((Auto3DUPnPService)comboBoxTV.SelectedItem).IP;
            _device.UDN = ((Auto3DUPnPService)comboBoxTV.SelectedItem).UniqueDeviceName;
        }

        private void buttonShowKey_Click(object sender, EventArgs e)
        {
            if (!UDAPnP.Connected)
            {
                if (!UDAPnP.UpdateServiceInformation(_device.IPAddress))
                {
                    MessageBox.Show("Connection to LG TV failed!");
                    return;
                }
            }

            UDAPnP.RequestPairingKey();
        }

        private void buttonSendKey_Click(object sender, EventArgs e)
        {
            if (!UDAPnP.Connected)
            {
                if (!UDAPnP.UpdateServiceInformation(_device.IPAddress))
                {
                    MessageBox.Show("Connection to LG TV failed!");
                    return;
                }
            }

            if (UDAPnP.RequestPairing(textBoxPairingKey.Text))
            {
                SaveSettings();
            }    
        }
    }
}
