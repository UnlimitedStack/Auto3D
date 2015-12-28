using MediaPortal.GUI.Library;
using MediaPortal.ProcessPlugins.Auto3D.UPnP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices.SonyAPI_Lib
{
    public class SonyDevice
    {
        public string actionListURL;

        private CookieContainer allcookies = new CookieContainer();
        private SonyCommandList dataSet = new SonyCommandList();

        /// <summary>
        /// This method Checks the current Status of the device Registration
        /// </summary>
        /// <returns>Returns a Bool True or False</returns>
        private bool checkReg()
        {
            bool results = false;

            // Gen 1 or 2 Devices
            if (Generation <= 2)
            {
                Log.Debug("Auto3D: Verifing Registration for: " + Name);
                
                // Will NOT return a Status if not Registered
                if (checkStatus() != "")
                {
                    Registered = true;
                    results = true;
                }
            }
            else
            {
                // Generation 3 devices uses a Cookie
                try
                {
                    // Check and Load cookie. If Found then Registration = True
                    
                    Log.Debug("Auto3D: Checking for Generation 3 Cookie");

                    System.IO.StreamReader myFile = new System.IO.StreamReader("cookie.json");
                    string myString = myFile.ReadToEnd();
                    myFile.Close();
                    List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(myString);
                    allcookies.Add(new Uri(@"http://" + DeviceIPAddress + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
                    Cookie = myString;
                    Registered = true;
                    results = true;
                    Log.Debug("Auto3D: " + Name + ": Cookie found: auth=" + Cookie);                   
                }
                catch
                {
                    Log.Error("Auto3D: No Cookie was found");
                    
                    results = false;
                    Registered = false;
                }
            }

            Log.Debug("Auto3D: " + Name + ": Registration Check returned: " + results.ToString());                               
            return results;
        }

        /// <summary>
        /// This method Gets the current Status of the device
        /// </summary>
        /// <returns>Returns the device response as a string</returns>
        public string checkStatus()
        {
            string retstatus = "";

            if (Generation != 3)
            {
                try
                {
                    Log.Debug("Auto3D: Checking Status of Device " + Name);
                    
                    string cstatus;
                    int x;
                    cstatus = HttpGet(getActionlist("getStatus", actionListURL));
                    cstatus = cstatus.Replace("/n", "");
                    x = cstatus.IndexOf("name=");
                    cstatus = cstatus.Substring(x + 6);
                    x = cstatus.IndexOf("\"");
                    string sname = cstatus.Substring(0, x);
                    cstatus = cstatus.Substring(x);
                    x = cstatus.IndexOf("value=");
                    cstatus = cstatus.Substring(x + 7);
                    x = cstatus.IndexOf("\"");
                    string sval = cstatus.Substring(0, x);
                    retstatus = sname + ":" + sval;

                    Log.Debug("Auto3D: Device returned a Status of: " + retstatus);
                }
                catch (Exception ex)
                {
                    Log.Error("Auto3D: Checking Device Status for " + Name + " failed!");
                    Log.Error("Auto3D: " + ex.Message);
                    retstatus = "";
                }
            }
            else
            {
                try
                {
                    Log.Debug("Auto3D: Checking Status of Device " + Name);
                    
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + DeviceIPAddress + @"/sony/system");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\"id\":19,\"method\":\"getPowerStatus\",\"version\":\"1.0\",\"params\":[]}\"";
                        streamWriter.Write(json);
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var responseText = streamReader.ReadToEnd();
                        dataSet = JsonConvert.DeserializeObject<SonyCommandList>(responseText);
                    }
                    string first = dataSet.result[0].ToString();
                    first = first.Replace("{", "");
                    first = first.Replace("\"", "");
                    retstatus = first;
                    
                    Log.Debug("Auto3D: Device returned a Status of: " + retstatus);
                }
                catch (Exception ex)
                {
                    Log.Error("Auto3D: Checking Device Status for " + Name + " failed!");
                    Log.Error("Auto3D: " + ex.Message);
                    retstatus = "";
                }
            }

            return retstatus;
        }

        /// <summary>
        /// Method used to retrieve Gen3 Devices Mac Address
        /// </summary>
        /// <returns></returns>
        private string findDevMac()
        {
            String macaddress = "";
            
            Log.Debug("Auto3D: Retrieving the Mac Address from: " + Name + " at IP: " + DeviceIPAddress);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + DeviceIPAddress + @"/sony/system");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"id\":19,\"method\":\"getSystemSupportedFunction\",\"version\":\"1.0\",\"params\":[]}\"";
                streamWriter.Write(json);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                dataSet = JsonConvert.DeserializeObject<SonyCommandList>(responseText);
            }
            string first = dataSet.result[0].ToString();
            List<SonyOption> bal = JsonConvert.DeserializeObject<List<SonyOption>>(first);
            macaddress = bal.Find(x => x.option.ToLower() == "WOL".ToLower()).value.ToString();

            Log.Debug("Auto3D: Device Mac Address: " + macaddress);
            
            return macaddress;
        }

        /// <summary>
        /// This method will retrieve Gen1 and Gen2 XML IRCC Command List or Gen3 JSON Command List.
        /// </summary>
        /// <returns>Returns a string containing the contents of the returned XML Command List for your Use</returns>
        /// <remarks>This method will also populate the SonyDevice.Commands object list with the retrieved command list</remarks>
        public string get_remote_command_list()
        {
            string cmdList = "";
            
            if (Generation <= 2)
            {
                Log.Debug(Name + " is Retrieving Generation:" + Generation + " Remote Command List");
                
                cmdList = HttpGet(getActionlist("getRemoteCommandList", actionListURL));
                
                if (cmdList != "")
                {
                    Log.Debug("Retrieve Command List was Successful");
                    
                    DataSet CommandList = new DataSet();
                    System.IO.StringReader xmlSR = new System.IO.StringReader(cmdList);
                    CommandList.ReadXml(xmlSR, XmlReadMode.Auto);
                    DataTable IRCCcmd = new DataTable();
                    var items = CommandList.Tables[0].AsEnumerable().Select(r => new SonyCommands { name = r.Field<string>("name"), value = r.Field<string>("value") });
                    var itemlist = items.ToList();
                    Commands = itemlist;

                    Log.Debug(Name + " Commands have been Populated");                    
                }
                else
                {
                    Log.Error("Retrieve Command List was NOT successful");                                        
                }
            }
            else
            {
                Log.Debug(Name + " is Retrieving Generation:" + Generation + " Remote Command List");                
                
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + DeviceIPAddress + @"/sony/system");
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"id\":20,\"method\":\"getRemoteControllerInfo\",\"version\":\"1.0\",\"params\":[]}";
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    cmdList = responseText;
                    if (cmdList != "")
                    {
                        Log.Debug("Response Returned: " + cmdList);
                        Log.Debug("Retrieve Command List was successful");
                    }
                    else
                    {
                        Log.Error("Retrieve Command List was NOT successful");      
                    }
                    dataSet = JsonConvert.DeserializeObject<SonyCommandList>(responseText);
                }
                string first = dataSet.result[1].ToString();
                List<SonyCommands> bal = JsonConvert.DeserializeObject<List<SonyCommands>>(first);
                Commands = bal;

                Log.Debug(Name + " Commands have been Populated: " + Commands.Count().ToString());                
            }
            return cmdList;
        }

        /// <summary>
        /// This method retrieves the DLNA Device Action List XML.
        /// </summary>
        /// <param name="lAction">This is a string containing the Action name to retrieve</param>
        /// <param name="actionList_URL">URL For this devices Action List</param>
        /// <returns></returns>
        /// 
        private string getActionlist(string lAction, string actionList_URL)
        {
            Func<DataRow, bool> predicate = null;
            string str = "";
            if (!(actionList_URL != ""))
            {
                return str;
            }
            DataSet set = new DataSet();
            string fileName = actionList_URL.ToString();
            set.ReadXml(fileName);
            DataTable table = new DataTable();
            table = set.Tables[0];
            if (predicate == null)
            {
                predicate = myRow => myRow.Field<string>("name") == lAction;
            }
            return table.Rows.Cast<DataRow>().Where<DataRow>(predicate).ElementAt<DataRow>(0).Field<string>("url");
        }

        /// <summary>
        /// This method sets the Action List URL found in the DLNA Device Action List XML.
        /// </summary>
        /// <param name="devName">This is the Name of the Device to get the infromation for</param>
        /// 
        private void getActionlist_URL(UPnPService service)
        {
            Log.Debug("Auto3D: Retrieving Action List URL from device description file: " + service.ParentDevice.FriendlyName);

            try
            {
                XElement xDeviceInfo = service.ParentDevice.DeviceXML.Elements().First(e => e.Name.LocalName == "X_UNR_DeviceInfo");

                if (xDeviceInfo != null)
                {
                    XElement xActionList = xDeviceInfo.Elements().First(e => e.Name.LocalName == "X_CERS_ActionList_URL");

                    if (xActionList != null)
                    {
                        actionListURL = xActionList.Value;
                        Log.Debug("Auto3D: Action List URL found for device: " + service.ParentDevice.FriendlyName);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Auto3D: " + ex.Message);            
            }

            Log.Error("Auto3D: Action List URL was NOT found for device: " + service.ParentDevice.FriendlyName);            
            actionListURL = "";
        }

        /// <summary>
        /// getControlMac retrieves the MAC address from the static method GetMacAddress().
        /// </summary>
        /// <returns>A string containing a processed MAC address. 
        /// For example: Actual Mac 01:02:03:04:05:06 returns 01-02-03-04-05-06</returns>
        private string getControlMac()
        {
            Log.Debug("Auto3D: Retrieving Controlling devices Mac Address. (this computer)");

            string mac = GetMacAddress();

            Log.Debug("Auto3D: Mac Address retrieved: " + mac);
            Log.Debug("Auto3D: Re-Parsing Mac Address. (Replace : with -)");            
            
            string new_mac = "";
            int i = mac.Length;
            int y = 1;
            int z;
            for (z = 1; z <= i; z++)
            {
                new_mac += mac.Substring(z - 1, 1);
                if (y == 2)
                {
                    if (z < i)
                    {
                        new_mac += "-";
                        y = 0;
                    }
                }
                y = y + 1;
            }
            
            mac = new_mac;
            Log.Debug("Auto3D: Mac Address has been re-Parsed: " + mac);            
            return mac;
        }

        public string getIRCCcommandString(string cmdname)
        {
            Log.Debug("Auto3D: Retrieving Command String for Command: " + cmdname);
            
            string str = "";
            SonyCommands commands = Commands.FirstOrDefault<SonyCommands>(o => o.name == cmdname);
            if (commands != null)
            {
                try
                {
                    str = commands.value;
                    Log.Debug("Auto3D: Found Command String for: " + cmdname + " - " + str);                    
                }
                catch (Exception ex)
                {
                    str = "";
                    Log.Error("Auto3D: Command String for: " + cmdname + " NOT Found!");
                    Log.Error("Auto3D: " + ex.Message);                    
                }
            }
            return str;
        }

        /// <summary>
        /// Static method used to obtain your NIC MAC address.
        /// </summary>
        /// <returns>A string containing the MAC address of the fastest NIC found in your system.</returns>
        /// <remarks>Should not be used publically. Use the getControlMac() method instead.</remarks>
        private static string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }
            
            return macAddress;
        }

        /// <summary>
        /// Gets the Registration Mode from the ActionList.
        /// Or uses Gen 3 if no action list is found.
        /// </summary>
        /// <returns>Returns a string wih the Mode (1, 2 or 3)</returns>
        private string getRegistrationMode()
        {
            string lAction = "register";
            string foundVal = "3";

            Log.Debug("Auto3D: Retrieving Device Registration Mode");
            
            if (actionListURL != "")
            {
                DataSet acList = new DataSet();
                acList.ReadXml(actionListURL);
                DataTable act = new DataTable();
                act = acList.Tables[0];
                var results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == lAction select myRow;
                foundVal = results.ElementAt(0).Field<string>("mode");
            }
            else
            {
                Log.Debug("Auto3D: No Action List found");
            }

            Log.Info("Auto3D: Using Registration mode: " + foundVal);            
            return foundVal;
        }


        private string HttpGet(string Url)
        {
            Log.Debug("Auto3D: Creating HttpWebRequest to URL: " + Url);
            
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.KeepAlive = true;
            
            // Set our default header Info
            Log.Debug("Auto3D: Setting Header Information: " + req.Host.ToString());
            
            req.Host = DeviceIPAddress + ":" + DevicePort;
            req.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";
            req.Headers.Add("X-CERS-DEVICE-INFO", "Android4.03/TVSideViewForAndroid2.7.1/EVO");
            req.Headers.Add("X-CERS-DEVICE-ID", "TVSideView:" + ServerMacAddress);
            req.Headers.Add("Accept-Encoding", "gzip");
            try
            {
                Log.Debug("Auto3D: Creating Web Request Response");
                
                System.Net.WebResponse resp = req.GetResponse();

                Log.Debug("Auto3D: Executing StreamReader");
                
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string results = sr.ReadToEnd().Trim();

                Log.Debug("Auto3D: Response returned: " + results);
                                
                sr.Close();
                resp.Close();
                return results;
            }
            catch (Exception ex)
            {
                Log.Debug("Auto3D: There was an error during the Web Request or Response! " + ex.ToString());                
                return "false : " + ex;
            }
        }

        private string HttpPost(string Url, string Parameters)
        {
            Log.Debug("Auto3D: Creating HttpWebRequest to URL: " + Url);
            
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);

            Log.Debug("Auto3D: Sending the following parameter: " + Parameters.ToString());
            
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Parameters);
            req.KeepAlive = true;
            req.Method = "POST";
            req.ContentType = "text/xml; charset=utf-8";
            req.ContentLength = bytes.Length;

            Log.Debug("Auto3D: Setting Header Information: " + req.Host.ToString());
            
            if (DevicePort != 80)
            {
                req.Host = DeviceIPAddress + ":" + DevicePort;
            }
            else
            {
                req.Host = DeviceIPAddress;
            }

            Log.Debug("Auto3D: Header Host: " + req.Host.ToString());

            req.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";
            Log.Debug("Auto3D: Setting Header User Agent: " + req.UserAgent);

            if (Generation == 3)
            {
                Log.Debug("Auto3D: Processing Auth Cookie");
                
                req.CookieContainer = new CookieContainer();
                List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(Cookie);
                req.CookieContainer.Add(new Uri(@"http://" + DeviceIPAddress + bal[0].Path.ToString()), new Cookie(bal[0].Name, bal[0].Value));

                Log.Debug("Auto3D: Cookie Container Count: " + req.CookieContainer.Count.ToString());
                Log.Debug("Auto3D: Setting Header Cookie: auth=" + bal[0].Value);                
            }
            else
            {
                Log.Debug("Auto3D: Setting Header X-CERS-DEVICE-ID: TVSideView-" + ServerMacAddress);                
                req.Headers.Add("X-CERS-DEVICE-ID", "TVSideView:" + ServerMacAddress);
            }
            req.Headers.Add("SOAPAction", "\"urn:schemas-sony-com:service:IRCC:1#X_SendIRCC\"");
            if (Generation != 3)
            {
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
            }
            else
            {
                req.Headers.Add("Accept-Encoding", "gzip");
            }

            Log.Debug("Auto3D: Sending WebRequest");                
            
            System.IO.Stream os = req.GetRequestStream();
            // Post data and close connection
            os.Write(bytes, 0, bytes.Length);

            Log.Debug("Auto3D: Sending WebRequest Complete");                            
            
            // build response object if any                        
            Log.Debug("Auto3D: Creating Web Request Response");                            
            
            System.Net.HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            Stream respData = resp.GetResponseStream();
            StreamReader sr = new StreamReader(respData);
            string response = sr.ReadToEnd();

            Log.Debug("Auto3D: Response returned: " + response);                            
            
            os.Close();
            sr.Close();
            respData.Close();
            return response;
        }

        /// <summary>
        /// Initializes a NEW Sony Device object with settings from a device retrieved from sonyDiscover(device).
        /// </summary>
        /// <param name="device">A Sony Device object selected from the list obtained with sonyDiscover(device) method.</param>
        public void initialize(UPnPService service)
        {
            Name = service.ParentDevice.FriendlyName;
            DeviceIPAddress = service.ParentDevice.WebAddress.Host;
            
            if (actionListURL == null)
            {
                getActionlist_URL(service);
            }
            
            Generation = Convert.ToInt32(getRegistrationMode());
                                
            if (DevicePort == 0)
            {
                if (Generation <= 2)
                {
                    string devIP = actionListURL;
                    devIP = devIP.Replace("http://", "");
                    string s1 = ":";
                    int endIP = devIP.IndexOf(s1);
                    string port = devIP.Substring(endIP + 1);
                    devIP = devIP.Substring(0, endIP);
                    s1 = "/";
                    int endPort = port.IndexOf(s1);
                    port = port.Substring(0, endPort);
                    DevicePort = Convert.ToInt32(port);
                }
                else
                {
                    DevicePort = 80;
                }
            }

            Log.Debug("Auto3D: Initializing Device: " + Name + " @ " + DeviceIPAddress + ":" + DevicePort.ToString());
          
            if (ServerName == null)
            {
                ServerName = SystemInformation.ComputerName + "(SonyAPILib)";
            }

            ircc = service;
            
            ServerMacAddress = getControlMac();
            
            if (Generation == 3)
            {
                DeviceMacAddress = findDevMac().ToString();
            }
            
            checkReg();            
            Log.Debug("Auto3D: Device Initialized: " + Name);
        }

        /// <summary>
        /// Sends the Registration command to the selected device
        /// </summary>
        /// <returns>Returns a bool True or False</returns>
        public bool register()
        {
            Log.Debug("Auto3D: Controlling Mac address: " + ServerMacAddress);
            
            string reg = "false";
            string args1 = "name=" + ServerName + "&registrationType=initial&deviceId=TVSideView%3A" + ServerMacAddress + " ";
            string args2 = "name=" + ServerName + "&registrationType=new&deviceId=TVSideView%3A" + ServerMacAddress + " ";

            if (Generation == 1)
            {
                reg = HttpGet(getActionlist("register", actionListURL) + "?" + args1);
                Log.Debug("Auto3D: Registering Generation 1 Sony Device");
            }
            else if (Generation == 2)
            {
                reg = HttpGet(getActionlist("register", actionListURL) + "?" + args2);
                Log.Debug("Auto3D: Registering Generation 2 Sony Device");
            }
            else if (Generation == 3)
            {
                Log.Debug("Auto3D: Registering Generation 3 Sony Device");

                string hostname = ServerName;
                string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + "\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + "\",\"function\":\"WOL\"}]]}";
                try
                {                    
                    Log.Debug("Auto3D: Creating JSON Web Request");
                    
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://" + DeviceIPAddress + "/sony/accessControl");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.AllowAutoRedirect = true;
                    httpWebRequest.Timeout = 500;

                    Log.Debug("Auto3D: Sending Generation 3 JSON Registration");
                    
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(jsontosend);
                    }
                    try
                    {
                        httpWebRequest.GetResponse();
                    }
                    catch { }

                    Log.Debug("Auto3D: Must run Method: SendAuth(pincode)");                    
                    reg = "Gen3 Pin Code Required";
                }
                catch
                {
                    Log.Error("Auto3D: Device not reachable");                         
                }
            }

            if (reg == "")
            {
                Log.Debug("Auto3D: Registration was Successful for device at: " + DeviceIPAddress);                
                Registered = true;
                return true;
            }
            else if (reg == "Gen3 Pin Code Required")
            {
                Log.Debug("Auto3D: Registration not complete for Gen3 device at: " + DeviceIPAddress);                
                Registered = false;
                return true;
            }
            else
            {
                Log.Debug("Auto3D: Registration was NOT successful for device at: " + DeviceIPAddress);                
                Registered = false;
                return false;
            }
        }

        public string send_ircc(string irccCode)
        {
            Log.Debug("Auto3D: Recieved IRCC Command: " + irccCode);

            string str = "";
            StringBuilder builder = new StringBuilder("<?xml version=\"1.0\"?>");
            builder.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">");
            builder.Append("<s:Body>");
            builder.Append("<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">");
            builder.Append("<IRCCCode>" + irccCode + "</IRCCCode>");
            builder.Append("</u:X_SendIRCC>");
            builder.Append("</s:Body>");
            builder.Append("</s:Envelope>");

            Log.Debug("Auto3D: Sending IRCC Command: " + irccCode);
            
            str = HttpPost(ircc.ControlUrl, builder.ToString());
            if (str != "")
            {
                Log.Debug("Auto3D: Command was sent successfully");                
                return str;
            }

            Log.Error("Auto3D: Command was NOT sent successfully");                            
            return str;
        }

        /// <summary>
        /// Sends the Authorization PIN code to the Gen3 Device
        /// </summary>
        /// <param name="pincode"></param>
        /// <returns>True or False</returns>
        public bool sendAuth(string pincode)
        {
            bool reg = false;
            string hostname = ServerName;
            string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + "\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + "\",\"function\":\"WOL\"}]]}";
            try
            {
                var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(@"http://" + DeviceIPAddress + @"/sony/accessControl");
                httpWebRequest2.ContentType = "application/json";
                httpWebRequest2.Method = "POST";
                httpWebRequest2.AllowAutoRedirect = true;
                httpWebRequest2.CookieContainer = allcookies;
                httpWebRequest2.Timeout = 500;
                using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                {
                    streamWriter.Write(jsontosend);
                }
                string authInfo = "" + ":" + pincode;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;
                var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();

                    Log.Debug("Auto3D: Registration response: " + responseText);                                        
                    reg = true;
                }
                string answerCookie = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + DeviceIPAddress + "/sony/appControl")));
                System.IO.StreamWriter file = new System.IO.StreamWriter("cookie.json");
                file.WriteLine(answerCookie);
                file.Close();
                Cookie = answerCookie;
            }
            catch
            {
                Log.Error("Auto3D: Registration process timed out");                                                        
                Registered = false;
            }
            return reg;
        }

        /// <summary>
        /// Gets or Sets the Sony Device Object's List of Commands
        /// </summary>
        public List<SonyCommands> Commands { get; set; }

        /// <summary>
        /// Gets or Sets the Sony Device Gen 3 Object's Authentication Cookie
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        /// Gets or Sets the Sony Device Object's IP Address
        /// </summary>
        public string DeviceIPAddress { get; set; }

        /// <summary>
        /// Gets or Sets the Sony Device Object's MAC Address
        /// </summary>
        public string DeviceMacAddress { get; set; }

        /// <summary>
        /// Gets or Sets the Sony Device Object's Port number
        /// </summary>
        public int DevicePort { get; set; }

        /// <summary>
        /// Gets or Sets the Sony Device Object Generation 1, 2 or 3
        /// </summary>
        public int Generation { get; set; }

        public UPnPService ircc { get; set; }

        /// <summary>
        /// Gets or Sets the Sony Device Object Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the status of registration
        /// </summary>
        public bool Registered { get; set; }

        /// <summary>
        /// Gets or Sets the Mac Address of he controlling device
        /// </summary>
        public string ServerMacAddress { get; set; }

        /// <summary>
        /// Gets or Sets the Name of he controlling device
        /// </summary>
        public string ServerName { get; set; }
    }
}
