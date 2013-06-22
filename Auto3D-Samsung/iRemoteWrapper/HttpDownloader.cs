using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices.Samsung.iRemoteWrapper
{
    public class HttpDownloader
    {
        // Fields
        private CallbackDownloaded callback;
        private Control control;
        private Image image;
        private string m_url;
        private static string tvIp = "";

        // Methods
        public void Downloader()
        {
            try
            {
                this.image = Image.FromStream(WebRequest.Create(this.m_url).GetResponse().GetResponseStream());
                this.control.Invoke(this.callback, new object[] { this.image });
            }
            catch (Exception ex)
            {
                //Debug.Log("[HTTPDOWNLOADER EX]: " + exception.ToString());
            }
        }

        public void SetCbInfo(Control ctrl, CallbackDownloaded cb)
        {
            this.control = ctrl;
            this.callback = cb;
        }

        public static void SetTvIp(string ip)
        {
            tvIp = ip;
        }

        public void StartDownload(string url)
        {
            if (!url.ToLower().Contains("http://"))
            {
                url = "http://" + tvIp + url;
            }
            this.m_url = url;
            new Thread(new ThreadStart(this.Downloader)).Start();
        }
    }
}
