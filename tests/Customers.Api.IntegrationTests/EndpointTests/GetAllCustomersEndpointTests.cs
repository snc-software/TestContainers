using System.Net.Http.Json;
using Customers.Api.IntegrationTests.Extensions;
using Customers.Api.IntegrationTests.Infrastructure;
using Customers.TestData;
using FluentAssertions;

namespace Customers.Api.IntegrationTests.EndpointTests;

public class GetAllCustomersEndpointTests : IntegrationTestBase
{
    readonly HttpClient _client;
    readonly CustomerContractFaker _faker;
    readonly IEnumerable<Contracts.Customer> _customers;
    readonly List<Contracts.Customer> _createdCustomers;

    public GetAllCustomersEndpointTests(CustomersWebApplicationFactory factory) : base(factory)
    {
        _client = factory.CreateDefaultClient();

        _faker = new CustomerContractFaker();
        var toGenerate = Random.Shared.Next(5, 10);
        _customers = _faker.Generate(toGenerate);
        _createdCustomers = new List<Contracts.Customer>();
    }

    [Fact]
    public async Task GetAllReturnsAllCustomers()
    {
        await CreateCustomers();

        var response = await _client.GetAsync("customers");

        var customers = await response.Content.ReadFromJsonAsync<IEnumerable<Contracts.Customer>>();

        customers.Should().BeEquivalentTo(_customers, 
            opt => opt.Excluding(p => p.Id));
    }
    
    #region Supporting Code

    async Task CreateCustomers()
    {
        foreach (var customer in _customers)
        {
            var created = await _client.CreateCustomer(customer);
            _createdCustomers.Add(created);
        }
    }
    
    #endregion
}