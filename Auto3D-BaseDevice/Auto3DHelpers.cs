using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
  }
}
