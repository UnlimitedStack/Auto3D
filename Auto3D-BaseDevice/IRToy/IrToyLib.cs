using System;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace IrToyLibrary 
{
	public delegate void IrReceivedHandler(object sender, String code);

    public class IrToy 
	{
		public event IrReceivedHandler Received;

        private SerialPort serialPort;

        private readonly int IRTOY_BUFFER_SIZE = 62;

        private readonly byte[] CMD_RESET				= new byte[] { 0, 0, 0, 0, 0 };                             // 5 x '0x00' 
        private readonly byte[] CMD_SAMPLEMODE			= new byte[] { Convert.ToByte(Convert.ToInt32('s')) };		// 's'
        private readonly byte[] CMD_VERSION				= new byte[] { Convert.ToByte(Convert.ToInt32('v')) };		// 'v'
        private readonly byte[] CMD_TRANSMIT			= new byte[] { Convert.ToByte(Convert.ToInt32("03", 16)) }; // 0x03
        private readonly byte[] CMD_BYTE_COUNT_REPORT	= new byte[] { Convert.ToByte(Convert.ToInt32("24", 16)) }; // 0x24           
        private readonly byte[] CMD_NOTIFY_ON_COMPLETE	= new byte[] { Convert.ToByte(Convert.ToInt32("25", 16)) };	// 0x25
        private readonly byte[] CMD_HANDSHAKE			= new byte[] { Convert.ToByte(Convert.ToInt32("26", 16)) }; // 0x26
        
		private readonly static String REQUIRED_VERSION		= "V224";
	    private readonly static String REQUIRED_SAMPLEMODE	= "S01";

		public IrToy()
		{
		}

		public void Connect(String comPort)
		{
			try
			{
				serialPort = new SerialPort();				
				serialPort.PortName		= comPort;
				serialPort.BaudRate		= 115200;
				serialPort.ReadTimeout	= 3000;

				serialPort.Open();				
			}
			catch (Exception ex)
			{
				if (serialPort.IsOpen)
					serialPort.Close();

				serialPort.Dispose();
				serialPort = null;

				throw ex;
			}

			CheckVersion();

			serialPort.DataReceived += serialPort_DataReceived;

			PrepareSend();
		}

		public void Close()
		{
			if (serialPort != null)
			{
				serialPort.DataReceived -= serialPort_DataReceived;

                try
                {
                    serialPort.Close();
                }
                finally
                {
                    serialPort.Dispose();
                    serialPort = null;
                }
			}
		}

		private void CheckVersion()
		{
			sendRawData(CMD_RESET);			
			sendRawData(CMD_VERSION);

			string version = readString(4);
			
			if (version != REQUIRED_VERSION)
			{
				throw new IrToyException("Version error: [" + version + "] does not match with the required version [" + REQUIRED_VERSION + "].");
			}
		}

		void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if (serialPort.BytesToRead == 0)
				return;

			byte[] response = readRawData(serialPort.BytesToRead);

			// check for special codes

			String responseString = Encoding.Default.GetString(response);

			if (responseString == REQUIRED_SAMPLEMODE)
			{
				return;
			}

			if (responseString.StartsWith("t"))
			{
				// transmit count
				return;
			}

			if (responseString.StartsWith("C"))
			{
				// notify on complete
				return;
			}

			if (response[0] == IRTOY_BUFFER_SIZE)
			{
				// handshake
				return;
			}

			// must be command

			string hex = BitConverter.ToString(response);

			hex = hex.Replace("-", " ").ToLower();		

			// check for 

			if (Received != null)
				Received(this, hex);
		}

        public void Send(string command) 
		{
            try 
			{
				PrepareSend();
				sendInternal(command);
			}                
			catch (Exception e) 
			{
                Close();
                throw e;
            }
        }

		public bool IsConnected()
		{
			return serialPort != null && serialPort.IsOpen;
		}

		private void PrepareSend()
		{
			sendRawData(CMD_RESET);
			sendRawData(CMD_SAMPLEMODE);			
			sendRawData(CMD_BYTE_COUNT_REPORT);
			sendRawData(CMD_NOTIFY_ON_COMPLETE);
			sendRawData(CMD_HANDSHAKE);
		}

        private void sendInternal(string command) 
		{
            if (serialPort == null) 
			{
                throw new IrToyException("The connection has been closed.");
            }

            if (!command.EndsWith(" ff ff")) 
			{
                throw new IrToyException("The command does not end with 'ff ff'.");
            }

            sendRawData(CMD_TRANSMIT);

            byte[] cmd = getCommandBytes(command);
            
			for (int i = 0; i < cmd.Length; i = i + IRTOY_BUFFER_SIZE) 
			{
                int len = Math.Min(cmd.Length - i, IRTOY_BUFFER_SIZE);
                byte[] bytesToSend = new byte[len];
                Array.Copy(cmd, i, bytesToSend, 0, len);
                sendRawData(bytesToSend);
            }
        }

        private byte[] getCommandBytes(string cmd) 
		{
			int len = (cmd.Length + 1) / 3;
         
			byte[] output = new byte[len];

            string[] hex = cmd.Split(' ');
            
			for (int i = 0; i < hex.Length; i++) 
			{
                int intValue = Convert.ToInt32(hex[i], 16);
                output[i] = Convert.ToByte(intValue);
            }
            
			return output;
        }

        private void sendRawData(byte[] data) 
		{
            serialPort.Write(data, 0, data.Length);
        }

        private byte[] readRawData(int numBytes) 
		{
            byte[] buffer = new byte[numBytes];

			for (int i = 0; i < numBytes; i++)
			{
				buffer[i] = (byte)serialPort.ReadByte();				
			}
				
			return buffer;
        }

        private string readString(int length) 
		{
            byte[] response = readRawData(length);
            return Encoding.Default.GetString(response);
        }
    }
}
