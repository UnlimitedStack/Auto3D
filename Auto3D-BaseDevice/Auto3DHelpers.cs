using MediaPortal.GUI.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
        try
        {
			Form mainForm = Auto3DHelpers.GetMainForm();

			if (Auto3DHelpers.GetMainForm().IsDisposed)
				return;

			if (mainForm.InvokeRequired)
            {
                mainForm.Invoke(new ShowMessageDelegate(ShowAuto3DMessage), msg, forceMPGUI, seconds);
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
        catch (Exception ex)
        {
            Log.Error("ShowAuto3DMessage failed: " + ex.Message);
        }
    }

    public delegate void ShowMessageDelegate(String msg, bool forceMPGUI, int seconds);

    public static bool Ping(String ipAddress)
    {
        try
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            options.DontFragment = true;
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;

            PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);

            return reply.Status == IPStatus.Success;
        }
        catch (Exception ex)
        {
            Log.Error("Auto3D: Ping failed - " + ex.Message);
            return false;
        }        
    }

	[DllImport("iphlpapi.dll")]
	private static extern int SendARP(int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen);

	/// <summary>
	/// Requests the MAC address using Address Resolution Protocol
	/// </summary>
	/// <param name="IP">The IP.</param>
	/// <returns>the MAC address</returns>
	public static string RequestMACAddress(string IP)
	{
		try
		{
			if (Ping(IP))
			{
				Log.Info("Auto3D: Request MAC-Address for IP: " + IP);				

				IPAddress addr = IPAddress.Parse(IP);

				byte[] mac = new byte[6];
				int length = mac.Length;

				SendARP(BitConverter.ToInt32(addr.GetAddressBytes(), 0), 0, mac, ref length);
				string macAddress = BitConverter.ToString(mac, 0, length);

				if (macAddress.Length > 0)
				{
					Log.Info("Auto3D: MAC: " + macAddress);				
					return macAddress;
				}
				else
					Log.Info("Auto3D: Failed to get MAC-Address : ");				
			}
		}
		catch (Exception ex)
		{
			Log.Info("Auto3D: Request MAC Error: " + ex.Message);				
		}

		return "00-00-00-00-00-00";
	}

	public static void WakeOnLan(String macStr)
	{
		string[] Temp = macStr.Split('-');
		int Length = Temp.Length;
		byte[] mac = new byte[Length];
		
		for (int i = 0; i < Length; i++)
			mac[i] = Convert.ToByte(Temp[i], 16);
		
		// WOL packet is sent over UDP 255.255.255.0:40000.
		UdpClient client = new UdpClient();
		client.Connect(IPAddress.Broadcast, 40000);

		// WOL packet contains a 6-bytes trailer and 16 times a 6-bytes sequence containing the MAC address.
		byte[] packet = new byte[17 * 6];

		// Trailer of 6 times 0xFF.
		for (int i = 0; i < 6; i++)
			packet[i] = 0xFF;

		// Body of magic packet contains 16 times the MAC address.
		for (int i = 1; i <= 16; i++)
			for (int j = 0; j < 6; j++)
				packet[i * 6 + j] = mac[j];

		// Send WOL packet.
		client.Send(packet, packet.Length);
	}
  }
}
