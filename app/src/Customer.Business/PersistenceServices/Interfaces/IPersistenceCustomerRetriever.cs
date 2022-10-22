namespace Customer.Business.PersistenceServices.Interfaces;

public interface IPersistenceCustomerRetriever
{
    Task<CustomerModel> GetByIdAsync(Guid id, SqlConnection connection, SqlTransaction? transaction = null);

    Task<IEnumerable<CustomerModel>> GetAllAsync(SqlConnection connection, SqlTransaction? transaction = null);
}