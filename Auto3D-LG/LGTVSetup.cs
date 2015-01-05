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
  public partial class LGTVSetup : UserControl, IAuto3DUPnPSetup
  {
    LGTV _device;

    public LGTVSetup(IAuto3D device)
    {
      InitializeComponent();

      if (!(device is LGTV))
        throw new Exception("Auto3D: Device is no LGTV");

      _device = (LGTV)device;

      WebOS.UpdatePairingKey += WebOS_UpdatePairingKey;
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
          Log.Info("Auto3D: LG service selected -> " + service.ParentDevice.UDN);
          comboBoxTV.SelectedItem = item;
          break;
        }
      }

      if (comboBoxTV.SelectedIndex == -1)
      {
        Log.Info("Auto3D: LG service selected as default -> " + service.ParentDevice.UDN);
        comboBoxTV.SelectedItem = service;
      }

      listBoxCompatibleModels.Items.Clear();

      foreach (String model in _device.SelectedDeviceModel.CompatibleModels)
      {
        listBoxCompatibleModels.Items.Add(" " + model);
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
        listBoxCompatibleModels.Items.Add(" " + model);
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

      switch (comboBoxModel.SelectedIndex)
      {
          case 0:
            
              UDAPnP.Protocol = UDAPnP.LGProtocol.LG2011;
              buttonShowKey.Text = "Show key on TV";
              break;
      
          case 1:
            
              UDAPnP.Protocol = UDAPnP.LGProtocol.LG2012x;
              buttonShowKey.Text = "Show key on TV";
              break;

          case 2:

              UDAPnP.Protocol = UDAPnP.LGProtocol.WebOS;
              buttonShowKey.Text = "Show pair request on TV";
              break;
    }

      textBoxPairingKey.Enabled = (UDAPnP.Protocol != UDAPnP.LGProtocol.WebOS);
      buttonSendKey.Enabled = (UDAPnP.Protocol != UDAPnP.LGProtocol.WebOS);

      listBoxCompatibleModels.Items.Clear();

      foreach (String model in _device.SelectedDeviceModel.CompatibleModels)
      {
        listBoxCompatibleModels.Items.Add(" " + model);
      }

      _device.SaveSettings();

      if (comboBoxTV.SelectedIndex > -1)
        _device.ConnectAndPair();
    }

    private void comboBoxTV_SelectedIndexChanged(object sender, EventArgs e)
    {
      buttonShowKey.Enabled = true;

      // as we do not send our commands via the Intel UPnP library we have to
      // store the IP for our own calls from the device

      _device.IPAddress = ((UPnPService)comboBoxTV.SelectedItem).ParentDevice.WebAddress.Host;
      _device.UDN = ((UPnPService)comboBoxTV.SelectedItem).ParentDevice.UDN;
    }

    private void buttonShowKey_Click(object sender, EventArgs e)
    {
        if (UDAPnP.Protocol == UDAPnP.LGProtocol.LG2011 ||
            UDAPnP.Protocol == UDAPnP.LGProtocol.LG2012x)
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
        else // WebOS
        {
            textBoxPairingKey.Text = WebOS.Pair(_device.IPAddress);
            SaveSettings();
        }
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

    void WebOS_UpdatePairingKey(object sender, string key)
    {
        textBoxPairingKey.Text = key;
        SaveSettings();
        _device.ConnectAndPair();
    }
  }
}
