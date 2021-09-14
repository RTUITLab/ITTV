using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.Network
{
    class TimeTableNetwork : IDisposable
    {
        public enum TimeTableEnum
        {
            today,
            tomorrow,
            full_schedule
        }


        private readonly HttpClient _client = new HttpClient();
        string BaseURL = "https://schedule-rtu.rtuitlab.dev/api/schedule/";

        public async Task GetGroupsToFile()
        {
            try
            {
                var str = await _client.GetStringAsync(BaseURL + "get_groups");
                File.WriteAllText("Settings/groups.json", str);
            }
            catch (HttpRequestException exception)
            {
                MainWindow.Instance.Log(exception.ToString());
            }
        }

        public async Task<TResponse> GetTimeTable<TResponse>(string group, TimeTableEnum time)
            where TResponse: class
        {
            var uri = BaseURL + group + "/" + time;
            try
            {
                var response = await _client.GetStringAsync(uri);
                var answer = JsonConvert.DeserializeObject<TResponse>(response);
                return answer;
            } catch (HttpRequestException exception)
            {
                MainWindow.Instance.Log(exception.ToString());
                return null;
            } catch (TaskCanceledException exception)
            {
                MainWindow.Instance.Log(exception.ToString());
                return null;
            }
        }
        
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
