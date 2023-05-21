using Microsoft.Data.SqlClient;
using System.Data;

namespace TimeChimp.Backend.Assessment.Repositories
{
    public interface IContext
    {
        IDbConnection CreateConnection();
    }
}
