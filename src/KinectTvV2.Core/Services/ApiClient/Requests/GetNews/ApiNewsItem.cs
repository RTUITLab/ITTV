namespace KinectTvV2.Core.Services.ApiClient.Requests.GetNews
{
    public class ApiNewsItem
    {
        public ApiNewsItem()
        { }

        public ApiNewsItem(string title, 
            string content,
            string link)
        {
            Title = title;
            Content = content;
            Link = link;
        }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
    }
}