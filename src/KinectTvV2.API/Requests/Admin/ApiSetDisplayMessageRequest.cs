namespace KinectTvV2.API.Requests.Admin
{
    public class ApiSetDisplayMessageRequest
    {
        public ApiSetDisplayMessageRequest()
        { }

        public ApiSetDisplayMessageRequest(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}