using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampSecurity.MS_Auth.Domain
{
    public class Token
    {
        public Guid IdToken { get; set; }

        [JsonProperty("idUsuario")]
        public Guid IdUsuario { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; } = null!;
        [JsonProperty("refreshTokenExpiryTime")]
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}
