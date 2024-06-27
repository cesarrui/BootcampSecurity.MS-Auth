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
    public class TokenRepository : ITokenRepository
    {
        private readonly IDataAccessRepository _dataAccesRepository;
        private readonly IConfiguration _configuration;
        public TokenRepository(IDataAccessRepository dataAccesRepository, IConfiguration configuration)
        {

            _dataAccesRepository = dataAccesRepository;
            _configuration = configuration;

        }
        public async Task CreateOrUpdate(Token token)
        {
     
                await _dataAccesRepository.ExecuteFirst<Token>(_configuration["CreateOrUpdate"]!, token, CommandType.StoredProcedure);
            
            
        }

        public async Task<Token?> Get(Token token)
        {
            
            var data = await _dataAccesRepository.ExecuteFirst<Token>(_configuration["SearchToken"]!, token, CommandType.StoredProcedure);
           
            return data;
        }

        public async Task update(Token token)
        {

            await _dataAccesRepository.ExecuteFirst<Token>(_configuration["UpdateToken"]!, token, CommandType.StoredProcedure);
        }
    }
}
