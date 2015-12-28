using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
	public interface IPhilipsTVAdapter
	{
		bool SendCommand(string command);

		SystemBase Connect(string host);

		void Disconnect();

		bool IsConnected { get; }
	}

}
