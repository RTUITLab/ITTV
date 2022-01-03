namespace ITTV.API.Core.Configuration
{
    /// <summary>
    /// Options to configure JWT
    /// </summary>
    public class JWTOptions
    {
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