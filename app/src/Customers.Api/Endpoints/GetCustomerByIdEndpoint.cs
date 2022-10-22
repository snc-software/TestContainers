namespace Customers.Api.Endpoints;

public static class GetCustomerByIdEndpoint
{
    public static WebApplication MapGetCustomerByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/customers/{id:guid}", async (Guid id, ICustomerRetrieverService service, IMapper mapper) =>
        {
            var customer = await service.GetByIdAsync(id);
            
            return Results.Ok(mapper.Map<Contracts.Customer>(customer));
        }).WithName("GetCustomerById");
        return app;
    }
}