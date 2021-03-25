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
            var response = await client.GetByteArrayAsync(BaseURL + "get_groups");
            var cyrilic_response = (Encoding.ASCII.GetString(response));

            File.WriteAllText("Settings/groups.json", cyrilic_response);
        }

        public async Task<List<Lesson>> GetTimeTable(string group, TimeTableTime time)
        {
            string uri = BaseURL + group + "/" + time.ToString();
            var response = await client.GetStringAsync(uri);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            //List<Lesson> answer = JsonConvert.DeserializeObject<List<Lesson>>(response, settings);

            //var custom_answer = JsonConvert.DeserializeObject<List<Dictionary<string, Dictionary<string, string>>>>(response, settings);
            return null; 
        }
        public async Task<FullSchedule> GetAllTimeTable(string group)
        {
            string uri = BaseURL + group + "/" + TimeTableTime.full_schedule.ToString();
            var response = await client.GetStringAsync(uri);
            FullSchedule answer = JsonConvert.DeserializeObject<FullSchedule>(response);

            //var custom_answer = JsonConvert.DeserializeObject<List<Dictionary<string, Dictionary<string, string>>>>(response, settings);
            return null;
        }
    }
}
