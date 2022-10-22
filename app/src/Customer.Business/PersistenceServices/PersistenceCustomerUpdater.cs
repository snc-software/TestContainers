namespace Customer.Business.PersistenceServices;

public class PersistenceCustomerUpdater : IPersistenceCustomerUpdater
{
    public async Task UpdateAsync(CustomerModel model, SqlConnection connection, SqlTransaction transaction)
    {
        // this uses Id over UniqueId as it will be faster using the PK here
        await connection.ExecuteAsync(
            "UPDATE dbo.Customers SET Name = @Name, Email = @Email WHERE Id = @Id", model,
            transaction: transaction);
    }
}