
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
	public class DiVineAdapter : IPhilipsTVAdapter
	{
		private static readonly Dictionary<string, DiVine.RC6Codes> _keys = new Dictionary<string, DiVine.RC6Codes>
                                                                       {
                                                                           { "Home", DiVine.RC6Codes.rc6S0MenuOn },
                                                                           { "Adjust", DiVine.RC6Codes.rc6S0AmbLightMode },
                                                                           { "Back", DiVine.RC6Codes.rc6S0PreviousProgram },
                                                                           { "Options", DiVine.RC6Codes.rc6S0Options },
                                                                           { "OK", DiVine.RC6Codes.rc6S0Acknowledge },
                                                                           { "CursorLeft", DiVine.RC6Codes.rc6S0StepLeft },
                                                                           { "CursorRight", DiVine.RC6Codes.rc6S0StepRight },
                                                                           { "CursorUp", DiVine.RC6Codes.rc6S0StepUp },
                                                                           { "CursorDown", DiVine.RC6Codes.rc6S0StepDown },
                                                                           { "3D", DiVine.RC6Codes.rc6S0Display3D },
                                                                           { "Delay", DiVine.RC6Codes.rc6S0 },
                                                                           { "Off", DiVine.RC6Codes.rc6S0SystemStandby },
                                                                       };

		public bool SendCommand(string command)
		{
			DiVine.RC6Codes key;
			if (_keys.TryGetValue(command, out key))
			{
				DiVine.SendKeyEx(key);
				return true;
			}

			return false;
		}

		public void Connect(string host)
		{
			IPAddress address;
			if (IPAddress.TryParse(host, out address) && !address.Equals(IPAddress.Any) && !address.Equals(IPAddress.Broadcast))
			{
				DiVine.Init(host);
			}
		}

		public SystemBase TestConnection(string host)
		{
			return new DiVineSystem
			{
				country = string.Empty,
				name = "DiVine TV"
			};
		}

		public void Disconnect()
		{
			if (DiVine.IsConnected)
				DiVine.Exit();
		}

		public bool IsConnected
		{
			get
			{
				return DiVine.IsConnected;
			}
		}
	}

}
