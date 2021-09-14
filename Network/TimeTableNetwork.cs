using Microsoft.Samples.Kinect.ControlsBasics.DataModel;
using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.Network
{
    class TimeTableNetwork
    {
        public enum TimeTableTime
        {
            today,
            tomorrow,
            full_schedule
        }


        HttpClient client = new HttpClient();
        string BaseURL = "https://schedule-rtu.rtuitlab.dev/api/schedule/";

        public async Task GetGroupsToFile()
        {
            client.DefaultRequestHeaders.Add("Accept-Charset", "");
            try
            {
                var response = await client.GetByteArrayAsync(BaseURL + "get_groups");
                var cyrilic_response = (Encoding.ASCII.GetString(response));

                File.WriteAllText("Settings/groups.json", cyrilic_response);
            }
            catch (HttpRequestException exception)
            {
                MainWindow.Instance.Log(exception.ToString());
            }
        }

        public async Task<List<Lesson>> GetTimeTable(string group, TimeTableTime time)
        {
            string uri = BaseURL + group + "/" + time.ToString();
            try
            {
                client.Timeout = new TimeSpan(5000);
                var response = await client.GetStringAsync(uri);
                List<Lesson> answer = JsonConvert.DeserializeObject<List<Lesson>>(response);
                return answer;
            } catch (HttpRequestException exception)
            {
                MainWindow.Instance.Log(exception.ToString());
                return null;
            } catch (TaskCanceledException exception)
            {
                //throw new Exception("Test");
                MainWindow.Instance.Log(exception.ToString());
                return null;
            }
        }
        public async Task<FullSchedule> GetAllTimeTable(string group)
        {
            string uri = BaseURL + group + "/" + TimeTableTime.full_schedule.ToString();
            try
            {
                var response = await client.GetStringAsync(uri);
                FullSchedule answer = JsonConvert.DeserializeObject<FullSchedule>(response);
                return answer;
            } catch (HttpRequestException exception){
                MainWindow.Instance.Log(exception.ToString());
                return null;
            } catch (TaskCanceledException exception)
            {
                //throw new Exception("Test");
                MainWindow.Instance.Log(exception.ToString());
                return null;
            }
        }
    }
}
