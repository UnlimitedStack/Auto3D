using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices.Samsung.iRemoteWrapper
{
    public delegate void CallbackDownloaded(Image img);
    public delegate void IPacketParser(ref IntPtr packet);
    public delegate void SearchMenuPressedDelegate(TMainMenuItem item);
    public delegate void UPnPCallbackFunc(ref TVInfo info);
    public delegate void IRemoteCallback();

    public enum REMOCON_TYPE
    {
        REMOCON_TYPE_AUTO = 2,
        REMOCON_TYPE_NORMAL = 0,
        REMOCON_TYPE_ONLYRELEASE = 3,
        REMOCON_TYPE_ONLYRELEASEWITHSLEEP = 4
    }

    public enum REMOCONCODE
    {
        REMOCON_INVALID,
        REMOCON_1,
        REMOCON_2,
        REMOCON_3,
        REMOCON_4,
        REMOCON_5,
        REMOCON_6,
        REMOCON_7,
        REMOCON_8,
        REMOCON_9,
        REMOCON_0,
        REMOCON_DASH,
        REMOCON_CHUP,
        REMOCON_CHDOWN,
        REMOCON_VOLUP,
        REMOCON_VOLDOWN,
        REMOCON_PRECH,
        REMOCON_ENTER,
        REMOCON_RETURN,
        REMOCON_MUTE,
        REMOCON_TOOLS,
        REMOCON_MENU,
        REMOCON_UP,
        REMOCON_DOWN,
        REMOCON_LEFT,
        REMOCON_RIGHT,
        REMOCON_APP,
        REMOCON_EXIT,
        REMOCON_RED,
        REMOCON_GREEN,
        REMOCON_BLUE,
        REMOCON_YELLOW,
        REMOCON_SOURCE,
        REMOCON_CHLIST,
        REMOCON_FAVCH,
        REMOCON_REW,
        REMOCON_FF,
        REMOCON_PLAY,
        REMOCON_PAUSE,
        REMOCON_REC,
        REMOCON_POWER,
        REMOCON_STOP,
        REMOCON_INFO,
        REMOCON_CONTENTS,
        REMOCON_GUIDE,
        REMOCON_TV,
        REMOCON_SOUND_MODE,
        REMOCON_PICTURE_MODE,
        REMOCON_PSIZE,
        REMOCON_MTS,
        REMOCON_CAPTION,
        REMOCON_YAHOO,
        REMOCON_MEDIA,
        REMOCON_SRS,
        REMOCON_3D,
        REMOCON_D,
        REMOCON_DUAL,
        REMOCON_AD,
        REMOCON_HDMI,
        REMOCON_SLEEP,
        REMOCON_ESAVING,
        REMOCON_SUBTITLE,
        REMOCON_ANTENA,
        REMOCON_HOTAPPS,
        REMOCON_MHP,
        REMOCON_TTX,
        REMOCON_TTX_MIX,
        REMOCON_TTX_INDEX,
        REMOCON_TTX_PAGEDOWN,
        REMOCON_TTX_SUBPAGE,
        REMOCON_TTX_PAGEUP,
        REMOCON_TTX_HOLD,
        REMOCON_TTX_DOUBLESIZE,
        REMOCON_TTX_STORE,
        REMOCON_TTX_REVEAL,
        REMOCON_TTX_FLOFLIST,
        REMOCON_TTX_CANCEL
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TMainMenuItem
    {
        public string id;
        public string type;
        public string title;
        public string name;
        public string iconUrl;
        public string link;
        public string onHistory;
        public string onEvent;
        public List<string> tbTitles;
        public List<string> tbUrls;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TVInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)]
        public string Name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)]
        public string Ip;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)]
        public string Mac;
        public int port;
        public int type;
        public bool ttx;
        public bool mhp;
        
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Name, this.Ip);
        }

        public String UniqueDeviceName
        {
            get { return string.Format("{0} ({1})", this.Name, this.Mac);  }
        }
    }
}
