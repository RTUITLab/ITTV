namespace ITTV.WPF.Core.Services.ApiClient.Requests.GetNews
{
    public class ApiNewsItem
    {
        public ApiNewsItem()
        { }

        public ApiNewsItem(string title, 
            string content,
            string link, ApiImageItem[] photos)
        {
            Title = title;
            Content = content;
            Link = link;
            Photos = photos;
        }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public ApiImageItem[] Photos { get; set; }
    }
}