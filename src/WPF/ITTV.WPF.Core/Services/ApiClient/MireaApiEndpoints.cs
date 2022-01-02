namespace ITTV.WPF.Core.Services.ApiClient
{
    public static class MireaApiEndpoints
    {
        public const string NewsBaseAddress = "https://www.mirea.ru/";
        public const string ScheduleBaseAddress = "https://schedule-rtu.rtuitlab.dev/api/schedule";

        public static string GetNewsEndpoint => NewsBaseAddress + "news/";
        public static string GetAllGroups => ScheduleBaseAddress + "/get_groups";

        public static string GetTodayScheduleForGroup(string groupName)
            => ScheduleBaseAddress + $"/{groupName}" + "/today";
        public static string GetTomorrowScheduleForGroup(string groupName)
            => ScheduleBaseAddress + $"/{groupName}" + "/tomorrow";
        public static string GetFullScheduleForGroup(string groupName)
            => ScheduleBaseAddress + $"/{groupName}" + "/full_schedule";
    }
}
