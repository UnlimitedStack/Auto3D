using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MediaPortal.GUI.Library;
using Newtonsoft.Json;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices
{
	public abstract class JointSpaceBaseAdapter : IPhilipsTVAdapter
	{
		protected string Host { get; set; }

		private const string ContentType = "application/json; charset=UTF-8"; 

		public virtual bool SendCommand(string command)
		{
			return PostRequest(KeyUri, new JointSpaceKey { key = command });
		}

		public virtual void Connect(string host)
		{
			Host = host;
		}

		public virtual SystemBase TestConnection(string host)
		{
			return null;
		}

		public virtual void Disconnect()
		{
			Host = string.Empty;
		}

		public virtual bool IsConnected
		{
			get
			{
				if (!string.IsNullOrEmpty(Host))
				{
					return !string.IsNullOrEmpty(GetRequest(SystemUri, string.Empty));
				}

				return false;
			}
		}

		protected abstract string SystemUri { get; }
		protected abstract string KeyUri { get; }

		protected bool PostRequest(string url, JointSpaceKey key)
		{
			try
			{
				Log.Debug("Auto3D: PostRequest to URL = \"" + url + "\"");

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

				request.Timeout = 3000;
				request.ContentType = ContentType;
				request.Method = "POST";

				var jsonString = JsonConvert.SerializeObject(key, Formatting.Indented);
				Log.Debug("Auto3D: JSON-String = \"" + jsonString + "\"");

				using (var streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					streamWriter.Write(jsonString);
					streamWriter.Flush();
					streamWriter.Close();
				}

				Application.DoEvents();
				Thread.Sleep(50);

				using (var httpResponse = (HttpWebResponse)request.GetResponse())
				{
					using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
					{
						var result = streamReader.ReadToEnd();
						Log.Debug(result);
					}
				}

				Application.DoEvents();
			}
			catch (Exception ex)
			{
				Log.Info("Auto3D: PostRequest: " + ex.Message);
				Auto3DHelpers.ShowAuto3DMessage("Command to TV could not be sent: " + ex.Message, false, 0);
				return false;
			}

			return true;
		}

		protected string GetRequest(string url, string jsonString)
		{
			string result = string.Empty;			
			
			try
			{
				Log.Debug("Auto3D: GetRequest to URL = \"" + url + "\"");

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

				request.Timeout = 3000;
				request.ContentType = ContentType;
				request.Accept = ContentType;
				request.Method = "GET";

				if (!string.IsNullOrEmpty(jsonString))
				{
					Log.Debug("Auto3D: JSON-String = \"" + jsonString + "\"");

					using (var streamWriter = new StreamWriter(request.GetRequestStream()))
					{
						streamWriter.Write(jsonString);
						streamWriter.Flush();
						streamWriter.Close();
					}
				}

				Application.DoEvents();
				Thread.Sleep(50);

				using (var httpResponse = (HttpWebResponse)request.GetResponse())
				{
					using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
					{
						result = streamReader.ReadToEnd();
						Log.Debug(result);
					}
				}

				Application.DoEvents();
			}
			catch (Exception ex)
			{
				Log.Info("Auto3D: GetRequest: " + ex.Message);
				Auto3DHelpers.ShowAuto3DMessage("Request to TV could not be sent: " + ex.Message, false, 0);
				result = string.Empty;
			}

			return result;
		}
	}
}
