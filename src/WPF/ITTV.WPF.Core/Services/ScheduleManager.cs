using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Enums;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Providers.MireaApi;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetGroups;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetScheduleForGroup;

namespace ITTV.WPF.Core.Services
{
    public class ScheduleManager
    {
        private const int CountOfClasses = 7;

        private readonly List<string> _supportedDegrees = new()
        {
            "Бакалавриат",
            "Магистратура"
        };
        private readonly MireaApiProvider _mireaApiProvider;

        public ScheduleManager(MireaApiProvider mireaApiProvider)
        {
            _mireaApiProvider = mireaApiProvider;
        }

        public async Task<ApiGroupsItem[]> GetGroupTypesForCourse(DegreeEnum degree, int courseNumber)
        {
            var allGroups = await _mireaApiProvider.GetGroups();
            if (degree == DegreeEnum.Bachelor)
            {
                var groups = courseNumber switch
                {
                    1 => allGroups.Bachelor.First,
                    2 => allGroups.Bachelor.Second,
                    3 => allGroups.Bachelor.Third,
                    4 => allGroups.Bachelor.Fourth,
                    _ => throw new ArgumentException($"Unsupported bachelor courseNumber {courseNumber}")
                };

                return groups;
            }

            if (degree == DegreeEnum.Master)
            {
                var groups = courseNumber switch
                {
                    1 => allGroups.Master.First,
                    2 => allGroups.Master.Second,
                    _ => throw new ArgumentException($"Unsupported master courseNumber {courseNumber}")
                };

                return groups;
            }

            throw new ArgumentException($"Unsupported degree type {degree}");
        }

        public async Task<string[]> GetGroups(DegreeEnum degree, int courseNumber, string groupType)
        {
            var groupTypes = await GetGroupTypesForCourse(degree, courseNumber);
            var groups = groupTypes.FirstOrDefault(x => groupType == x.Name)?.Numbers;
            if (groups == null)
            {
                throw new ArgumentException(
                    $"Unsupported request degree: {degree}, groupType: {groupType}, courseNumber: {courseNumber}");
            }

            return groups;
        }

        public IEnumerable<SelectedScheduleTypeEnum> GetSupportedScheduleTypes()
        { 
            var enums = (SelectedScheduleTypeEnum[]) Enum.GetValues(typeof(SelectedScheduleTypeEnum));
            return enums.Skip(1);
        }

        public async Task<ApiScheduleLesson[]> GetLessonsForDay(string groupName, SelectedScheduleTypeEnum scheduleType)
        {
            var calculatingDay = scheduleType switch
            {
                SelectedScheduleTypeEnum.Today => DateTime.Today,
                SelectedScheduleTypeEnum.Tomorrow => DateTime.Today.AddDays(1),
                _ => throw new ArgumentException($"Unsupported schedule type {scheduleType}")
            };

            var weekNumber = MireaTimeHelper.CalculateNumberOfWeek(calculatingDay);

            if (weekNumber is null)
                return Array.Empty<ApiScheduleLesson>();
            
            var schedule = await _mireaApiProvider.GetFullSchedule(groupName);

            var selectedWeek = weekNumber switch
            {
                1 => schedule.FirstWeek,
                2 => schedule.SecondWeek,
                _ => throw new ArgumentOutOfRangeException()
            };

            var lessons = calculatingDay.DayOfWeek switch
            {
                DayOfWeek.Monday => selectedWeek.Monday,
                DayOfWeek.Tuesday => selectedWeek.Tuesday,
                DayOfWeek.Wednesday => selectedWeek.Wednesday,
                DayOfWeek.Thursday => selectedWeek.Thursday,
                DayOfWeek.Friday => selectedWeek.Friday,
                DayOfWeek.Saturday => selectedWeek.Saturday,
                DayOfWeek.Sunday => Array.Empty<ApiScheduleLesson>(),
                _ => throw new ArgumentOutOfRangeException()
            };
            return lessons;
        }

        public async Task<ApiFullScheduleResponse> GetFullSchedule(string groupName)
        {
            var schedule = await _mireaApiProvider.GetFullSchedule(groupName);
            return schedule;
        }
        
        public IEnumerable<int> GetSupportedCoursesForDegree(DegreeEnum degree)
            => degree switch
            {
                DegreeEnum.Bachelor => Enumerable.Range(1, 4),
                DegreeEnum.Master => Enumerable.Range(1, 2),
                _ => throw new ArgumentException($"Unsupported degree {degree}")
            };

        public IEnumerable<string> GetSupportedDegrees()
            => _supportedDegrees.AsEnumerable();
    }
}