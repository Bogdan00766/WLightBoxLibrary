using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WLightBox.Library
{


    public class Connection
    {
        //List<String> deviceIps = new List<String>();
        public List<String> deviceIps { get; set; }
        public Device[] devices { get; set; }
        
        public void FindDevicesAddresses()
        {
            deviceIps = new List<string>();
            List<string> subnets = GetLocalSubnetsList();
            foreach (var subnet in subnets)
            {
                for (int i = 2; i <= 255; i++)
                {
                    string address = subnet + '.' + i;
                    SendRequest(address, 1);
                }
            }
        }

        private List<string> GetLocalSubnetsList()
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
                throw new Exception("Not connected to network");
            }
        }

        

        public void SendRequest(string address, int attempts)
        {
            for (int i = 0; i < attempts; i++)
            {
                string requestString = $"http://{address}/info";
                new Thread(async delegate ()
                {
                    try
                    {
                        HttpClient client = new HttpClient();
                        await client.GetStringAsync(requestString);
                        requestString = requestString.Split("//")[1];
                        requestString = requestString.Split('/')[0];
                        deviceIps.Add(requestString);
                    }
                    catch
                    {
                    }
                }).Start();
            }
        }

        private void PingCompleted(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;
            //Console.WriteLine(ip);

            if(e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                Console.WriteLine(ip);
            }
        }
    }

    
}
