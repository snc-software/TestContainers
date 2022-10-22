using System.Net;
using Customer.ServiceInterface.Responses;

namespace Customers.Api.IntegrationTests.EndpointTests;

public class DeleteCustomerEndpointTests : IntegrationTestBase
{
    readonly HttpClient _client;
    readonly CustomerContractFaker _faker;
    
    public DeleteCustomerEndpointTests(CustomersWebApplicationFactory factory) : base(factory)
    {
        _client = factory.CreateDefaultClient();
        _faker = new CustomerContractFaker();
    }

    [Fact]
    public async Task AnExistingCustomerCanBeDeleted()
    {
        var customer = _faker.Generate();
        var createdResponse = await _client.CreateCustomer(customer);
        createdResponse.IsSuccessStatusCode.Should().BeTrue();
        var existing = await createdResponse.Content.ReadFromJsonAsync<Contracts.Customer>();

        var response = await _client.DeleteAsync($"customers/{existing.Id}");
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var retrieved = await _client.GetAsync($"customers/{existing.Id}");
        retrieved.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ACustomerThatDoesntExistCannotBeDeleted()
    {
        var id = Guid.NewGuid();
     
        var response = await _client.DeleteAsync($"customers/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var exception = await response.Content.ReadFromJsonAsync<ExceptionResponse>();
        exception.Should().NotBeNull();
        exception.Message.Should().Be($"The customer with id '{id}' was not found.");
    }
}