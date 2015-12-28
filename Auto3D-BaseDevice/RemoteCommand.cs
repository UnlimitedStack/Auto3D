using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using MediaPortal.GUI.Library;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public class RemoteCommand
  {
    public RemoteCommand(XmlNode node)
    {
		try
		{
			Command = node.ChildNodes.Item(0).InnerText;
			IrCode = node.ChildNodes.Item(1).InnerText;
			Delay = int.Parse(node.ChildNodes.Item(2).InnerText);
		}
		catch (Exception ex)
		{
			Log.Error("Auto3D: Error reading node: " + ex.Message);
		}
	}

    public RemoteCommand(String command, int delay, String code)
    {
      Command = command;
      Delay = delay;
	  IrCode = code;
    }

    public String Command
    {
      get;
      set;
    }

    public int Delay
    {
      get;
      set;
    }

	public String IrCode
	{
	  get;
	  set;
	}

    public override string ToString()
    {
      return Command;
    }
  }
}
