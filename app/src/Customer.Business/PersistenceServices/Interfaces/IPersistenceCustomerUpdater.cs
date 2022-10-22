namespace Customer.Business.PersistenceServices.Interfaces;

public interface IPersistenceCustomerUpdater
{
    Task UpdateAsync(CustomerModel model, SqlConnection connection, SqlTransaction transaction);
}