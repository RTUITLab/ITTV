﻿namespace KinectTvV2.Core.Services.ApiClient
{
    public static class MireaApiEndpoints
    {
        public const string NewsBaseAddress = "https://www.mirea.ru";
        public const string ScheduleBaseAddress = "https://schedule-rtu.rtuitlab.dev/api/schedule";

        public static string GetNewsEndpoint => NewsBaseAddress + "/news";
        public static string GetAllGroups => ScheduleBaseAddress + "/get_groups";

        public static string GetScheduleForGroup(string groupName)
            => ScheduleBaseAddress + $"/{groupName}" + "/full_schedule";
    }
}
