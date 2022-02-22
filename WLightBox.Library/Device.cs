using Newtonsoft.Json;
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

        public async Task<String> GetCurrentEffectAsync()
        {

            String requestString = $"http://{Ip}/api/rgbw/extended/state";
            if (IsConnected())
            {
                try
                {
                    String jsonResponse = await client.GetStringAsync(requestString);
                    //response = response.Split(@"""currentColor"": """)[1].Split('"')[0];
                    var obj = JObject.Parse(jsonResponse);
                    Console.WriteLine(jsonResponse.ToString());
                    string effectID = obj["rgbw"]["effectID"].ToString();
                    string response = obj["rgbw"]["effectNames"][effectID].ToString();
                    return response;
                }
                catch (Exception ex)
                {
                    return String.Empty;
                }
            }
            return String.Empty;
        }

        public async void SetColorAsync(string color)
        {
            String requestUrl = $"http://{Ip}/api/rgbw/set";
            String requestJson = JsonConvert.SerializeObject(new { rgbw = new { desiredColor = color } });
            var content = new StringContent(requestJson.ToString(), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(requestUrl, content);
        }
    }
}
