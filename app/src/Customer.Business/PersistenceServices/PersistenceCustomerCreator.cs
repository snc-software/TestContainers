namespace Customer.Business.PersistenceServices;

public class PersistenceCustomerCreator : IPersistenceCustomerCreator
{
    public async Task CreateAsync(CustomerModel model, SqlConnection connection, SqlTransaction transaction)
    {
        await connection.ExecuteAsync(
            "INSERT INTO dbo.Customers (UniqueId, Name, Email) VALUES (@UniqueId, @Name, @Email)", model,
            transaction: transaction);
    }
}