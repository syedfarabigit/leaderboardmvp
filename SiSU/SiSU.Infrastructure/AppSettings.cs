
namespace SiSU.Infrastructure
{
    public class AppSettings
    {
        public string JwtSecretKey { get; set; }
        public string JwtTokenIssuer { get; set; }
        public string JwtAudience { get; set; }
    }
}
