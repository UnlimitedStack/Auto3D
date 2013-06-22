using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MediaPortal.GUI.Library;
using System.Xml;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    public class RemoteCommandSequences
    {
        XmlNode _rootNode;

        List<String> _commands2D3DSBS = new List<String>();
        List<String> _commands3DSBS2D = new List<String>();
        List<String> _commands2D3DTAB = new List<String>();
        List<String> _commands3DTAB2D = new List<String>();
        List<String> _commands2D3D = new List<String>();
        List<String> _commands3D2D = new List<String>();

        public RemoteCommandSequences()
        {            
        }

        public String Name
        {
            get;
            set;
        }

        public String BasePath
        {
            get;
            set;
        }

        public bool Modified
        {
            get;
            set;
        }

        public List<String> Commands2D3DSBS
        {
            get { return _commands2D3DSBS; }
        }

        public List<String> Commands3DSBS2D
        {
            get { return _commands3DSBS2D; }
        }

        public List<String> Commands2D3DTAB
        {
            get { return _commands2D3DTAB; }
        }

        public List<String> Commands3DTAB2D
        {
            get { return _commands3DTAB2D; }
        }

        public List<String> Commands2D3D
        {
            get { return _commands2D3D; }
        }

        public List<String> Commands3D2D
        {
            get { return _commands3D2D; }
        }      
     
        public void ReadCommands(XmlNode node)
        {
            _rootNode = node;
                 
            foreach (XmlNode listNode in node.ChildNodes)
            {
                switch (listNode.Name)
                {
                    case "Commands2D3DSBS":

                        ReadList(_commands2D3DSBS, listNode);
                        break;

                    case "Commands3DSBS2D":

                        ReadList(_commands3DSBS2D, listNode);
                        break;


                    case "Commands2D3DTAB":

                        ReadList(_commands2D3DTAB, listNode);
                        break;

                    case "Commands3DTAB2D":

                        ReadList(_commands3DTAB2D, listNode);
                        break;

                    case "Commands2D3D":

                        ReadList(_commands2D3D, listNode);
                        break;

                    case "Commands3D2D":

                        ReadList(_commands3D2D, listNode);
                        break;
                }
            }
        }

        private void ReadList(List<String> list, XmlNode node)
        {
            foreach (XmlNode command in node.ChildNodes)
            {
                list.Add(command.InnerText);
            }
        }

        public void WriteCommands()
        {
            foreach (XmlNode listNode in _rootNode.ChildNodes)
            {
                switch (listNode.Name)
                {
                    case "Commands2D3DSBS":

                        WriteList(_commands2D3DSBS, listNode);
                        break;

                    case "Commands3DSBS2D":

                        WriteList(_commands3DSBS2D, listNode);
                        break;


                    case "Commands2D3DTAB":

                        WriteList(_commands2D3DTAB, listNode);
                        break;

                    case "Commands3DTAB2D":

                        WriteList(_commands3DTAB2D, listNode);
                        break;

                    case "Commands2D3D":

                        WriteList(_commands2D3D, listNode);
                        break;

                    case "Commands3D2D":

                        WriteList(_commands3D2D, listNode);
                        break;
                }
            }
        }

        private void WriteList(List<String> list, XmlNode node)
        {
            XmlElement element = (XmlElement)node;
            element.RemoveAll();

            foreach (String command in list)
            {
                XmlElement child = node.OwnerDocument.CreateElement("Command");
                child.InnerText = command;
                element.AppendChild(child);
            }
        }
    }
}
