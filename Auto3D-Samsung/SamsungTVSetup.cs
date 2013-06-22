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
using MediaPortal.ProcessPlugins.Auto3D.Samsung.iRemoteWrapper;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    public partial class SamsungTVSetup : UserControl, IAuto3DSetup
    {
        SamsungTV _device;

        public SamsungTVSetup(IAuto3D device)
        {
            InitializeComponent();

            if (!(device is SamsungTV))
                throw new Exception("Auto3D: Device is no SamsungTV");

            _device = (SamsungTV)device;
        }

        public IAuto3D GetDevice()
        {
            return _device;
        }

        public void TVAdded(ref Samsung.iRemoteWrapper.TVInfo info)
        {
            comboBoxTV.Items.Add(info);

            String tv = comboBoxTV.Items[0].ToString();

            using (Settings reader = new MPSettings())
            {
                tv = reader.GetValueAsString("Auto3DPlugin", "SamsungAddress", info.ToString());
            }

            foreach (Samsung.iRemoteWrapper.TVInfo item in comboBoxTV.Items)
            {
                if (item.ToString() == info.ToString())
                {
                    comboBoxTV.SelectedItem = item;
                    break;
                }
            }

            comboBoxTV.SelectedItem = info;
        }

        public void TVRemoved(ref Samsung.iRemoteWrapper.TVInfo info)
        {
            int i = comboBoxTV.FindStringExact(info.ToString());

            if (i > 0)
                comboBoxTV.Items.RemoveAt(i);
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
            Samsung.iRemoteWrapper.TVInfo info = (Samsung.iRemoteWrapper.TVInfo)comboBoxTV.SelectedItem;

            if (_device.iRemote.CurrentTV.ToString() != info.ToString())
            {
                _device.iRemote.ConnectTo((Samsung.iRemoteWrapper.TVInfo)comboBoxTV.SelectedItem);
                _device.IPAddress = info.ToString();
            }
        }

        private void comboBoxModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            _device.SelectedDeviceModel = (Auto3DDeviceModel)comboBoxModel.SelectedItem;
        }
    }
}
