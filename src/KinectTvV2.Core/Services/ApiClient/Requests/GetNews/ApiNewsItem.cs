namespace KinectTvV2.Core.Services.ApiClient.Requests.GetNews
{
    public class ApiNewsItem
    {
        public ApiNewsItem()
        { }

        public ApiNewsItem(string title, string content)
        {
            Title = title;
            Content = content;
        }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}