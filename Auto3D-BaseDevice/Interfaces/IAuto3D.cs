using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.ProcessPlugins.Auto3D;
using System.Windows.Forms;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public enum VideoFormat { Fmt2D, Fmt3DSBS, Fmt3DTAB, Fmt2D3D };

  public interface IAuto3D
  {
    void Start();                                               // Sub-plugin is started (alloc resources)
    void Stop();                                                // Sub-plugin is stopped (release resources)

    void LoadSettings();                                        // Load all settings for the device
    void SaveSettings();                                        // Save all settings for the device

    UserControl GetSetupControl();                              // Return the setup subpage for the device (314 x 286 Pixel)
    UserControl GetRemoteControl();                             // Return the control with the command keys for the device (256 x 256 Pixel)        

    bool SwitchFormat(VideoFormat fmtOld, VideoFormat fmtNew);  // Switch device to specific format (normally handled by Auto3DBaseDevice)
    bool IsDefined(VideoFormat fmt);                            // Check if sequence for format is defined (normally handled by Auto3DBaseDevice)

    bool SendCommand(String rc);                                // Send a command to the device
  }
}
