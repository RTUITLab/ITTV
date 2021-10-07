namespace KinectTvV2.API.Configuration
{
    /// <summary>
    /// Options to configure JWT
    /// </summary>
    public class JWTOptions
    {
        public const string ConfigSectionName = "JWTConfiguration";

        /// <summary>
        /// Authority to use when making OpenIdConnect calls.
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// Single valid audience value for any received OpenIdConnect token
        /// </summary>
        public string Audience { get; set; }
    }
}