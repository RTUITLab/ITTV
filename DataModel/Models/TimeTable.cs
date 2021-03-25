using Microsoft.Samples.Kinect.ControlsBasics.Interface.Pages;
using Microsoft.Samples.Kinect.ControlsBasics.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    class TimeTable : Singleton<TimeTable>
    {
        private TimeTableNetwork network = new TimeTableNetwork();
        List<int> answers = new List<int>();
        Groups Groups;

        public TimeTable()
        {
            Configure();
        }

        async void Configure()
        {
            if (!File.Exists("Settings/groups.json") || string.IsNullOrEmpty(File.ReadAllText("Settings/groups.json"))) {
                await network.GetGroupsToFile();
            }

            string json = File.ReadAllText("Settings/groups.json");
            Groups = JsonConvert.DeserializeObject<Groups>(json);
        }

        private async Task<FullSchedule> GetFullSchedule(string group)
        {
            return await network.GetAllTimeTable(group);
        }
        private async Task<List<Lesson>> GetTodaySchedule(string group)
        {
            return await network.GetTimeTable(group, TimeTableNetwork.TimeTableTime.today);
        }
        private async Task<List<Lesson>> GetTomorrowSchedule(string group)
        {
            return await network.GetTimeTable(group, TimeTableNetwork.TimeTableTime.tomorrow);
        }

        public string GetImageSourceTimeTable()
        {
            string[] files = Directory.GetFiles("Settings/TimeTables/");

            if (files.Length > 0) {
                if (answers[0] == 0) {
                    switch (answers[1])
                    {
                        case 0:
                            return files[0];
                        case 1:
                            return files[1];
                        case 2:
                            return files[2];
                        case 3:
                            return files[3];
                    }
                } else if (answers[0] == 1)
                {
                    switch (answers[1])
                    {
                        case 0:
                            return files[4];
                        case 1:
                            return files[5];
                    }
                }
            }
            return "";
        }

        public void Choose(int answer)
        {
            answers.Add(answer);
        }

        public List<string> GetContent()
        {
            if (answers.Count == 0)
            {
                return new List<string>() { "Бакалавриат", "Магистратура" };
            } else
            {
                return new List<string>();
            }
        }

        public bool GetAll()
        {
            return answers.Count == 2 && Directory.Exists("Settings/TimeTables/") && Directory.GetFiles("Settings/TimeTables/").ToList().Count > 0;
        }
    }
}
