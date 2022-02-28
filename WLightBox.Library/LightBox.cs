using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WLightBox.Library
{
    public class LightBox
    {
        public List<Device> Devices { set; get; }
        private Ping _ping;
        private HttpClient _httpClient;
        public LightBox()
        {
            Devices = new List<Device>();
            _ping = new Ping();
            _httpClient = new HttpClient();
        }

        public Task ResearchDevices()
        {
            Devices.Clear();
            SetupConnection();
            return Task.CompletedTask;
        }
        public Task SetupConnection()
        {
            Connection connection = new Connection();
            connection.FindDevicesAddresses();
            foreach (string ip in connection.deviceIps)
            {
                Devices.Add(new Device(ip, _httpClient, _ping));
            }
            return Task.CompletedTask;
        }
    }

}
