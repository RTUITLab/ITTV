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

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    class TimeTable : Singleton<TimeTable>
    {
        private Dictionary<string, Dictionary<string, Dictionary<string, List<int>>>> timetable = null;

        private Dictionary<string, string> rusLevels;
        private Dictionary<string, string> rusCourses;
        private List<string> GroupNames;
        private Dictionary<string, int> Groups;
        private List<string> Times;
        private List<Lesson> Lessons;

        private List<string> choosing = new List<string>();

        public async Task<bool> GetTimetable()
        {
            if (timetable == null) {
                await TimeTableNetwork.Instance.GetGroups();

                string json = File.ReadAllText("Settings/groups.json");

                try
                {
                    timetable = JsonConvert.DeserializeObject<JObject>(json).ToObject<Dictionary<string, Dictionary<string, Dictionary<string, List<int>>>>>();

                    return true;
                } catch (Exception e)
                {
                    return false;
                }
            } else
            {
                return true;
            }
        }

        public void Choose(string variant)
        {
            if (choosing.Count == 0 && rusLevels != null)
            {
                if (rusLevels.ContainsKey(variant)) {
                    choosing.Add(rusLevels[variant]);
                }
            } else if (choosing.Count == 1 && rusCourses != null)
            {
                if (rusCourses.ContainsKey(variant)) {
                    choosing.Add(rusCourses[variant]);
                }
            }
            else if (choosing.Count == 2 && GroupNames != null)
            {
                if (GroupNames.Contains(variant))
                {
                    choosing.Add(variant);
                }
            }
            else if (choosing.Count == 3 && Groups != null)
            {
                if (Groups.ContainsKey(variant))
                {
                    choosing.Add(Groups[variant].ToString());
                }
            }
            else if (choosing.Count == 4 && Times != null)
            {
                if (Times.Contains(variant))
                {
                    choosing.Add(variant);
                }
            }

        }

        public void Back()
        {
            if (choosing.Count > 0) {
                choosing.RemoveAt(choosing.Count - 1);

                switch (choosing.Count)
                {
                    case 0:
                        rusCourses = null;
                        GroupNames = null;
                        Groups = null;
                        Times = null;
                        break;
                    case 1:
                        GroupNames = null;
                        Groups = null;
                        Times = null;
                        break;
                    case 2:
                        Groups = null;
                        Times = null;
                        break;
                    case 3:
                        Times = null;
                        break;
                }
            }
        }

        public async Task<UserControl> GetContent()
        {
            if (timetable == null)
            {
                await GetTimetable();
            }

            switch (choosing.Count)
            {
                case 0:
                    return new TimeTableList(GetLevels());
                case 1:
                    return new TimeTableList(GetCourses());
                case 2:
                    return new TimeTableList(GetGroupsName());
                case 3:
                    return new TimeTableList(GetGroupsNumber());
                case 4:
                    return new TimeTableList(GetTimeTableForTime());
                case 5:
                    return new TimeTableList(await GetTimeTable());
                default:
                    return null;
            }
        }

        public bool IsOut()
        {
            return choosing.Count == 0;
        }

        private List<string> GetLevels()
        {
            rusLevels = null;
            rusCourses = null;
            GroupNames = null;
            Groups = null;
            Times = null;
            Lessons = null;

            rusLevels = new Dictionary<string, string>();

            var keys = timetable.Keys;
            foreach (var key in keys)
            {
                switch (key)
                {
                    case "bachelor":
                        rusLevels.Add("Бакалавриат", key);
                        break;
                    case "master":
                        rusLevels.Add("Магистратура", key);
                        break;
                }
            }

            return rusLevels.Keys.ToList();
        }

        private List<string> GetCourses()
        {
            rusCourses = null;
            GroupNames = null;
            Groups = null;
            Times = null;
            Lessons = null;

            if (timetable.ContainsKey(choosing[0])) {
                rusCourses = new Dictionary<string, string>();

                var keys = timetable[choosing[0]].Keys;
                foreach (var key in keys)
                {
                    switch (key)
                    {
                        case "1":
                            rusCourses.Add("1-ый курс", key);
                            break;
                        case "2":
                            rusCourses.Add("2-ой курс", key);
                            break;
                        case "3":
                            rusCourses.Add("3-ий курс", key);
                            break;
                        case "4":
                            rusCourses.Add("4-ый курс", key);
                            break;
                    }
                }

                return rusCourses.Keys.ToList();
            } else
            {
                Back();
                return null;
            }
        }

        private List<string> GetGroupsName()
        {
            GroupNames = null;
            Groups = null;
            Times = null;
            Lessons = null;

            if (timetable[choosing[0]].ContainsKey(choosing[1]))
            {
                GroupNames = timetable[choosing[0]][choosing[1]].Keys.ToList();

                return GroupNames;
            }
            else
            {
                Back();
                return null;
            }
        }

        private List<string> GetGroupsNumber()
        {
            Groups = null;
            Times = null;
            Lessons = null;

            if (timetable[choosing[0]][choosing[1]].ContainsKey(choosing[2]))
            {
                Groups = new Dictionary<string, int>();
                foreach (int i in timetable[choosing[0]][choosing[1]][choosing[2]])
                {
                    Groups.Add(choosing[2] + "-" + i.ToString() + "-" + (DateTime.Now.Year + (DateTime.Now.Month < 9 ? 0 : 1) - int.Parse(choosing[1])).ToString() , i);
                }

                return Groups.Keys.ToList();
            }
            else
            {
                Back();
                return null;
            }
        } 
        
        private List<string> GetTimeTableForTime()
        {
            Times = null;
            Lessons = null;

            if (timetable[choosing[0]][choosing[1]][choosing[2]].Contains(int.Parse(choosing[3])))
            {
                Times = new List<string>() { "Сегодня", "Завтра", "Общее"};

                return Times;
            }
            else
            {
                Back();
                return null;
            }
        }

        private async Task<List<Lesson>> GetTimeTable()
        {
            Lessons = null;

            if (choosing[4].Equals("Сегодня") || choosing[4].Equals("Завтра") || choosing[4].Equals("Общее"))
            {
                switch (choosing[4])
                {
                    case "Сегодня":
                        return await TimeTableNetwork.Instance.GetTimeTable(Groups.FirstOrDefault(x => x.Value == int.Parse(choosing[3])).Key, TimeTableNetwork.TimeTableTime.today);  // Change to taking data from cache
                    case "Завтра":
                        return await TimeTableNetwork.Instance.GetTimeTable(Groups.FirstOrDefault(x => x.Value == int.Parse(choosing[3])).Key, TimeTableNetwork.TimeTableTime.tomorrow);  // Change to taking data from cache
                    case "Общее":
                        return await TimeTableNetwork.Instance.GetTimeTable(Groups.FirstOrDefault(x => x.Value == int.Parse(choosing[3])).Key, TimeTableNetwork.TimeTableTime.today);  // Change to taking data from cache
                    default:
                        return null;
                }
            } else
            {
                Back();
                return null;
            }
        }
    }
}
