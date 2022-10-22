using Customer.Business.Persistence.Interfaces;

namespace Customer.Business.Persistence;

public class DbConnectionFactory : IDbConnectionFactory
{
    readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}