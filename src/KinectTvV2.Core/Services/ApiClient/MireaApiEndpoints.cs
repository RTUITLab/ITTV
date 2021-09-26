namespace KinectTvV2.Core.Services.ApiClient
{
    public static class MireaApiEndpoints
    {
        public const string NewsBaseAddress = "http://www.mirea.ru";
        public const string ScheduleBaseAddress = "http://schedule.mirea.ninja:5000/api/schedule";

        public static string GetNewsEndpoint => NewsBaseAddress + "/news";
        public static string GetAllGroups => ScheduleBaseAddress + "/groups";

        public static string GetScheduleForGroup(string groupName)
            => ScheduleBaseAddress + $"/{groupName}" + "/full_schedule";
    }
}
