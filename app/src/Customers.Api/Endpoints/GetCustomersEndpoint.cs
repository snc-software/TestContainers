namespace Customers.Api.Endpoints;

public static class GetCustomersEndpoint
{
    public static WebApplication MapGetCustomersEndpoint(this WebApplication app)
    {
        app.MapGet("/customers", async (ICustomerRetrieverService service, IMapper mapper) =>
        {
            var customers = await service.GetAllAsync();
            
            return Results.Ok(mapper.Map<IEnumerable<Contracts.Customer>>(customers));
        });
        return app;
    }
}