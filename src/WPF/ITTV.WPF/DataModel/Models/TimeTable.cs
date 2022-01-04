using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ITTV.WPF.Interface.Pages;
using ITTV.WPF.Network;
using Newtonsoft.Json;

namespace ITTV.WPF.DataModel.Models
{
    public class TimeTable : Singleton<TimeTable>
    {
        private readonly TimeTableNetwork network = new TimeTableNetwork();
        //TODO: rename field
        private readonly List<string> answers = new List<string>();
        private Groups groups;

        public TimeTable()
        {
            Configure();
        }
        
        //TODO: Refactoring, not right using of async void
        async void Configure()
        {
            var groupsInfoPath = AllPaths.FileGroupsCachePath;
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
            var files = Directory.GetFiles(AllPaths.GetDirectoryTimeTablesPath);

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
                            groups.Bachelor.First.ForEach(direction => { outList.Add(direction.Name); });
                            return new TimeTableList(outList, "Назад к выбору курса");
                        case "2-ой курс":
                            groups.Bachelor.Second.ForEach(direction => { outList.Add(direction.Name); });
                            return new TimeTableList(outList, "Назад к выбору курса");
                        case "3-ий курс":
                            groups.Bachelor.Third.ForEach(direction => { outList.Add(direction.Name); });
                            return new TimeTableList(outList, "Назад к выбору курса");
                        case "4-ый курс":
                            groups.Bachelor.Fourth.ForEach(direction => { outList.Add(direction.Name); });
                            return new TimeTableList(outList, "Назад к выбору курса");
                    }
                } 
                else if (answers[0].Equals("Магистратура"))
                {
                    List<string> outList = new List<string>();
                    switch (answers[1])
                    {
                        case "1-ый курс":
                            groups.Master.First.ForEach(direction => { outList.Add(direction.Name); });
                            return new TimeTableList(outList, "Назад к выбору курса");
                        case "2-ой курс":
                            groups.Master.Second.ForEach(direction => { outList.Add(direction.Name); });
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
                            groups.Bachelor.First.ForEach(direction => { if (direction.Name.Equals(answers[2])) { outList.AddRange(direction.Numbers); } });
                            return new TimeTableList(outList, "Назад к выбору направления");
                        case "2-ой курс":
                            groups.Bachelor.Second.ForEach(direction => { if (direction.Name.Equals(answers[2])) { outList.AddRange(direction. Numbers); } });
                            return new TimeTableList(outList, "Назад к выбору направления");
                        case "3-ий курс":
                            groups.Bachelor.Third.ForEach(direction => { if (direction.Name.Equals(answers[2])) { outList.AddRange(direction.Numbers); } });
                            return new TimeTableList(outList, "Назад к выбору направления");
                        case "4-ый курс":
                            groups.Bachelor.Fourth.ForEach(direction => { if (direction.Name.Equals(answers[2])) { outList.AddRange(direction.Numbers); } });
                            return new TimeTableList(outList, "Назад к выбору направления");
                    }
                }
                else if (answers[0].Equals("Магистратура"))
                {
                    List<string> outList = new List<string>();
                    switch (answers[1])
                    {
                        case "1-ый курс":
                            groups.Master.First.ForEach(direction => { if (direction.Name.Equals(answers[2])) { outList.AddRange(direction.Numbers); } });
                            return new TimeTableList(outList, "Назад к выбору направления");
                        case "2-ой курс":
                            groups.Master.Second.ForEach(direction => { if (direction.Name.Equals(answers[2])) { outList.AddRange(direction.Numbers); } });
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
                        var todayLessons = await GetTodaySchedule(answers[3]);
                        MainWindow.Instance.content.NavigateTo(new SchedulePage(todayLessons));
                        UnChoose();
                        return null;
                    case "Завтра":
                        var tommorowLessons = await GetTomorrowSchedule(answers[3]);
                        MainWindow.Instance.content.NavigateTo(new SchedulePage(tommorowLessons));
                        UnChoose();
                        return null;
                    case "Общее":
                        var allLessons = await GetFullSchedule(answers[3]);
                        MainWindow.Instance.content.NavigateTo(new SchedulePage(allLessons));
                        UnChoose();
                        return null;
                }
            }
            return null;
        }

        //TODO: Edit name of method
        public bool GetAll()
        {
            return answers.Count == 2 && Directory.GetFiles(AllPaths.GetDirectoryTimeTablesPath).Length > 0;
        }
    }
}
