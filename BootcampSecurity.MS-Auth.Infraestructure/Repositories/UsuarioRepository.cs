using BootcampSecurity.MS_Auth.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampSecurity.MS_Auth.Infraestructure
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDataAccessRepository _dataAccesRepository;
        private readonly IConfiguration _configuration;
        public UsuarioRepository(IDataAccessRepository dataAccesRepository, IConfiguration configuration)
        {

            _dataAccesRepository = dataAccesRepository;
            _configuration = configuration;

        }

        public async Task<Usuario> FindUser(Usuario usuario)
        {
            var result = await _dataAccesRepository.ExecuteFirst<Usuario>(_configuration["UserValidate"]!, usuario, CommandType.StoredProcedure);

            // Devolver el valor obtenido directamente
            return result;
        }


        
    }
}
