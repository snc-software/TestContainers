using Customer.ServiceInterface.Requests;

namespace Customer.Business.Services.Interfaces;

public interface ICustomerUpdaterService
{
    Task<CustomerModel> UpdateAsync(Guid id, UpdateCustomerRequest request);
}