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
using MediaPortal.ProcessPlugins.Auto3D.UPnP;

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

    public void ServiceAdded(UPnPService service)
    {
      comboBoxTV.Items.Add(service);

      foreach (UPnPService item in comboBoxTV.Items)
      {
        if (item.ParentDevice.UDN == _device.UDN)
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

    public void ServiceRemoved(UPnPService service)
    {
      for (int i = 0; i < comboBoxTV.Items.Count; i++)
      {
        UPnPService srv = (UPnPService)comboBoxTV.Items[i];

        if (srv.ParentDevice.WebAddress.Host == service.ParentDevice.WebAddress.Host)
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
    }

    public void SaveSettings()
    {
      _device.SaveSettings();
    }

    private void comboBoxTV_SelectedIndexChanged(object sender, EventArgs e)
    {
      buttonRegister.Enabled = true;
      _device.UDN = ((UPnPService)comboBoxTV.SelectedItem).ParentDevice.UDN;
    }

    private void comboBoxModel_SelectedIndexChanged(object sender, EventArgs e)
    {
      _device.SelectedDeviceModel = (Auto3DDeviceModel)comboBoxModel.SelectedItem;
    }

    private void buttonRegister_Click(object sender, EventArgs e)
    {
      UPnPService service = (UPnPService)comboBoxTV.SelectedItem;
      _device.RegisterClient(service.ParentDevice.WebAddress.Host + ":80");
    }
  }
}
