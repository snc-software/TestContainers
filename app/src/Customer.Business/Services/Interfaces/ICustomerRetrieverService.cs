namespace Customer.Business.Services.Interfaces;

public interface ICustomerRetrieverService
{
    Task<CustomerModel> GetByIdAsync(Guid id);

    Task<IEnumerable<CustomerModel>> GetAllAsync();
}