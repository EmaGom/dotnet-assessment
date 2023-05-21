using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace TimeChimp.Backend.Assessment.Repositories
{
    public class ContextDapper : IContext
    {
        private ConnectionStringOptions connectionStringOptions;

        public ContextDapper(IOptionsMonitor<ConnectionStringOptions> optionsMonitor)
        {
            connectionStringOptions = optionsMonitor.CurrentValue;
        }
        public IDbConnection CreateConnection() => new SqlConnection(connectionStringOptions.SqlConnection);
    }
}
