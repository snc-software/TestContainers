namespace Customer.Business.PersistenceServices;

public class PersistenceCustomerRetriever : IPersistenceCustomerRetriever
{
    public async Task<CustomerModel> GetByIdAsync(Guid id, SqlConnection connection, SqlTransaction? transaction = null)
    {
        return await connection.QuerySingleOrDefaultAsync<CustomerModel>("SELECT Id, UniqueId, Name, Email FROM dbo.Customers WHERE UniqueId = @Id",
            new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<CustomerModel>> GetAllAsync(SqlConnection connection, SqlTransaction? transaction = null)
    {
        return await connection.QueryAsync<CustomerModel>("SELECT Id, UniqueId, Name, Email FROM dbo.Customers", transaction: transaction);
    }
}