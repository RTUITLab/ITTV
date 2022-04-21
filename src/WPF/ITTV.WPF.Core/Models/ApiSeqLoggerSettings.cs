namespace ITTV.WPF.Core.Models
{
    public class ApiSeqLoggerSettings
    {
        public string Uri { get; set; }
        public string ApiKey { get; set; }

        
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Uri))
                return false;
            if (string.IsNullOrWhiteSpace(ApiKey))
                return false;

            return true;
        }
    }
}