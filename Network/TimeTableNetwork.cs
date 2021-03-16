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
    class TimeTableNetwork : Singleton<TimeTableNetwork>
    {
        public enum TimeTableTime
        {
            today,
            tomorrow,
            all
        }


        HttpClient client = new HttpClient();

        string BaseURL = "https://schedule-rtu.rtuitlab.dev/api/schedule/";

        public async Task GetGroups()
        {
            var response = await client.GetStringAsync(BaseURL + "get_groups");

            File.WriteAllText("Settings/groups.json", response);
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
    }
}
