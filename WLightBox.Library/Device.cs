using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WLightBox.Library
{
    public class Device
    {
        public string Ip { get; set; }
        HttpClient client;
        public Device(string ip)
        {
            this.Ip = ip;
            client = new HttpClient();
        }

        public bool IsConnected()
        {
            Ping p = new Ping();
            try
            {
                var pingResponse = p.Send(Ip);
                return pingResponse != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<String> GetCurrentColorAsync()
        {
            
            String requestString = $"http://{Ip}/api/rgbw/state";
            if (IsConnected())
            {
                try
                {
                    String jsonResponse = await client.GetStringAsync(requestString);
                    //response = response.Split(@"""currentColor"": """)[1].Split('"')[0];
                    var obj = JObject.Parse(jsonResponse);
                    string response = obj["rgbw"]["currentColor"].ToString();
                    return response;
                }
                catch(Exception ex)
                {
                    return String.Empty;
                }
            }
            return String.Empty;
        }
    }
}
