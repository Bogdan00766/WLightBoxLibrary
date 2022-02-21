using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WLightBox.Library
{
    public class Connection
    {
        public async Task<String> FindDeviceIp()
        {
            HttpClient client = new HttpClient();
            List<String>? subnets = GetLocalSubnetsList();
            if (subnets == null) return null;
            foreach (var subnet in subnets)
            {
                for (int i = 1; i < 256; i++)
                {
                    String requestString = $"{subnet}.i/info";
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(requestString);
                        Console.WriteLine(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                }

            }
            return null;
        }
        public List<String>? GetLocalSubnetsList()
        {
            List<String>? result = new List<string>();
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                if (host != null)
                {
                    foreach (var x in host.AddressList)
                    {
                        if (x != null && x.ToString().Contains('.'))
                        {
                            var elements = x.ToString().Split('.');
                            String subnet = elements[0] + '.' + elements[1] + '.' + elements[2];
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
