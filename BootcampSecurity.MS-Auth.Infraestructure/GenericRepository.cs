using BootcampSecurity.MS_Auth.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampSecurity.MS_Auth.Infrastructure
{
    public class GenericRepository: DataAccessRepository, IDataAccessRepository
    {
        public GenericRepository(IDbConnection connection) : base(connection) { }
        

        public async Task<IEnumerable<TOutput>> Execute<TOutput>(string storedProcedureKey, object obj, CommandType commandType)
        {
            return await Exec<object, TOutput>(storedProcedureKey, obj, commandType);
        }

        public async Task<TOutput> ExecuteFirst<TOutput>(string storedProcedureKey, object obj, CommandType commandType)
        {
            return await ExecFirst<object, TOutput>(storedProcedureKey, obj, commandType);
        }
    }
}
