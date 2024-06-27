using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampSecurity.MS_Auth.Domain
{
    public interface IUsuarioRepository
    {
        Task<Usuario> FindUser(Usuario usuario);


    }
}
