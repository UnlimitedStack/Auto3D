using MediaPortal.GUI.Library;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
	public class JointSpaceV1Adapter : JointSpaceBaseAdapter
	{
		private static readonly Dictionary<string, string> _keys = new Dictionary<string, string>
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
                                                                           { "Delay", string.Empty },
                                                                           { "Off", "Standby" },
                                                                       };

		public override bool SendCommand(string command)
		{
			string key;
			
			if (_keys.TryGetValue(command, out key))
			{
				return base.SendCommand(key);
			}
			else
			{
				Log.Info("Auto3D: Unknown command - " + command);
			}

			return false;
		}

		public override SystemBase Connect(string host)
		{
			base.Connect(host);
			return JsonConvert.DeserializeObject<JointSpaceV1System>(GetRequest(SystemUri, string.Empty));
		}

		protected override string SystemUri
		{
			get
			{
				return string.Format(@"http://{0}:1925/1/system", Host);
			}
		}
		protected override string KeyUri
		{
			get
			{
				return string.Format(@"http://{0}:1925/1/input/key", Host);
			}
		}
	}
}
