namespace Customer.Business.PersistenceServices.Interfaces;

public interface IPersistenceCustomerCreator
{
    Task CreateAsync(CustomerModel model, SqlConnection connection, SqlTransaction transaction);
}