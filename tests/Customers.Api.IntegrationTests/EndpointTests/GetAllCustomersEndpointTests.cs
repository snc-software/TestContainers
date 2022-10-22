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
            var createdResponse = await _client.CreateCustomer(customer);
            createdResponse.IsSuccessStatusCode.Should().BeTrue();
            var created = await createdResponse.Content.ReadFromJsonAsync<Contracts.Customer>();
            _createdCustomers.Add(created);
        }
    }
    
    #endregion
}