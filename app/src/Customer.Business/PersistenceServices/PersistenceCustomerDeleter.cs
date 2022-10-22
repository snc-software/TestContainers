namespace Customer.Business.PersistenceServices;

public class PersistenceCustomerDeleter : IPersistenceCustomerDeleter
{
    public async Task DeleteAsync(int id, SqlConnection connection, SqlTransaction transaction)
    {
        await connection.ExecuteAsync(
            "DELETE FROM dbo.Customers WHERE Id = @Id", new { Id = id}, transaction: transaction);
    }
}