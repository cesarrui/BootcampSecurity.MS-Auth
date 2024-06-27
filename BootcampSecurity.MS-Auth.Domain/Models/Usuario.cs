using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampSecurity.MS_Auth.Domain 
{ 
    public class Usuario
    {

        [JsonProperty("idUsuario")]
        public Guid idUsuario { get; set; } 
        [JsonProperty("usuario")]
        public string usuario { get; set; } = string.Empty;

        [JsonProperty("clave")]
        public string Clave { get; set; } = string.Empty;
    }
}

