using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.ProcessPlugins.Auto3D.Devices.SonyAPI_Lib
{
    public class SonyCommandList
    {
        /// <summary>
        /// Gets or Sets the Devices Command List ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Gets or Sets the Devices Command List Results
        /// </summary>
        public List<object> result { get; set; }
    }
}
