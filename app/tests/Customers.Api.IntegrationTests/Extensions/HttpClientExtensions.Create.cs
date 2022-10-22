using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Customer.ServiceInterface.Requests;

namespace Customers.Api.IntegrationTests.Extensions;

public static class HttpClientExtensions_Create
{
    public static async Task<HttpResponseMessage> CreateCustomer(this HttpClient client, Contracts.Customer customer)
    {
        var request = new CreateCustomerRequest
        {
            Name = customer.Name,
            Email = customer.Email
        };
        var json = JsonSerializer.Serialize(request);
        return await client.PostAsync("customers", new StringContent(json, Encoding.UTF8, "application/json"));
    }
}