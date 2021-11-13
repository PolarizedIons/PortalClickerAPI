using System.ComponentModel.DataAnnotations;

namespace PortalClickerApi.Config
{
    public class ApiConfig
    {
        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string CorsHosts { get; set; }

        [Required]
        public JwtConfig Jwt { get; set; }
    }

    public class JwtConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
