using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    public class Auto3DDeviceModel
    {
        List<String> _compatibleModels = new List<String>();
        RemoteCommandSequences _remoteCommandSequences = new RemoteCommandSequences();        

        public Auto3DDeviceModel()
        {            
        }

        public Auto3DDeviceModel(XmlNode node)
        {
            Name = node.ChildNodes.Item(0).InnerText;

            foreach (XmlNode subNode in node.ChildNodes.Item(1).ChildNodes)
            {
                _compatibleModels.Add(subNode.InnerText);
            }

            _remoteCommandSequences.ReadCommands(node.ChildNodes.Item(2));
        }
       
        public String Name
        {
            get;
            set;
        }

        public String Interface
        {
            get;
            set;
        }

        public String DefaultInterface
        {
            get;
            set;
        }

        public List<String> CompatibleModels
        {
            get { return _compatibleModels; }
        }

        public RemoteCommandSequences RemoteCommandSequences
        {
            get
            {
                return _remoteCommandSequences;
            }
        }        
      
        public override string ToString()
        {
            return Name;
        }
    }
}
