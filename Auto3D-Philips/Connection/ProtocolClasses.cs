namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
	public class SystemBase
	{
		public string name { get; set; }
		public string country { get; set; }
	}

	public class DiVineSystem : SystemBase
	{
	}

	public class JointSpaceV1System : SystemBase
	{
		public string menulanguage { get; set; }
		public string serialnumber { get; set; }
		public string softwareversion { get; set; }
		public string model { get; set; }
	}

	public class JointSpaceV5System : SystemBase
	{
		public string menulanguage { get; set; }
		public string serialnumber_encrypted { get; set; }
		public string nettvversion { get; set; }
		public string softwareversion_encrypted { get; set; }
		public string model_encrypted { get; set; }
		public string deviceid_encrypted { get; set; }
	}

	public class JointSpaceKey
	{
		public string key { get; set; }
	}
}

