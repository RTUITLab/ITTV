using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Interface.Pages;
using ITTV.WPF.Network;
using Newtonsoft.Json;

namespace ITTV.WPF.DataModel.Models
{
    class TimeTable : Singleton<TimeTable>
    {
        private readonly TimeTableNetwork network = new TimeTableNetwork();
        private readonly List<string> answers;
        private Groups groups;

        public TimeTable()
        {
            Configure();
            answers = new List<string>();
        }

        async void Configure()
        {
            const string groupsInfoPath = "Settings/groups.json";
            if (!File.Exists(groupsInfoPath) || string.IsNullOrEmpty(File.ReadAllText(groupsInfoPath)))
            {
                await network.SyncGroupsToFile();
            }
            else
            {
                var updateTime = TimeSpan.FromMinutes(30); 
                var groupsJson = File.ReadAllText(groupsInfoPath);
                var newGroups = JsonConvert.DeserializeObject<Groups>(groupsJson);
                if (newGroups != null && (!newGroups.Updated.HasValue || groups.Updated - DateTime.Now > updateTime))
                {
                    await network.SyncGroupsToFile();
                }
            }

            var json = File.ReadAllText(groupsInfoPath);
            groups = JsonConvert.DeserializeObject<Groups>(json);
        }

        private async Task<FullSchedule> GetFullSchedule(string group)
        {
            return await network.GetTimeTable<FullSchedule>(group, TimeTableNetwork.TimeTableEnum.full_schedule);
        }
        private async Task<List<Lesson>> GetTodaySchedule(string group)
        {
            return await network.GetTimeTable<List<Lesson>>(group, TimeTableNetwork.TimeTableEnum.today);
        }
        private async Task<List<Lesson>> GetTomorrowSchedule(string group)
        {
            return await network.GetTimeTable<List<Lesson>>(group, TimeTableNetwork.TimeTableEnum.tomorrow);
        }

        public string GetImageSourceTimeTable()
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"TimeTables/");

            if (files.Length > 0) {
                if (answers[0].Equals("Бакалавриат")) {
                    switch (answers[1])
                    {
                        case "1-ый курс":
                            return files[0];
                        case "2-ой курс":
                            return files[1];
                        case "3-ий курс":
                            return files[2];
                        case "4-ый курс":
                            return files[3];
                    }
                } else if (answers[0].Equals("Магистратура"))
                {
                    switch (answers[1])
                    {
                        case "1-ый курс":
                            return files[4];
                        case "2-ой курс":
                            return files[5];
                    }
                }
            }
            return "";
        }

        public async Task<TimeTableList> Choose(string answer)
        {
            answers.Add(answer);
            return await GetContent();
        }

        public void UnChoose()
        {
            if (answers.Count > 0) {
                answers.RemoveAt(answers.Count - 1);
            }
        }

        public void CloseTimeTable()
        {
            answers.Clear();
        }

        public async Task<TimeTableList> GetContent()
        {
            if (answers.Count == 0)
            {
                return new TimeTableList(new List<string>() { "Бакалавриат", "Магистратура" }, "");
            } 
            else if (answers.Count == 1)
            {
                if (answers[0].Equals("Бакалавриат")) {
                    return new TimeTableList(new List<string>() { "1-ый курс", "2-ой курс", "3-ий курс", "4-ый курс" }, "Назад к уровням подготовки"); 
                } 
                else if (answers[0].Equals("Магистратура")) {
                    return new TimeTableList(new List<string>() { "1-ый курс", "2-ой курс" }, "Назад к уровням подготовки"); 
                }
            }
            else if (answers.Count == 2)
            {
                if (answers[0].Equals("Бакалавриат"))
                {
                    List<string> outList = new List<string>();
                    switch (answers[1])
                    {
                        case "1-ый курс":
                            groups.bachelor.first.ForEach(direction => { outList.Add(direction.name); });
                            return new TimeTableList(outList, "Назад к выбору курса");
                        case "2-ой курс":
                            groups.bachelor.second.ForEach(direction => { outList.Add(direction.name); });
                            return new TimeTableList(outList, "Назад к выбору курса");
                        case "3-ий курс":
                            groups.bachelor.third.ForEach(direction => { outList.Add(direction.name); });
                            return new TimeTableList(outList, "Назад к выбору курса");
                        case "4-ый курс":
                            groups.bachelor.fourth.ForEach(direction => { outList.Add(direction.name); });
                            return new TimeTableList(outList, "Назад к выбору курса");
                    }
                } 
                else if (answers[0].Equals("Магистратура"))
                {
                    List<string> outList = new List<string>();
                    switch (answers[1])
                    {
                        case "1-ый курс":
                            groups.master.first.ForEach(direction => { outList.Add(direction.name); });
                            return new TimeTableList(outList, "Назад к выбору курса");
                        case "2-ой курс":
                            groups.master.second.ForEach(direction => { outList.Add(direction.name); });
                            return new TimeTableList(outList, "Назад к выбору курса");
                    }
                }
            }
            else if (answers.Count == 3)
            {
                if (answers[0].Equals("Бакалавриат"))
                {
                    List<string> outList = new List<string>();
                    switch (answers[1])
                    {
                        case "1-ый курс":
                            groups.bachelor.first.ForEach(direction => { if (direction.name.Equals(answers[2])) { outList.AddRange(direction.numbers); } });
                            return new TimeTableList(outList, "Назад к выбору направления");
                        case "2-ой курс":
                            groups.bachelor.second.ForEach(direction => { if (direction.name.Equals(answers[2])) { outList.AddRange(direction.numbers); } });
                            return new TimeTableList(outList, "Назад к выбору направления");
                        case "3-ий курс":
                            groups.bachelor.third.ForEach(direction => { if (direction.name.Equals(answers[2])) { outList.AddRange(direction.numbers); } });
                            return new TimeTableList(outList, "Назад к выбору направления");
                        case "4-ый курс":
                            groups.bachelor.fourth.ForEach(direction => { if (direction.name.Equals(answers[2])) { outList.AddRange(direction.numbers); } });
                            return new TimeTableList(outList, "Назад к выбору направления");
                    }
                }
                else if (answers[0].Equals("Магистратура"))
                {
                    List<string> outList = new List<string>();
                    switch (answers[1])
                    {
                        case "1-ый курс":
                            groups.master.first.ForEach(direction => { if (direction.name.Equals(answers[2])) { outList.AddRange(direction.numbers); } });
                            return new TimeTableList(outList, "Назад к выбору направления");
                        case "2-ой курс":
                            groups.master.second.ForEach(direction => { if (direction.name.Equals(answers[2])) { outList.AddRange(direction.numbers); } });
                            return new TimeTableList(outList, "Назад к выбору направления");
                    }
                }
            }
            else if (answers.Count == 4)
            {
                return new TimeTableList(new List<string>() { "Сегодня", "Завтра", "Общее" }, "Назад к выбору группы");
            }
            else if (answers.Count >= 5)
            {
                switch(answers[4])
                {
                    case "Сегодня":
                        List<Lesson> todayLessons = await GetTodaySchedule(answers[3]);
                        MainWindow.Instance.content.NavigateTo(new SchedulePage(todayLessons));
                        UnChoose();
                        return null;
                    case "Завтра":
                        List<Lesson> tommorowLessons = await GetTomorrowSchedule(answers[3]);
                        MainWindow.Instance.content.NavigateTo(new SchedulePage(tommorowLessons));
                        UnChoose();
                        return null;
                    case "Общее":
                        FullSchedule allLessons = await GetFullSchedule(answers[3]);
                        MainWindow.Instance.content.NavigateTo(new SchedulePage(allLessons));
                        UnChoose();
                        return null;
                }
            }
            return null;
        }

        public bool GetAll()
        {
            return answers.Count == 2 && Directory.Exists("TimeTables/") && Directory.GetFiles("TimeTables/").ToList().Count > 0;
        }
    }
}
