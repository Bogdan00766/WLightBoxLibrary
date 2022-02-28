using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WLightBox.Library
{
    public class Device
    {
        public string Ip { get; set; }
        HttpClient _client;
        Ping _ping;
              
        public Device(string ip, HttpClient client, Ping ping)
        {
            Ip = ip;
            _client = client;
            _ping = ping;
        }

        public bool IsConnected()
        {
            try
            {
                var pingResponse = _ping.Send(Ip);
                return pingResponse != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<List<string>> GetEffectsListAsync()
        {
            string requestString = $"http://{Ip}/api/rgbw/extended/state";
            List<string> output = new List<string>();
            if (IsConnected())
            {
                try
                {
                    string jsonResponse = await _client.GetStringAsync(requestString);
                    //response = response.Split(@"""currentColor"": """)[1].Split('"')[0];
                    var obj = JObject.Parse(jsonResponse);
                    var response = obj["rgbw"]["effectNames"];
                    for(int i = 0; i < 10; i++)
                    {
                        if (response[i.ToString()] != null)
                        {
                        Console.WriteLine(response[i.ToString()].ToString());
                        output.Add(response[i.ToString()].ToString());

                        }
                    }

                    return output;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        public async Task<Rgbww> GetCurrentColorAsync()
        {

            string requestString = $"http://{Ip}/api/rgbw/state";
            if (IsConnected())
            {
                try
                {
                    string jsonResponse = await _client.GetStringAsync(requestString);
                    //response = response.Split(@"""currentColor"": """)[1].Split('"')[0];
                    var obj = JObject.Parse(jsonResponse);
                    string response = obj["rgbw"]["currentColor"].ToString();
                    Rgbww rgbww = new Rgbww();
                    rgbww.Red = Int32.Parse(response.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    rgbww.Green = Int32.Parse(response.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    rgbww.Blue = Int32.Parse(response.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    rgbww.WarmWhite = Int32.Parse(response.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                    rgbww.ColdWhite = Int32.Parse(response.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);

                    return rgbww;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        public async Task<string> GetCurrentEffectAsync()
        {

            string requestString = $"http://{Ip}/api/rgbw/extended/state";
            if (IsConnected())
            {
                try
                {
                    string jsonResponse = await _client.GetStringAsync(requestString);
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

        public Task SetColor(int red, int green, int blue, int warm_w, int cold_w)
        {
            string r = Convert.ToString(red, 16);
            string g = Convert.ToString(green, 16);
            string b = Convert.ToString(blue, 16);
            string ww = Convert.ToString(warm_w, 16);
            string cw = Convert.ToString(cold_w, 16);

            if (warm_w < 16) ww = "0" + ww;
            if (cold_w < 16) cw = "0" + cw;
            if (red < 16) r = "0" + r;
            if (green < 16) g = "0" + g;
            if (blue < 16) b = "0" + b;

            string color = r + g + b + ww + cw;

            string requestUrl = $"http://{Ip}/api/rgbw/set";
            string requestJson = JsonConvert.SerializeObject(new { rgbw = new { desiredColor = color } });
            var content = new StringContent(requestJson.ToString(), Encoding.UTF8, "application/json");
            _client.PostAsync(requestUrl, content);
            return Task.CompletedTask;
        }
        public Task SetEffect(int effectId)
        {
            string requestUrl = $"http://{Ip}/api/rgbw/set";
            string requestJson = JsonConvert.SerializeObject(new { rgbw = new { effectID = effectId.ToString() } });
            var content = new StringContent(requestJson.ToString(), Encoding.UTF8, "application/json");
            _client.PostAsync(requestUrl, content);
            return Task.CompletedTask;
        }
    }

    
}
