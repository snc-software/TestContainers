using Customer.Business.Persistence.Interfaces;
using Customers.Domain.Exceptions;

namespace Customer.Business.Services;

public class CustomerDeleterService : ICustomerDeleterService
{
    readonly IDbConnectionFactory _dbConnectionFactory;
    readonly IPersistenceCustomerDeleter _deleter;
    readonly IPersistenceCustomerRetriever _retriever;

    public CustomerDeleterService(
        IDbConnectionFactory dbConnectionFactory, 
        IPersistenceCustomerDeleter deleter,
        IPersistenceCustomerRetriever retriever)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _deleter = deleter;
        _retriever = retriever;
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var connection = _dbConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var transaction = connection.BeginTransaction();
        try
        {
            var existing = await _retriever.GetByIdAsync(id, connection, transaction);
            if (existing is null)
            {
                throw new NotFoundException($"The customer with id '{id}' was not found.");
            }

            await _deleter.DeleteAsync(existing.Id, connection, transaction);
            await transaction.CommitAsync();
            await connection.CloseAsync();
        }
        catch
        {
            await ForceRollbackAndCloseConnection(transaction, connection);
            throw;
        }
    }
    
    async Task ForceRollbackAndCloseConnection(SqlTransaction transaction, SqlConnection connection)
    {
        try
        {
            await transaction.RollbackAsync();
            await connection.CloseAsync();
        }
        catch
        {
            // ignored
        }
    }
}