using AutoMapper;
using Customer.Business.Persistence.Interfaces;
using Customer.ServiceInterface.Requests;
using Customers.Domain.Exceptions;

namespace Customer.Business.Services;

public class CustomerUpdaterService : ICustomerUpdaterService
{
    readonly IDbConnectionFactory _dbConnectionFactory;
    readonly IPersistenceCustomerUpdater _updater;
    readonly IPersistenceCustomerRetriever _retriever;
    readonly IMapper _mapper;

    public CustomerUpdaterService(IDbConnectionFactory dbConnectionFactory, 
        IPersistenceCustomerUpdater updater,
        IPersistenceCustomerRetriever retriever, IMapper mapper)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _updater = updater;
        _retriever = retriever;
        _mapper = mapper;
    }

    public async Task<CustomerModel> UpdateAsync(Guid id, UpdateCustomerRequest request)
    {
        await using var connection = _dbConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var transaction = connection.BeginTransaction();
        try
        {
            var model = await _retriever.GetByIdAsync(id, connection, transaction);
            if (model is null)
            {
                throw new NotFoundException($"The customer with id '{id}' was not found.");
            }

            var toUpdate = _mapper.Map<CustomerModel>(request);
            toUpdate.Id = model.Id;
            toUpdate.UniqueId = model.UniqueId;
            
            await _updater.UpdateAsync(toUpdate, connection, transaction);
            await transaction.CommitAsync();
            await connection.CloseAsync();
            return toUpdate;
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