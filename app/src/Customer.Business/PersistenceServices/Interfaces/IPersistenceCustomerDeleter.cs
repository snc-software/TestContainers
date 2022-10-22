namespace Customer.Business.PersistenceServices.Interfaces;

public interface IPersistenceCustomerDeleter
{
    Task DeleteAsync(int id, SqlConnection connection, SqlTransaction transaction);
}