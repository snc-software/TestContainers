using Bogus;
using Customers.Domain;

namespace Customers.TestData;

public sealed class CustomerModelFaker : Faker<CustomerModel>
{
    public CustomerModelFaker()
    {
        RuleFor(p => p.Name, v => v.Person.FullName);
        RuleFor(p => p.Email, v => v.Person.Email);
    }
}