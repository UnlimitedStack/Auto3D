using MediaPortal.GUI.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public class Auto3DHelpers
  {
    public static System.Windows.Forms.Form GetMainForm()
    {
      foreach (System.Windows.Forms.Form form in System.Windows.Forms.Application.OpenForms)
      {
        if (form.Name == "D3DApp" || form.Name == "D3D" || form.Name == "SettingsForm")
          return form;
      }

      return null;
    }

    public static bool IsInSettings()
    {
      foreach (System.Windows.Forms.Form form in System.Windows.Forms.Application.OpenForms)
      {
        if (form.Name == "SettingsForm")
          return true;
      }

      return false;
    }

    public static void ShowAuto3DMessage(String msg, bool forceMPGUI, int seconds)
    {
        if (Auto3DHelpers.GetMainForm().InvokeRequired)
        {
            Auto3DHelpers.GetMainForm().Invoke(new ShowMessageDelegate(ShowAuto3DMessage), msg, forceMPGUI, seconds);
            return;
        }

        if (GUIGraphicsContext.IsFullScreenVideo || forceMPGUI)
        {
            GUIMessage guiMsg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_REFRESHRATE_CHANGED, 0, 0, 0, 0, 0, null);
            guiMsg.Label = "Auto3D";
            guiMsg.Label2 = msg;
            guiMsg.Param1 = seconds;

            GUIGraphicsContext.SendMessage(guiMsg);
        }
        else
        {
            MessageBox.Show(msg, "Auto3D");
        }
    }

    public delegate void ShowMessageDelegate(String msg, bool forceMPGUI, int seconds);
  }
}
