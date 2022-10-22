using System.Net;
using System.Text;
using System.Text.Json;
using Customer.ServiceInterface.Requests;

namespace Customers.Api.IntegrationTests.EndpointTests;

public class CreateCustomerEndpointTests : IntegrationTestBase
{
    readonly HttpClient _client;
    readonly Contracts.Customer _customer;

    public CreateCustomerEndpointTests(CustomersWebApplicationFactory factory) : base(factory)
    {
        _client = factory.CreateDefaultClient();
        var faker = new CustomerContractFaker();
        _customer = faker.Generate();
    }

    [Fact]
    public async Task ACustomerCanBeCreatedAndRetrieved()
    {
        var createdResponse = await _client.CreateCustomer(_customer);
        createdResponse.IsSuccessStatusCode.Should().BeTrue();
        var created = await createdResponse.Content.ReadFromJsonAsync<Contracts.Customer>();
        created.Should().NotBeNull();
        created.Should().BeEquivalentTo(_customer, options => options.Excluding(p => p.Id));
        
        var response = await _client.GetAsync($"/customers/{created.Id}");
        response.IsSuccessStatusCode.Should().BeTrue();
        var retrieved = await response.Content.ReadFromJsonAsync<Contracts.Customer>();
        retrieved.Should().NotBeNull();
        retrieved.Should().BeEquivalentTo(created);
    }
    
    [Fact]
    public async Task ACustomerWithNoNameCannotBeCreated()
    {
        var request = new CreateCustomerRequest
        {
            Name = string.Empty,
            Email = _customer.Email
        };
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PostAsync("customers",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ACustomerWithATooLongNameCannotBeCreated()
    {
        var request = new CreateCustomerRequest
        {
            Name = new string('*', 101),
            Email = _customer.Email
        };
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PostAsync("customers",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ACustomerWithNoEmailCannotBeCreated()
    {
        var request = new CreateCustomerRequest
        {
            Name = _customer.Name,
            Email = string.Empty
        };
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PostAsync("customers",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ACustomerWithTooLongEmailCannotBeCreated()
    {
        var request = new CreateCustomerRequest
        {
            Name = _customer.Name,
            Email = new string('*', 150)
        };
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PostAsync("customers",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData("myEmail")]
    [InlineData("myEmail.com")]
    public async Task ACustomerWithIncorrectEmailCannotBeCreated(string email)
    {
        var request = new CreateCustomerRequest
        {
            Name = _customer.Name,
            Email = email
        };
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PostAsync("customers",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}