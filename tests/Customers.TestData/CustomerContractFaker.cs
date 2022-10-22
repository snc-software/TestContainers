using Bogus;
using Contracts = Customer.ServiceInterface.Contracts; 

namespace Customers.TestData;

public sealed class CustomerContractFaker : Faker<Contracts.Customer>
{
    public CustomerContractFaker()
    {
        RuleFor(p => p.Name, v => v.Person.FullName);
        RuleFor(p => p.Email, v => v.Person.Email);
    }
}