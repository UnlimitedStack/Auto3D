
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
  public class DiVine
  {
    static bool _connected = false;

    public enum amLib_EnumActivityId
    {
      amLib_ACT_HomeScreen,
      amLib_ACT_NetTV,
      amLib_ACT_BrowseUSB,
      amLib_ACT_BrowseDLNA,
      amLib_ACT_WatchTV,
      amLib_ACT_WatchSatellite,
      amLib_ACT_WatchExt,
      amLib_ACT_MultiApp,
      amLib_ACT_VideoStore,
      amLib_ACT_Dfu,
      amLib_ACT_Teletext,
      amLib_ACT_Epg,
      amLib_ACT_Esticker,
      amLib_ACT_Mhp,
      amLib_ACT_Upgrade,
      amLib_ACT_LoadingAnimation,
      amLib_ACT_Widgets,
      amLib_ACT_RemoteApp
    };

    public enum amLib_EnumActivation
    {
      amLib_ACT_Toggle,
      amLib_ACT_Start,
      amLib_ACT_Stop,
      amLib_ACT_StartNoFocus
    } ;

    public enum RCCodeBase
    {
      keySourceLkb = 1,
      keySourceRc5 = 2,
      keySourceRc6 = 3
    }

    // RC6 codes

    public enum RC6Codes
    {
      rc6S0Digit0 = 0,
      rc6S0Digit1 = 1,
      rc6S0Digit2 = 2,
      rc6S0Digit3 = 3,
      rc6S0Digit4 = 4,
      rc6S0Digit5 = 5,
      rc6S0Digit6 = 6,
      rc6S0Digit7 = 7,
      rc6S0Digit8 = 8,
      rc6S0Digit9 = 9,
      rc6S0PreviousProgram = 10,
      rc6S0 = 11,
      rc6S0Standby = 12,
      rc6S0MuteDemute = 13,
      rc6S0PersonalPreference = 14,
      rc6S0Display = 15,
      rc6S0VolumeUp = 16,
      rc6S0VolumeDown = 17,
      rc6S0BrightnessUp = 18,
      rc6S0BrightnessDown = 19,
      rc6S0SaturationUpOr2D = 20,
      rc6S0SaturationDownOr3D = 21,
      rc6S0BassUp = 22,
      rc6S0BassDown = 23,
      rc6S0TrebleUp = 24,
      rc6S0TrebleDown = 25,
      rc6S0BalanceRight = 26,
      rc6S0BalanceLeft = 27,
      rc6S0CtrlLastPosition = 28,
      rc6S0SearchUp = 30,
      rc6S0SearchDown = 31,
      rc6S0Next = 32,
      rc6S0Previous = 33,
      rc6S0FastForward = 40,
      rc6S0ScanReverse = 43,
      rc6S0Play = 44,
      rc6S0Stop = 49,
      rc6S0Record = 55,
      rc6S0External1 = 56,
      rc6S0External2 = 57,
      rc6S0AbProgram = 59,
      rc6S0TxtSubmode = 60,
      rc6S0SystemStandby = 61,
      rc6S0SystemSelect = 63,
      rc6S0ContextualOptions = 64,
      rc6S0StoreOpenClose = 69,
      rc6S0ClosedCaptioning = 70,
      rc6S0Sleeptimer = 71,
      rc6S0PictureNumberTime = 74,
      rc6S0TvTextSubtitle = 75,
      rc6S0PlayNextFile = 76,
      rc6S0PlayPreviousFile = 77,
      rc6S0SoundSelect = 78,
      rc6S0SpatialStereo = 79,
      rc6S0StereoMono = 80,
      rc6S0SoundScroll = 81,
      rc6S0SurroundSound = 82,
      rc6S0SurroundSoundScroll = 83,
      rc6S0MenuOn = 84,
      rc6S0MenuOff = 85,
      rc6S0StepUp = 88,
      rc6S0StepDown = 89,
      rc6S0StepLeft = 90,
      rc6S0StepRight = 91,
      rc6S0Acknowledge = 92,
      rc6S0PipOnOff = 93,
      rc6S0PipSelect = 94,
      rc6S0PipShift = 95,
      rc6S0PipSize = 96,
      rc6S0PipStepDown = 97,
      rc6S0PipStepUp = 98,
      rc6S0PipMainSwap = 99,
      rc6S0PipFreeze = 100,
      rc6S0PipStrobe = 101,
      rc6S0MosaicMultiPip = 102,
      rc6S0MainFreezed = 103,
      rc6S0MainStored = 104,
      rc6S0Red = 109,
      rc6S0Green = 110,
      rc6S0Yellow = 111,
      rc6S0Cyan = 112,
      rc6S0IndexWhite = 113,
      rc6S0TimerSet = 117,
      rc6S0Preset10 = 120,
      rc6S0Preset11 = 121,
      rc6S0Preset12 = 122,
      rc6S0Preset13 = 123,
      rc6S0Preset14 = 124,
      rc6S0Preset15 = 125,
      rc6S0Preset16 = 126,
      rc6S0Time = 127,
      rc6S0Help = 129,
      rc6S0DefaultMenuSelect = 130,
      rc6S0FavouriteMode = 132,
      rc6S0SwivelMode = 133,
      rc6S0External3 = 134,
      rc6S0External4 = 135,
      rc6S0External5 = 136,
      rc6S0VideoVgaSelect = 138,
      rc6S0NextSource = 139,
      rc6S0BacklightUp = 140,
      rc6S0BacklightDown = 141,
      rc6S0HdAtHome = 142,
      rc6S0AmbLightOnOffDim = 143,
      rc6S0AmbLightMode = 144,
      rc6S0AmbilightBrightnessUp = 145,
      rc6S0AmbilightBrightnessDown = 146,
      rc6S0Smart = 150,
      rc6S0SoftKey1A = 151,
      rc6S0SoftKey1B = 152,
      rc6S0SoftKey2A = 153,
      rc6S0SoftKey2B = 154,
      rc6S0SoftKey3A = 155,
      rc6S0SoftKey3B = 156,
      rc6S0Smiley = 157,
      rc6S0Frownie = 158,
      rc6S0AvMute = 163,
      rc6S0MainSubmode = 176,
      rc6S0Resume = 177,
      rc6S0Display3D = 186,
      rc6S0DisplayBrowser = 190,
      rc6S0AnalogueDigital = 192,
      rc6S0DigitalSetupMenu = 193,
      rc6S0OneTouchHomeCinema = 194,
      rc6S0BatteryLow = 195,
      rc6S0CursorUpRight = 196,
      rc6S0CursorUpLeft = 197,
      rc6S0CursorDownRight = 198,
      rc6S0CursorDownLeft = 199,
      rc6S0Digit100 = 200,
      rc6S0RotCabClockwise = 201,
      rc6S0CenterCabinet = 202,
      rc6S0RotCabAntiClockwise = 203,
      rc6S0EpgGuide = 204,
      rc6S0ToggleStandby = 205,
      rc6S0PageUp = 206,
      rc6S0PageDown = 207,
      rc6S0DigitalPictureMenu = 208,
      rc6S0StartPage = 209,
      rc6S0FavouritesList = 210,
      rc6S0MarkListitem = 211,
      rc6S0SubmodeSystemMenu = 213,
      rc6S0ZoomOnOff = 214,
      rc6S0CableMode = 215,
      rc6S0WideScreen = 216,
      rc6S0Dot = 217,
      rc6S0SpeechMusic = 218,
      rc6S0Options = 219,
      rc6S0RfSwitch = 220,
      rc6S0ActuatorOnOff = 221,
      rc6S0Learn1 = 222,
      rc6S0Learn2 = 223,
      rc6S0CancelPicture = 224,
      rc6S0Enter = 225,
      rc6S0Exchange = 226,
      rc6S0TxtTv = 227,
      rc6S0NewsFlash = 228,
      rc6S0RowZero = 229,
      rc6S0SequenceOut = 230,
      rc6S0LargeTopBottomNormal = 231,
      rc6S0StepPageDown = 232,
      rc6S0StepPageUp = 233,
      rc6S0RevealConceal = 234,
      rc6S0PageHold = 235,
      rc6S0Index = 236,
      rc6S0RadioChannelDown = 237,
      rc6S0RadioChannelUp = 238,
      rc6S0PayTvChannelDown = 239,
      rc6S0PayTvChannelUp = 240,
      rc6S0TiltForward = 241,
      rc6S0TiltBackward = 242,
      rc6S0VideoPp = 243,
      rc6S0AudioPp = 244,
      rc6S0MovieExpand = 245,
      rc6S0ZoomMinus = 246,
      rc6S0ZoomPlus = 247,
      rc6S0ShowClock = 248,
      rc6S0PictureDigitalNoiseReduction = 249,
      rc6S0Crispener = 250,
      rc6S0ContrastDown = 251,
      rc6S0ContrastUp = 252,
      rc6S0TintHueDown = 253,
      rc6S0TintHueUp = 254,
      rc6S0Wysiwyr = 255
    }

    [DllImport("libdirectfb.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    static extern unsafe int DirectFBInit(int* argc, String[] argv);

    [DllImport("libdirectfb.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    static extern int DirectFBSetOption(String name, String value);

    [DllImport("libjs.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    static extern unsafe int jslibrc_Init(int* argc, String[] argv);

    [DllImport("libjs.dll", SetLastError = true)]
    static extern void jslibrc_Exit();

    [DllImport("libjs.dll", SetLastError = true)]
    static extern void jslibrc_KeyDown(RCCodeBase src, int sys, int cmd);

    [DllImport("libjs.dll", SetLastError = true)]
    static extern void jslibrc_KeyUp(RCCodeBase src, int sys, int cmd);

    [DllImport("libjs.dll", SetLastError = true)]
    static extern void jslibrc_KeyDownEx(RCCodeBase src, int sys, int cmd);

    [DllImport("libjs.dll", SetLastError = true)]
    static extern void jslibrc_KeyUpEx(RCCodeBase src, int sys, int cmd);

    [DllImport("libjs.dll", SetLastError = true)]
    static extern void jslibrc_RequestActivity(amLib_EnumActivityId act, amLib_EnumActivation mode, int cookie);

    public static bool IsConnected
    {
      get { return _connected; }
    }

    private static bool Ping(String ipAddress)
    {
      Ping pingSender = new Ping();
      PingOptions options = new PingOptions();

      options.DontFragment = true;
      string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
      byte[] buffer = Encoding.ASCII.GetBytes(data);
      int timeout = 120;

      PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);

      return reply.Status == IPStatus.Success;
    }

    public static bool Init(String ipAddress)
    {
      if (Ping(ipAddress))
      {
        unsafe
        {
          String[] argv = null;
          int count = 0;
          DirectFBInit(&count, argv);
          DirectFBSetOption("remote", ipAddress);

          int ret = jslibrc_Init(&count, argv);
          _connected = (ret == 0);
          return _connected;
        }
      }
      else
        return false;
    }

    public static void SendKey(RC6Codes code)
    {
      jslibrc_KeyDown(RCCodeBase.keySourceRc6, 0, (int)code);
      jslibrc_KeyUp(RCCodeBase.keySourceRc6, 0, (int)code);
    }

    public static void SendKeyEx(RC6Codes code)
    {
      jslibrc_KeyDownEx(RCCodeBase.keySourceRc6, 0, (int)code);
      jslibrc_KeyUpEx(RCCodeBase.keySourceRc6, 0, (int)code);
    }

    public static void KeyDown(RC6Codes code)
    {
      jslibrc_KeyDown(RCCodeBase.keySourceRc6, 0, (int)code);
    }

    public static void KeyUp(RC6Codes code)
    {
      jslibrc_KeyUp(RCCodeBase.keySourceRc6, 0, (int)code);
    }

    public static void RequestActivity(amLib_EnumActivityId act, amLib_EnumActivation mode)
    {
      jslibrc_RequestActivity(act, mode, 0);
    }

    public static void Exit()
    {
      jslibrc_Exit();
      _connected = false;
    }
  }
}
