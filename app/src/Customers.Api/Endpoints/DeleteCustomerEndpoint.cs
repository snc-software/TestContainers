namespace Customers.Api.Endpoints;

public static class DeleteCustomerEndpoint
{
    public static WebApplication MapDeleteCustomerEndpoint(this WebApplication app)
    {
        app.MapDelete("/customers/{id:guid}", async (Guid id, ICustomerDeleterService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });
        return app;
    }
}