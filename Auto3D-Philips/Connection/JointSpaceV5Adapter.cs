using MediaPortal.GUI.Library;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
	public class JointSpaceV5Adapter : JointSpaceBaseAdapter
	{
		private static readonly Dictionary<string, string> Keys = new Dictionary<string, string>
                                                                       {
                                                                           { "Home", "Home" },
                                                                           { "Adjust", "Adjust" },
                                                                           { "Back", "Back" },
                                                                           { "Options", "Options" },
                                                                           { "OK", "Confirm" },
                                                                           { "CursorLeft", "CursorLeft" },
                                                                           { "CursorRight", "CursorRight" },
                                                                           { "CursorUp", "CursorUp" },
                                                                           { "CursorDown", "CursorDown" },
                                                                           { "3D", "3dFormat" },
                                                                           { "Delay", string.Empty },
                                                                           { "Off", "Standby" },
                                                                       };

		public override bool SendCommand(string command)
		{
			string key;
			
			if (Keys.TryGetValue(command, out key))
			{
				return base.SendCommand(key);
			}
			else
			{
				Log.Info("Auto3D: Unknown command - " + command);
			}

			return false;
		}

		public override SystemBase TestConnection(string host)
		{
                        base.TestConnection(host);
			return JsonConvert.DeserializeObject<JointSpaceV5System>(GetRequest(SystemUri, string.Empty));
		}

		protected override string SystemUri
		{
			get
			{
				return string.Format(@"http://{0}:1925/5/system", Host);
			}
		}

		protected override string KeyUri
		{
			get
			{
				return string.Format(@"http://{0}:1925/5/input/key", Host);
			}
		}
	}
}
