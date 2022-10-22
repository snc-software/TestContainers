using AutoMapper;
using Customer.Business.Persistence.Interfaces;
using Customer.ServiceInterface.Requests;

namespace Customer.Business.Services;

public class CustomerCreatorService : ICustomerCreatorService
{
    readonly IDbConnectionFactory _dbConnectionFactory;
    readonly IPersistenceCustomerCreator _creator;
    readonly IPersistenceCustomerRetriever _retriever;
    readonly IMapper _mapper;

    public CustomerCreatorService(IDbConnectionFactory dbConnectionFactory,
        IPersistenceCustomerCreator creator, 
        IPersistenceCustomerRetriever retriever,
        IMapper mapper)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _creator = creator;
        _retriever = retriever;
        _mapper = mapper;
    }

    public async Task<CustomerModel> CreateAsync(CreateCustomerRequest request)
    {
        var model = _mapper.Map<CustomerModel>(request);
        
        await using var connection = _dbConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var transaction = connection.BeginTransaction();
        try
        {
            model.UniqueId = Guid.NewGuid();
            await _creator.CreateAsync(model, connection, transaction);
            var retrieved = await _retriever.GetByIdAsync(model.UniqueId, connection, transaction);
            await transaction.CommitAsync();
            await connection.CloseAsync();
            return retrieved;
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