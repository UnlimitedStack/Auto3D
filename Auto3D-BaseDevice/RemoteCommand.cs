using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MediaPortal.GUI.Library;
using System.Xml;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    public class RemoteCommand
    {
        public RemoteCommand(XmlNode node)
        {
            Command = node.ChildNodes.Item(0).InnerText;
            Delay = int.Parse(node.ChildNodes.Item(1).InnerText);
        }

        public RemoteCommand(String command, int delay)
        {
            Command = command;
            Delay = delay;
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

        public override string ToString()
        {
            return Command;
        }
    }
}
