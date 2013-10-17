using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MediaPortal.ServiceImplementations;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
    public class UDAPnP
    {
        public enum LGProtocol { LG2011, LG2012x };

        static LGProtocol _protocol;

        static String _port;
        static String _ipAddress;
        static String _sessionID;
        static String _lastWebResult;

        public static LGProtocol Protocol
        {
            get
            {
                return _protocol;
            }

            set
            {
                _protocol = value;

                if (_protocol == LGProtocol.LG2011)
                {
                    _port = "8080";
                }
                else
                {
                    _port = "6767";
                }
            }
        }

        public static bool Connected
        {
            get;
            private set;
        }
            
        static UDAPnP()
        {
            Protocol = LGProtocol.LG2012x;
        }

        public static bool UpdateServiceInformation(String scanIP)
        {
            Log.Info("Auto3D: UpdateServiceInfo with IP: "+ scanIP);

            Connected = false;

            try
            {
                if (Protocol == LGProtocol.LG2012x)
                {
                    String deviceDescriptionURL = "http://" + scanIP + ":6767/udap/api/data?target=netrcu.xml";

                    Log.Info("Auto3D: LG device description = " + deviceDescriptionURL);

                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(deviceDescriptionURL);
                    myReq.Timeout = 3000;
                    myReq.UserAgent = "UDAP/2.0";

                    using (WebResponse resp = myReq.GetResponse())
                    {
                        using (Stream receiveStream = resp.GetResponseStream())
                        {
                            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                            {
                                String deviceDescription = readStream.ReadToEnd();

                                String[] temp = deviceDescriptionURL.Substring(7).Split(':');
                                _ipAddress = temp[0];
                                Connected = true;
                                return true;
                            }
                        }
                    }                    
                }
                else
                {
                    _ipAddress = scanIP;
                    Connected = true;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Log.Error("Auto3D: UpdateServiceInfo failed: " + ex.Message);
            }

            return false;
        }

        private static String GetXMLTag(String buffer, String tag)
        {
            String beginTag = "<" + tag + ">";
            String endTag = "</" + tag + ">";

            int posStart = buffer.IndexOf(beginTag) + beginTag.Length;
            int posEnd = buffer.IndexOf(endTag);

            return buffer.Substring(posStart, posEnd - posStart);
        }

        /*public static bool GetVersion()
        {
            if (Protocol == LGProtocol.LG2011)
            {
                String versionURL = "http://" + _ipAddress + ":" + _port + "/hdcp/api/data?target=version_info";

                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(versionURL);
                myReq.Timeout = 3000;
                myReq.UserAgent = "UDAP/2.0";

                try
                {
                    using (HttpWebResponse resp = (HttpWebResponse)myReq.GetResponse())
                    {
                        using (Stream receiveStream = resp.GetResponseStream())
                        {
                            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                            {
                                _lastWebResult = readStream.ReadToEnd();

                                /*readStream.Close();
                                receiveStream.Close();
                                resp.Close();

                                return resp.StatusCode == HttpStatusCode.OK;
                            }
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    Log.Error("Auto3D: GetVersion failed: {0}", ex.Message);
                    return false;	
                }
            }
            else
                return true;
        }*/

        public static bool RequestPairingKey()
        {
            if (Protocol == LGProtocol.LG2012x)
                return HttpPost(_ipAddress, _port, "/udap/api/pairing", RequestPairingKeyXML());
            else
                return HttpPost(_ipAddress, _port, "/hdcp/api/auth", RequestPairingKeyXML());
        }

        public static bool RequestPairing(String pairingKey)
        {
            if (Protocol == LGProtocol.LG2012x)
                return HttpPost(_ipAddress, _port, "/udap/api/pairing", RequestPairingXML(pairingKey));
            else
            {
                if (HttpPost(_ipAddress, _port, "/hdcp/api/auth", RequestPairingXML(pairingKey)))
                {
                    if (_lastWebResult.Contains("session"))
                    {
                        _sessionID = GetXMLTag(_lastWebResult, "session");
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool HandleKeyInput(String keyInput)
        {
            if (Protocol == LGProtocol.LG2012x)
                return HttpPost(_ipAddress, _port, "/udap/api/command", HandleKeyInputXML("", keyInput));
            else
                return HttpPost(_ipAddress, _port, "/hdcp/api/dtv_wifirc", HandleKeyInputXML(_sessionID, keyInput));
        }

        private static bool HttpPost(String ip, String port, String path, String envelope)
        {
            String post = "http://" + ip + ":" + port + path;

            try
            {                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(post);

                request.Timeout = 3000;
                request.Method = "POST";

                if (Protocol == LGProtocol.LG2012x)
                {
                    request.Headers.Add("SOAPAction", "urn:schemas-udap:service:netrcu:1");
                    request.ContentType = "text/xml; charset=UTF-8";
                }
                else
                    request.ContentType = "application/atom+xml";

                request.UserAgent = "UDAP/2.0";

                byte[] byteArray = Encoding.UTF8.GetBytes(envelope);

                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();                

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            _lastWebResult = sr.ReadToEnd();
                            return response.StatusCode == HttpStatusCode.OK;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Auto3D: HttpPost ({0}) failed: {1}", post ,ex.Message);
            }

            return false;
        }

        private static string RequestPairingKeyXML()
        {
            StringBuilder msg = new StringBuilder();

            if (Protocol == LGProtocol.LG2012x)
            {
                msg.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                msg.AppendLine("<envelope>");
                msg.AppendLine("<api type=\"pairing\">");
                msg.AppendLine("<name>showKey</name>");
                msg.AppendLine("</api>");
                msg.AppendLine("</envelope>");
            }
            else
            {
                msg.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                msg.AppendLine("<auth>");
                msg.AppendLine("<type>AuthKeyReq</type>");
                msg.AppendLine("</auth>");
            }

            return msg.ToString();
        }

        private static string RequestPairingXML(String key)
        {
            StringBuilder msg = new StringBuilder();

            if (Protocol == LGProtocol.LG2012x)
            {
                msg.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                msg.AppendLine("<envelope>");
                msg.AppendLine("<api type=\"pairing\">");
                msg.AppendLine("<name>hello</name>");
                msg.AppendLine("<value>" + key + "</value>");
                msg.AppendLine("<port>6767</port>");
                msg.AppendLine("</api>");
                msg.AppendLine("</envelope>");
            }
            else
            {
                msg.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                msg.AppendLine("<auth>");
                msg.AppendLine("<type>AuthReq</type>");
                msg.AppendLine("<value>" + key + "</value>");
                msg.AppendLine("</auth>");
            }

            return msg.ToString();
        }

        private static string HandleKeyInputXML(String sessionID, String key)
        {
            StringBuilder msg = new StringBuilder();

            if (Protocol == LGProtocol.LG2012x)
            {
                msg.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                msg.AppendLine("<envelope>");
                msg.AppendLine("<api type=\"command\">");
                msg.AppendLine("<name>HandleKeyInput</name>");
                msg.AppendLine("<value>" + key + "</value>");
                msg.AppendLine("</api>");
                msg.AppendLine("</envelope>");
            }
            else
            {
                msg.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                msg.AppendLine("<command>");
                msg.AppendLine("<session>" + sessionID + "</session>");
                msg.AppendLine("<type>HandleKeyInput</type>");
                msg.AppendLine("<value>" + key + "</value>");
                msg.AppendLine("</command>");
            }

            return msg.ToString();
        }
    }
}
