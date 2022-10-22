namespace Customers.Api.Endpoints;

public static class EndpointMapper
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapGetCustomerByIdEndpoint()
            .MapGetCustomersEndpoint()
            .MapCreateCustomerEndpoint()
            .MapUpdateCustomerEndpoint()
            .MapDeleteCustomerEndpoint();

        return app;
    }
}