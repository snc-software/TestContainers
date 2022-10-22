using System.Net;
using Customer.ServiceInterface.Responses;

namespace Customers.Api.IntegrationTests.EndpointTests;

public class GetCustomerByIdEndpointTests : IntegrationTestBase
{
    readonly HttpClient _client;
    readonly Contracts.Customer _customer;
    
    public GetCustomerByIdEndpointTests(CustomersWebApplicationFactory factory) : base(factory)
    {
        _client = factory.CreateDefaultClient();
        var faker = new CustomerContractFaker();
        _customer = faker.Generate();
    }

    [Fact]
    public async Task GetByIdReturnsTheExpectedCustomer()
    {
        var createdResponse = await _client.CreateCustomer(_customer);
        createdResponse.IsSuccessStatusCode.Should().BeTrue();
        var created = await createdResponse.Content.ReadFromJsonAsync<Contracts.Customer>();

        var response = await _client.GetAsync($"/customers/{created.Id}");

        response.IsSuccessStatusCode.Should().BeTrue();
        var retrieved = await response.Content.ReadFromJsonAsync<Contracts.Customer>();
        retrieved.Should().NotBeNull();
        retrieved.Should().BeEquivalentTo(created);
    }

    [Fact]
    public async Task GetByIdReturnsNotFoundWhenCustomerDoesNotExist()
    {
        var id = Guid.NewGuid();
        
        var response = await _client.GetAsync($"/customers/{id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var retrieved = await response.Content.ReadFromJsonAsync<ExceptionResponse>();
        retrieved.Should().NotBeNull();
        retrieved.Message.Should().Be($"The customer with id '{id}' was not found.");

    }
}