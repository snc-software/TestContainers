using System.Net;
using System.Text;
using System.Text.Json;
using Customer.ServiceInterface.Requests;
using Customer.ServiceInterface.Responses;

namespace Customers.Api.IntegrationTests.EndpointTests;

public class UpdateCustomerEndpointTests : IntegrationTestBase
{
    readonly HttpClient _client;
    readonly Contracts.Customer _customer;
    
    public UpdateCustomerEndpointTests(CustomersWebApplicationFactory factory) : base(factory)
    {
        _client = factory.CreateDefaultClient();
        var faker = new CustomerContractFaker();
        _customer = faker.Generate();
    }

    [Fact]
    public async Task AnExistingCustomerCanBeUpdated()
    {
        var createdResponse = await _client.CreateCustomer(_customer);
        createdResponse.IsSuccessStatusCode.Should().BeTrue();
        var existing = await createdResponse.Content.ReadFromJsonAsync<Contracts.Customer>();
        
        var request = new UpdateCustomerRequest
        {
            Name = "Updated",
            Email = "Updated@email.com"
        };
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PutAsync($"customers/{existing.Id}",
            new StringContent(json, Encoding.UTF8, "application/json"));
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var updated = await response.Content.ReadFromJsonAsync<Contracts.Customer>();
        updated.Name.Should().Be(request.Name);
        updated.Email.Should().Be(request.Email);
        updated.Id.Should().Be(existing.Id);
    }
    
    [Fact]
    public async Task ACustomerThatDoesntExistCannotBeUpdated()
    {
        var id = Guid.NewGuid();
        var request = new UpdateCustomerRequest
        {
            Name = "Updated",
            Email = "Updated@email.com"
        };
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PutAsync($"customers/{id}",
            new StringContent(json, Encoding.UTF8, "application/json"));
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var exception = await response.Content.ReadFromJsonAsync<ExceptionResponse>();
        exception.Should().NotBeNull();
        exception.Message.Should().Be($"The customer with id '{id}' was not found.");
    }
    
    [Fact]
    public async Task ACustomerCannotBeUpdatedToHaveNoName()
    {
        var createdResponse = await _client.CreateCustomer(_customer);
        createdResponse.IsSuccessStatusCode.Should().BeTrue();
        var existing = await createdResponse.Content.ReadFromJsonAsync<Contracts.Customer>();
        
        var request = new UpdateCustomerRequest
        {
            Name = string.Empty,
            Email = _customer.Email
        };
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PutAsync($"customers/{existing.Id}",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ACustomerCannotBeUpdatedToHaveATooLongName()
    {
        var createdResponse = await _client.CreateCustomer(_customer);
        createdResponse.IsSuccessStatusCode.Should().BeTrue();
        var existing = await createdResponse.Content.ReadFromJsonAsync<Contracts.Customer>();
        
        var request = new UpdateCustomerRequest
        {
            Name = new string('*', 101),
            Email = _customer.Email
        };
        
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PutAsync($"customers/{existing.Id}",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ACustomerCannotBeUpdatedToHaveNoEmail()
    {
        var createdResponse = await _client.CreateCustomer(_customer);
        createdResponse.IsSuccessStatusCode.Should().BeTrue();
        var existing = await createdResponse.Content.ReadFromJsonAsync<Contracts.Customer>();
        
        var request = new UpdateCustomerRequest
        {
            Name = _customer.Name,
            Email = string.Empty
        };
        
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PutAsync($"customers/{existing.Id}",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ACustomerCannotBeUpdatedToHaveATooLongEmail()
    {
        var createdResponse = await _client.CreateCustomer(_customer);
        createdResponse.IsSuccessStatusCode.Should().BeTrue();
        var existing = await createdResponse.Content.ReadFromJsonAsync<Contracts.Customer>();
        
        var request = new UpdateCustomerRequest
        {
            Name = _customer.Name,
            Email = new('*', 151)
        };
        
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PutAsync($"customers/{existing.Id}",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData("myEmail")]
    [InlineData("myEmail.com")]
    public async Task ACustomerCannotBeUpdatedToHaveAnInvalidEmail(string email)
    {
        var createdResponse = await _client.CreateCustomer(_customer);
        createdResponse.IsSuccessStatusCode.Should().BeTrue();
        var existing = await createdResponse.Content.ReadFromJsonAsync<Contracts.Customer>();
        
        var request = new UpdateCustomerRequest
        {
            Name = _customer.Name,
            Email = email
        };
        
        var json = JsonSerializer.Serialize(request);
        var response = await _client.PutAsync($"customers/{existing.Id}",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}