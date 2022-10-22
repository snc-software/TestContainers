namespace Customer.Business.Services.Interfaces;

public interface ICustomerDeleterService
{
    Task DeleteAsync(Guid id);
}