namespace ITTV.WPF.Core.Helpers
{
    public static class LocalCacheHelper
    {
        public const string NewsCacheKey = "news-cache";
        public const string GroupsCacheKey = "groups-cache";
        public static string GroupScheduleCacheKey(string groupName)
            => $"group-{groupName}-schedule-cache";
    }
}