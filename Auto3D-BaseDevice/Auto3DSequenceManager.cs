using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MediaPortal.GUI.Library;
using System.Threading;
using MediaPortal.Profile;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public partial class Auto3DSequenceManager : Form, IAuto3DSequenceManager
  {
    static Auto3DSequenceManager _sequenceManager = null;

    Auto3DBaseDevice _device;

    List<ListBox> _lbList = new List<ListBox>();

    public Auto3DSequenceManager()
    {
      InitializeComponent();

      using (Settings reader = new MPSettings())
      {
        checkBoxSendOnAdd.Checked = reader.GetValueAsBool("Auto3DPluginSequenceManager", "SendCommandOnAdd", true);
      }
    }

    public static IAuto3DSequenceManager SequenceManager
    {
      get
      {
        return _sequenceManager;
      }

      set
      {
        _sequenceManager = (Auto3DSequenceManager)value;
      }
    }

    public void SetDevice(IAuto3D device)
    {
      _device = (Auto3DBaseDevice)device;
      textBoxDeviceName.Text = _device.SelectedDeviceModel.Name;

      InternalCommandSetToListBox(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3DSBS, listBox2D3DSBS);
      InternalCommandSetToListBox(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3DSBS2D, listBox3DSBS2D);
      InternalCommandSetToListBox(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3DTAB, listBox2D3DTAB);
      InternalCommandSetToListBox(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3DTAB2D, listBox3DTAB2D);
      InternalCommandSetToListBox(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3D, listBox2D3D);
      InternalCommandSetToListBox(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3D2D, listBox3D2D);

      _lbList.Add(listBox2D3DSBS);
      _lbList.Add(listBox3DSBS2D);
      _lbList.Add(listBox2D3DTAB);
      _lbList.Add(listBox3DTAB2D);
      _lbList.Add(listBox2D3D);
      _lbList.Add(listBox3D2D);

      panelKeyPad.Controls.Add(device.GetRemoteControl());
      device.GetRemoteControl().Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;

      AddButtonEventHandlersRecursive(device.GetRemoteControl());

      ToolTip toolTip = new ToolTip();
      toolTip.SetToolTip(buttonDELETE, "Delete command");

      toolTip = new ToolTip();
      toolTip.SetToolTip(buttonListUp, "Move command up");

      toolTip = new ToolTip();
      toolTip.SetToolTip(buttonListDown, "Move command down");

      toolTip = new ToolTip();
      toolTip.SetToolTip(buttonDelay, "Delay command");

      toolTip = new ToolTip();
      toolTip.SetToolTip(buttonTest, "Test current sequence");
    }

    private void AddButtonEventHandlersRecursive(Control control)
    {
      foreach (Control button in control.Controls)
      {
        if (button is Button)
        {
          button.Click += button_Click;
        }
        else
          AddButtonEventHandlersRecursive(button);
      }
    }

    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);
      RemoveButtonEventHandlersRecursive(_device.GetRemoteControl());
    }

    private void RemoveButtonEventHandlersRecursive(Control control)
    {
      foreach (Control button in control.Controls)
      {
        if (button is Button)
        {
          button.Click -= button_Click;
        }
        else
          RemoveButtonEventHandlersRecursive(button);
      }
    }

    void button_Click(object sender, EventArgs e)
    {
      Auto3DSequenceManager.SequenceManager.AddCommand(((Button)sender).Tag.ToString());
    }

    protected override void WndProc(ref Message m)
    {
      switch (m.Msg)
      {
        case 0x84:
          base.WndProc(ref m);
          if ((int)m.Result == 0x1)
            m.Result = (IntPtr)0x2;
          return;
      }

      base.WndProc(ref m);
    }

    public void InternalCommandSetToListBox(List<String> list, ListBox lb)
    {
      lb.Items.Clear();

      for (int i = 0; i < list.Count; i++)
      {
        lb.Items.Add(list[i]);
      }

      lb.Items.Add("");
      lb.SelectedIndex = 0;
    }

    public void InternalCheckForDifference(List<String> list, ListBox lb)
    {
      if (list.Count != lb.Items.Count - 1)
      {
        _device.Modified = true;
        return;
      }

      for (int i = 0; i < lb.Items.Count - 1; i++)
      {
        if (lb.Items[i].ToString() != list[i])
          _device.Modified = true;
      }
    }

    public void InternalListBoxToCommandset(List<String> list, ListBox lb)
    {
      list.Clear();

      for (int i = 0; i < lb.Items.Count - 1; i++)
      {
        list.Add(lb.Items[i].ToString());
      }
    }

    public void AddCommand(String cmd)
    {
      try
      {
        _lbList[tabControl.SelectedIndex].Items.Insert(_lbList[tabControl.SelectedIndex].SelectedIndex, cmd);

        if (checkBoxSendOnAdd.Checked)
          _device.SendCommand(cmd);
      }
      catch (Exception ex)
      {
        Log.Info("Auto3D: " + ex.Message);
      }
    }

    private void buttonDELETE_Click(object sender, EventArgs e)
    {
      if (_lbList[tabControl.SelectedIndex].SelectedIndex == -1)
        return;

      int index = _lbList[tabControl.SelectedIndex].SelectedIndex;

      if (index < _lbList[tabControl.SelectedIndex].Items.Count - 1)
      {
        _lbList[tabControl.SelectedIndex].Items.RemoveAt(_lbList[tabControl.SelectedIndex].SelectedIndex);

        if (index < _lbList[tabControl.SelectedIndex].Items.Count - 1)
          _lbList[tabControl.SelectedIndex].SelectedIndex = index;
        else
          _lbList[tabControl.SelectedIndex].SelectedIndex = _lbList[tabControl.SelectedIndex].Items.Count - 1;
      }
    }

    private void buttonListUp_Click(object sender, EventArgs e)
    {
      if (_lbList[tabControl.SelectedIndex].SelectedIndex == -1)
        return;

      int index = _lbList[tabControl.SelectedIndex].SelectedIndex;

      if (index > 0 && index < _lbList[tabControl.SelectedIndex].Items.Count - 1)
      {
        Object item = _lbList[tabControl.SelectedIndex].Items[index];
        _lbList[tabControl.SelectedIndex].Items.RemoveAt(index);
        _lbList[tabControl.SelectedIndex].Items.Insert(index - 1, item);
        _lbList[tabControl.SelectedIndex].SelectedIndex = index - 1;
      }
    }

    private void buttonListDown_Click(object sender, EventArgs e)
    {
      if (_lbList[tabControl.SelectedIndex].SelectedIndex == -1)
        return;

      int index = _lbList[tabControl.SelectedIndex].SelectedIndex;

      if (index < _lbList[tabControl.SelectedIndex].Items.Count - 2)
      {
        Object item = _lbList[tabControl.SelectedIndex].Items[index];
        _lbList[tabControl.SelectedIndex].Items.RemoveAt(index);
        _lbList[tabControl.SelectedIndex].Items.Insert(index + 1, item);
        _lbList[tabControl.SelectedIndex].SelectedIndex = index + 1;
      }
    }


    private void buttonTest_Click(object sender, EventArgs e)
    {
      if (_lbList[tabControl.SelectedIndex].SelectedIndex == -1)
        return;

      buttonTest.Enabled = false;

      for (int i = 0; i < _lbList[tabControl.SelectedIndex].Items.Count - 1; i++)
      {
        _lbList[tabControl.SelectedIndex].SelectedIndex = i;
        _lbList[tabControl.SelectedIndex].Refresh();

        String cmd = _lbList[tabControl.SelectedIndex].Items[i].ToString();

        if (!_device.SendCommand(cmd))
          break;

        Thread.Sleep(_device.GetRemoteCommandFromString(cmd).Delay);
      }

      _lbList[tabControl.SelectedIndex].SelectedIndex = 0;
      _lbList[tabControl.SelectedIndex].Refresh();

      buttonTest.Enabled = true;
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      InternalSave();

      using (Settings writer = new MPSettings())
      {
        writer.SetValue("Auto3DPluginSequenceManager", "SendCommandOnAdd", checkBoxSendOnAdd.Checked);
      }

      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void InternalSave()
    {
      if (textBoxDeviceName.Text != _device.SelectedDeviceModel.Name)
      {
        if (MessageBox.Show("Save new settings as " + textBoxDeviceName.Text + "?", "Save TV Settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3DSBS, listBox2D3DSBS);
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3DSBS2D, listBox3DSBS2D);
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3DTAB, listBox2D3DTAB);
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3DTAB2D, listBox3DTAB2D);
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3D, listBox2D3D);
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3D2D, listBox3D2D);

          _device.SelectedDeviceModel.RemoteCommandSequences.WriteCommands();
          _device.SaveDocument();
        }
      }
      else
      {
        InternalCheckForDifference(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3DSBS, listBox2D3DSBS);
        InternalCheckForDifference(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3DSBS2D, listBox3DSBS2D);
        InternalCheckForDifference(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3DTAB, listBox2D3DTAB);
        InternalCheckForDifference(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3DTAB2D, listBox3DTAB2D);
        InternalCheckForDifference(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3D, listBox2D3D);
        InternalCheckForDifference(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3D2D, listBox3D2D);

        if (_device.Modified && (MessageBox.Show("Save modified settings ?", "Save TV Settings", MessageBoxButtons.YesNo) == DialogResult.Yes))
        {
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3DSBS, listBox2D3DSBS);
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3DSBS2D, listBox3DSBS2D);
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3DTAB, listBox2D3DTAB);
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3DTAB2D, listBox3DTAB2D);
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands2D3D, listBox2D3D);
          InternalListBoxToCommandset(_device.SelectedDeviceModel.RemoteCommandSequences.Commands3D2D, listBox3D2D);

          _device.SelectedDeviceModel.RemoteCommandSequences.WriteCommands();
          _device.SaveDocument();
        }

        _device.Modified = false;
      }

      RemoveButtonEventHandlersRecursive(_device.GetRemoteControl());
      panelKeyPad.Controls.Remove(_device.GetRemoteControl());
    }

    private void buttonCommandTimings_Click(object sender, EventArgs e)
    {
      Auto3DTimings timings = new Auto3DTimings(_device);
      timings.ShowDialog();
    }

    private void buttonDelay_Click(object sender, EventArgs e)
    {
      Auto3DSequenceManager.SequenceManager.AddCommand("Delay");
    }
  }
}
