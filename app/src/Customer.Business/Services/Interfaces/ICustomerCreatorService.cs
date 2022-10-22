using Customer.ServiceInterface.Requests;

namespace Customer.Business.Services.Interfaces;

public interface ICustomerCreatorService
{
    Task<CustomerModel> CreateAsync(CreateCustomerRequest request);
}