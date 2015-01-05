using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MediaPortal.ServiceImplementations;
using WebSocketSharp;
using Newtonsoft.Json.Linq;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public class WebOS
  {
      static WebSocket webClient;
      static WebSocket webClientMouse;

      enum eCommand { None, Pair, Register, GetInputSocket };

      static eCommand cmd = eCommand.None;

      static String pairingKey;

    static WebOS()
    {
    }

    public static void Close()
    {
        if (webClient != null)
        {
            webClient.Close();
            webClient = null;
        }

        if (webClientMouse != null)
        {
            webClientMouse.Close();
            webClientMouse = null;
        }
    }

    private static void SendCommand(String command)
    {
        JObject sendData = JObject.Parse(
        @"{            
                'type': 'request',
                'id': '1'
            }");

        sendData.Add("uri", command);

        String jsonString = sendData.ToString();

        webClient.Send(jsonString);
    }

    public static void SendSpecialKey(String keyName)
    {
        if (webClientMouse == null)
            return;

        StringBuilder sb = new StringBuilder();

        sb.Append("type:button\n");
        sb.Append("name:" + keyName + "\n");
        sb.Append("\n");

        webClientMouse.Send(sb.ToString());
    }

    public static void ThreadPoolCallback(Object threadContext)
    {
        cmd = eCommand.GetInputSocket;
        SendCommand("ssap://com.webos.service.networkinput/getPointerInputSocket");
    }

    public static void TurnOff()
    {
        SendCommand("ssap://system/turnOff");
    }

    public delegate void UpdatePairingKeyHandler(object sender, String key);
    public static event UpdatePairingKeyHandler UpdatePairingKey;

    static void webClient_OnMessage(object sender, MessageEventArgs e)
    {
        JObject receiveData = JObject.Parse(e.Data);
        JProperty jp = receiveData.Property("payload");
        JProperty jpError = receiveData.Property("error");

        if (jpError != null)
        {
            Log.Error("Auto3D: " + jpError.Value.ToString());
            cmd = eCommand.None;
            pairingKey = "";
            return;
        }
        else
        {
            Log.Info("Auto3D: " + e.Data);
        }

        switch (cmd)
        {
            case eCommand.None:
            case eCommand.Pair:

                JToken jt = jp.Value["client-key"];

                if (jt != null)
                {
                    pairingKey = jt.ToString();
                    cmd = eCommand.None;

                    if (UpdatePairingKey != null)
                        UpdatePairingKey(null, pairingKey);
                }
                break;

            case eCommand.Register:

                cmd = eCommand.None;
                ThreadPool.QueueUserWorkItem(ThreadPoolCallback);
                break;

            case eCommand.GetInputSocket:

                String socketPath = jp.Value["socketPath"].ToString();
                socketPath = socketPath.Replace("wss:", "ws:").Replace(":3001/", ":3000/");

                if (webClientMouse != null)
                {
                    webClientMouse.Close();                    
                    webClientMouse = null;
                }

                webClientMouse = new WebSocket(socketPath);
                webClientMouse.Connect();
                cmd = eCommand.None;
                break;
        }
    }

    static public void InternalConnect(String ipAddress)
    {
        try
        {
            webClient = new WebSocket("wss://" + ipAddress + ":3001");
            webClient.OnMessage += webClient_OnMessage;
            webClient.Connect();
        }
        catch (Exception ex)
        {
            Log.Error("Auto3D: " + ex.Message);
        }
    }

    static public String Pair(String ipAddress)
    {
        pairingKey = null;

        if (webClient != null)
        {
            webClient.Close();
            webClient.OnMessage -= webClient_OnMessage;
            webClient = null;
        }

        InternalConnect(ipAddress);

        cmd = eCommand.Pair;

        JObject sendData = JObject.Parse(
        @"{            
                'type': 'register',
                'id': '1',
                'payload': 
                { 
                    'manifest': 
                    {
                        'manifestVersion' : '1',   
                        'permissions' : 
                        [   'LAUNCH', 'LAUNCH_WEBAPP', 'APP_TO_APP', 'CONTROL_AUDIO', 'CONTROL_INPUT_MEDIA_PLAYBACK', 
                            'CONTROL_POWER', 'READ_INSTALLED_APPS', 'CONTROL_DISPLAY', 'CONTROL_INPUT_JOYSTICK', 'CONTROL_INPUT_MEDIA_RECORDING', 'CONTROL_INPUT_TV', 'READ_INPUT_DEVICE_LIST', 'READ_NETWORK_STATE', 'READ_TV_CHANNEL_LIST', 'WRITE_NOTIFICATION_TOAST',
                            'CONTROL_INPUT_TEXT', 'CONTROL_MOUSE_AND_KEYBOARD', 'READ_CURRENT_CHANNEL', 'READ_RUNNING_APPS' ]                   
                    }
                }                 
            }");

        String jsonString = sendData.ToString();
        webClient.Send(jsonString);

        while (pairingKey == null)
            Thread.Sleep(50);

        if (pairingKey == "?")
            pairingKey = "";

        return pairingKey;
    }

    static public void Register(String ipAddress, String pairingKey)
    {
        if (webClient == null)
        {
            InternalConnect(ipAddress);
        }

        JObject sendData = JObject.Parse(
        @"{            
                'type': 'register',
                'id': '1',
                'payload': 
                 { 
                    'client-key': '',
                    'manifest': 
                    {
                        'manifestVersion' : '1',   
                        'permissions' : 
                        [   'LAUNCH', 'LAUNCH_WEBAPP', 'APP_TO_APP', 'CONTROL_AUDIO', 'CONTROL_INPUT_MEDIA_PLAYBACK', 
                            'CONTROL_POWER', 'READ_INSTALLED_APPS', 'CONTROL_DISPLAY', 'CONTROL_INPUT_JOYSTICK', 'CONTROL_INPUT_MEDIA_RECORDING', 'CONTROL_INPUT_TV', 'READ_INPUT_DEVICE_LIST', 'READ_NETWORK_STATE', 'READ_TV_CHANNEL_LIST', 'WRITE_NOTIFICATION_TOAST',
                            'CONTROL_INPUT_TEXT', 'CONTROL_MOUSE_AND_KEYBOARD', 'READ_CURRENT_CHANNEL', 'READ_RUNNING_APPS' ]                   
                    }
                 }                 
            }");

        JProperty jp = sendData.Property("payload");
        jp.Value["client-key"].Replace(pairingKey);

        String jsonString = sendData.ToString();
        webClient.Send(jsonString);
        cmd = eCommand.Register;
    }
  }
}
