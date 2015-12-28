using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices.SonyAPI_Lib
{
    public class SonyCookie
    {
        /// <summary>
        /// Gets or Sets the Cookie Comment
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Comment URI
        /// </summary>
        public object CommentUri { get; set; }
        /// <summary>
        /// Gets or Sets Cookie for HTTP Only
        /// </summary>
        public bool HttpOnly { get; set; }
        /// <summary>
        /// gets or Sets the Cookie Discard
        /// </summary>
        public bool Discard { get; set; }
        /// <summary>
        /// gets or Sets the Cookie Domain
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Expired
        /// </summary>
        public bool Expired { get; set; }
        /// <summary>
        /// Gets or Sets the Cookies Expiration
        /// </summary>
        public string Expires { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Path
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Gets or Sets the Is Cookie Secure
        /// </summary>
        public bool Secure { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Time Stamp
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or Sets the Cookie Version
        /// </summary>
        public int Version { get; set; }
    }
}
