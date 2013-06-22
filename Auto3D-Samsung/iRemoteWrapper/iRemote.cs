using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using MediaPortal.ProcessPlugins.Auto3D.Devices.Samsung.iRemoteWrapper;
using System.Windows.Forms;

namespace MediaPortal.ProcessPlugins.Auto3D.Samsung.iRemoteWrapper
{
    public class iRemote
    {
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void AuthenticationSuccess();
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void Close();
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void Connect(ref TVInfo info, int port, StringBuilder DeviceName);
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void HttpClientAction();
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void HttpClientClose();
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void HttpClientConnect();
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void HttpClientCreate();
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void HttpClientDestroy();
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void HttpClientInit();
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void HttpClientStartRecv();
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void InstallUPnPCallback(UPnPCallbackFunc addedCallback, UPnPCallbackFunc deletedCallback);
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern short PacketGetShort(ref IntPtr packet);
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void SendRemocon(REMOCONCODE code, REMOCON_TYPE type);
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void SendString(byte[] str, int stringLength);
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void SetConnectCallback(IRemoteCallback connectSuccess, IRemoteCallback connectFail, IRemoteCallback disconnect, IRemoteCallback authentication, IRemoteCallback authenticationFail, IRemoteCallback authenticationDeny, IRemoteCallback authenticationFull);
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void SetPacketParser(IPacketParser cb);
        [DllImport("iRemote.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern int StartUPnP(StringBuilder DeviceName);

        #region members

        public delegate void AddTVEventHandler(TVInfo info);
        public delegate void RemoveTVEventHandler(TVInfo info);

        public event AddTVEventHandler addTVEvent;
        public event RemoveTVEventHandler removeTVEvent;

        public UPnPCallbackFunc addTv;
        public UPnPCallbackFunc delTv;

        private List<TVInfo> tvs = new List<TVInfo>();

        public List<TVInfo> TVS
        {
            get { return tvs; }
        }

        public IRemoteCallback connectFail;
        public IRemoteCallback connectOk;
        public IRemoteCallback disc;
        public IRemoteCallback authentic;
        public IRemoteCallback authenticDeny;
        public IRemoteCallback authenticFail;
        public IRemoteCallback authenticFull;
        public IPacketParser packetCb;

        private TVInfo curTv;

        public TVInfo CurrentTV
        {
            get { return curTv; }
        }

        #endregion

        public iRemote()
        {
            this.addTv = new UPnPCallbackFunc(TvAddedCallback);
            this.delTv = new UPnPCallbackFunc(TvDeletedCallback);

            this.packetCb = new IPacketParser(PacketParser);
            this.connectOk = new IRemoteCallback(connectSuccess);
            this.connectFail = new IRemoteCallback(connectionFail);
            this.authentic = new IRemoteCallback(authentication);
            this.disc = new IRemoteCallback(disconnect);
            this.authenticFail = new IRemoteCallback(authenticationFail);
            this.authenticFull = new IRemoteCallback(authenticationFull);
            this.authenticDeny = new IRemoteCallback(authenticationDeny);

            iRemote.InstallUPnPCallback(this.addTv, this.delTv);
            int num = iRemote.StartUPnP(new StringBuilder("Mobile Simulator"));
        }

        public void TvAddedCallback(ref TVInfo info)
        {
            foreach (TVInfo info2 in this.tvs)
            {
                if (info2.Mac == info.Mac)
                {
                    return; // is already in list
                }
            }

            TVInfo localCopy = info;
            this.tvs.Add(localCopy);

            MediaPortal.ProcessPlugins.Auto3D.Devices.Auto3DHelpers.GetMainForm().Invoke((System.Windows.Forms.MethodInvoker)delegate
            {
                if (addTVEvent != null)
                    addTVEvent(localCopy);
            });
        }

        public void TvDeletedCallback(ref TVInfo info)
        {
            foreach (TVInfo info2 in this.tvs)
            {
                if (info2.Mac == info.Mac)
                {
                    this.tvs.Remove(info2);
                    break;
                }
            }

            TVInfo localCopy = info;

            MediaPortal.ProcessPlugins.Auto3D.Devices.Auto3DHelpers.GetMainForm().Invoke((System.Windows.Forms.MethodInvoker)delegate
            {
                if (removeTVEvent != null)
                    removeTVEvent(localCopy);
            });
        }

        public void PacketParser(ref IntPtr packet)
        {
            switch (iRemote.PacketGetShort(ref packet))
            {
                case 100:

                    if (iRemote.PacketGetShort(ref packet) == 1)
                    {
                        iRemote.AuthenticationSuccess();
                    }
                    break;

                case 200:
                    {
                        short num3 = iRemote.PacketGetShort(ref packet);
                        short num4 = iRemote.PacketGetShort(ref packet);
                        break;
                    }
            }
        }

        public void connectionFail()
        {
            MessageBox.Show("connectFail");
        }

        public void connectSuccess()
        {            
        }

        public void authentication()
        {
        }

        public void authenticationDeny()
        {
            MessageBox.Show("authenticationDeny");
        }

        public void authenticationFail()
        {
            MessageBox.Show("authenticationFail");
        }

        public void authenticationFull()
        {
            MessageBox.Show("authenticationFull");
        }

        public void disconnect()
        {
            iRemote.Close();            
        }

        public TVInfo GetCurrentTV()
        {
            return this.curTv;
        }

        public void ConnectTo(TVInfo info)
        {
            iRemote.SetConnectCallback(this.connectOk, this.connectFail, this.disc, this.authentic, this.authenticFail, this.authenticDeny, this.authenticFull);
            iRemote.SetPacketParser(this.packetCb);

            this.curTv = info;

            HttpDownloader.SetTvIp(info.Ip);

            iRemote.Connect(ref this.curTv, 0xd6d8, new StringBuilder("Mobile Simulator"));
        }
    }
}
