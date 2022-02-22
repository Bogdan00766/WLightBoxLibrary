using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WLightBox.Libraryold
{


    public class Connection
    {
        List<string> deviceIps = new List<string>();
        public Device[] devices { get; set; }

        public Connection()
        {
            FindDevicesIps();
            devices = new Device[deviceIps.Count];
            for (int i = 0; i < devices.Length; i++)
            {
                devices[i] = new Device(deviceIps[i]);
            }
        }



        public void FindDevicesIps()
        {
            HttpClient client = new HttpClient();
            List<string>? subnets = GetLocalSubnetsList();
            if (subnets == null) return;
            foreach (var subnet in subnets)
            {
                for (int i = 1; i < 255; i++)
                {
                    string requestString = $"http://{subnet}.{i}/info";
                    sendRequestAsync(client, requestString);
                }

            }
            return;

        }

        private async void sendRequestAsync(HttpClient client, string requestString)
        {
            try
            {
                await client.GetStringAsync(requestString);
                requestString = requestString.Split("//")[1];
                requestString = requestString.Split('/')[0];
                deviceIps.Add(requestString);
            }
            catch (Exception ex)
            {

            }
        }

        private List<string>? GetLocalSubnetsList()
        {
            List<string>? result = new List<string>();
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                if (host != null)
                {
                    foreach (var x in host.AddressList)
                    {
                        if (x != null && x.ToString().Contains('.'))
                        {
                            var elements = x.ToString().Split('.');
                            string subnet = elements[0] + '.' + elements[1] + '.' + elements[2];
                            result.Add(subnet);
                        }
                    }
                }
                return result;
            }
            else
            {
                Console.WriteLine("Not connected");
                return null;
            }
        }
    }
}
