using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WLightBox.Libraryold
{
    public class Device
    {
        public string Ip { get; set; }
        HttpClient client;
        public Device(string ip)
        {
            Ip = ip;
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

        public async Task<string> GetCurrentColorAsync()
        {

            string requestString = $"http://{Ip}/api/rgbw/state";
            if (IsConnected())
            {
                try
                {
                    string jsonResponse = await client.GetStringAsync(requestString);
                    //response = response.Split(@"""currentColor"": """)[1].Split('"')[0];
                    var obj = JObject.Parse(jsonResponse);
                    string response = obj["rgbw"]["currentColor"].ToString();
                    return response;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public async Task<string> GetCurrentEffectAsync()
        {

            string requestString = $"http://{Ip}/api/rgbw/extended/state";
            if (IsConnected())
            {
                try
                {
                    string jsonResponse = await client.GetStringAsync(requestString);
                    //response = response.Split(@"""currentColor"": """)[1].Split('"')[0];
                    var obj = JObject.Parse(jsonResponse);
                    string effectID = obj["rgbw"]["effectID"].ToString();
                    string response = obj["rgbw"]["effectNames"][effectID].ToString();
                    return response;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public async Task SetColorAsync(string color)
        {
            string requestUrl = $"http://{Ip}/api/rgbw/set";
            string requestJson = JsonConvert.SerializeObject(new { rgbw = new { desiredColor = color } });
            var content = new StringContent(requestJson.ToString(), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(requestUrl, content);
        }
        public async Task SetEffectAsync(int effectId)
        {
            string requestUrl = $"http://{Ip}/api/rgbw/set";
            string requestJson = JsonConvert.SerializeObject(new { rgbw = new { effectID = effectId.ToString() } });
            var content = new StringContent(requestJson.ToString(), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(requestUrl, content);
        }
    }
}
