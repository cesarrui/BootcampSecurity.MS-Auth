using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampSecurity.MS_Auth.Domain
{
    public interface ITokenRepository
    {
        Task CreateOrUpdate(Token token);
        Task<Token?> Get(Token token);
        Task update(Token token);
    }
}
