using MediaPortal.GUI.Library;
using MediaPortal.Profile;
using Hid = SharpLib.Hid;
using SharpLib.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.ProcessPlugins.Auto3D
{
	public class HIDInput : Form, IMessageFilter
	{
		private static HIDInput _instance;

		private const int WM_INPUT = 0x00ff;

		[StructLayout(LayoutKind.Explicit)]
		public struct RAWINPUT                                                                 
		{
			[FieldOffset(0)]
			public RAWINPUTHEADER Header;
			[FieldOffset(16)]
			public RAWHID HID;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RAWINPUTHEADER                                                              
		{
			public RawInputType Type;
			public int Size;
			public IntPtr Device;
			public IntPtr wParam;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RAWHID                                                                  
		{
			public uint dwSizeHID;                                                            
			public uint dwCount;                                                              
			public byte keyCodeA;                                                                
			public byte keyCodeB;                                                                
			public byte dataA;                                                                
			public byte dataB;                                                                
		}

		public enum RawInputType                                                                  
		{
			Mouse = 0,
			Keyboard = 1,
			HID = 2
		}

		enum RawInputCommand                                                                      
		{
			Input = 0x10000003,
			Header = 0x10000005
		}

		private Hid.Handler _handler;

		public delegate bool OnHidKeyEventDelegate(object aSender, String key);
		public delegate void OnHidEventDelegate(object aSender, SharpLib.Hid.Event aHidEvent);

		public event OnHidKeyEventDelegate HidEvent;

		private HIDInput()
		{			
		}

		public static bool HandleOwnDevices
		{
			get;
			set;
		}

		public static HIDInput getInstance()
		{
			if (_instance == null)
			{
				_instance = new HIDInput();
				_instance.Bounds = new System.Drawing.Rectangle(-1, -1, 1, 1);
				_instance.Show();
				_instance.Visible = false;

				Application.AddMessageFilter(_instance);
			}

			return _instance;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (HandleOwnDevices)
				RegisterHidDevices();
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
		}

		public void RegisterHidDevices()
		{
			SharpLib.Win32.RAWINPUTDEVICE[] rid = new SharpLib.Win32.RAWINPUTDEVICE[1];

			rid[0].usUsagePage = (ushort)SharpLib.Hid.UsagePage.WindowsMediaCenterRemoteControl;
			rid[0].usUsage = (ushort)SharpLib.Hid.UsageCollection.WindowsMediaCenter.WindowsMediaCenterRemoteControl;
			rid[0].dwFlags = Const.RIDEV_EXINPUTSINK;
			rid[0].hwndTarget = Handle;			

			int repeatDelay = -1;
			int repeatSpeed = -1;
			
			using (Settings settings = new MPSettings())
			{
				repeatDelay = settings.GetValueAsInt("remote", "HidRepeatDelayInMs", repeatDelay);
				repeatSpeed = settings.GetValueAsInt("remote", "HidRepeatSpeedInMs", repeatSpeed);
			}
			
			_handler = new SharpLib.Hid.Handler(rid, true, repeatDelay, repeatSpeed);

			if (!_handler.IsRegistered)
			{
				Debug.WriteLine("Failed to register raw input devices: " + Marshal.GetLastWin32Error().ToString());
			}

			_handler.OnHidEvent += HandleHidEventThreadSafe;
		}

		private void HandleHidEventThreadSafe(object aSender, SharpLib.Hid.Event aHidEvent)
		{
			if (aHidEvent.IsStray)
			{
				// Stray event just ignore it
				return;
			}

			if (this.InvokeRequired)
			{
				// Not in the proper thread, invoke ourselves.
				// Repeat events usually come from another thread.
				OnHidEventDelegate d = new OnHidEventDelegate(HandleHidEventThreadSafe);
				this.Invoke(d, new object[] { aSender, aHidEvent });
			}
			else
			{
				if (HidEvent != null)
				{
					foreach (ushort usage in aHidEvent.Usages)
					{
						String key = "HID " + usage.ToString("X4");
						HidEvent(aSender, key);
					}
				}
			}
		}

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == WM_INPUT)
			{
				if (HandleOwnDevices)
				{
					_handler.ProcessInput(ref m);
				}
				else
				{
					return ProcessRawInput(m.LParam);
				}
			}

			return false;
		}

		[DllImport("user32.dll")]
		static extern int GetRawInputData(IntPtr hRawInput, RawInputCommand uiCommand, out RAWINPUT pData, ref int pcbSize, int cbSizeHeader);

		private bool ProcessRawInput(IntPtr hRawInput)      
		{
			RAWINPUT pData = new RAWINPUT();
			
			int pcbSize = Marshal.SizeOf(typeof(RAWINPUT));
			int result = GetRawInputData(hRawInput, RawInputCommand.Input, out pData, ref pcbSize, Marshal.SizeOf(typeof(RAWINPUTHEADER)));

			if (result != -1)
			{
				if (pData.Header.Type == RawInputType.HID)  
				{
					String key = "HID " + pData.HID.keyCodeB.ToString("X4");
					return HidEvent(this, key);					
				}
			}

			return false;                                   
		}
	}
}
