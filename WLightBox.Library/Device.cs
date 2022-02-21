using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLightBox.Library
{
    public class Device
    {
        public string Ip { get; set; }

        public Device(string ip)
        {
            this.Ip = ip;
        }
    }
}
