using Customer.Business.Persistence.Interfaces;
using Customers.Domain.Exceptions;

namespace Customer.Business.Services;

public class CustomerRetrieverService : ICustomerRetrieverService
{
    readonly IDbConnectionFactory _dbConnectionFactory;
    readonly IPersistenceCustomerRetriever _retriever;

    public CustomerRetrieverService(IDbConnectionFactory dbConnectionFactory,
        IPersistenceCustomerRetriever retriever)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _retriever = retriever;
    }
    public async Task<CustomerModel> GetByIdAsync(Guid id)
    {
        await using var connection = _dbConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        try
        {
            var retrieved = await _retriever.GetByIdAsync(id, connection);
            if (retrieved is null)
            {
                throw new NotFoundException($"The customer with id '{id}' was not found.");
            }
            await connection.CloseAsync();
            return retrieved;
        }
        catch
        {
            await ForceClose(connection);
            throw;
        }
    }

    public async Task<IEnumerable<CustomerModel>> GetAllAsync()
    {
        await using var connection = _dbConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        try
        {
            var retrieved = await _retriever.GetAllAsync(connection);
            await connection.CloseAsync();
            return retrieved;
        }
        catch
        {
            await ForceClose(connection);
            throw;
        }
    }

    async Task ForceClose(SqlConnection connection)
    {
        try
        {
            await connection.CloseAsync();
        }
        catch
        {
            //ignored
        }
    }
}