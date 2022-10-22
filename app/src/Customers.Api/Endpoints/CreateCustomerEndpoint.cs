using Customers.Domain;

namespace Customers.Api.Endpoints;

public static class CreateCustomerEndpoint
{
    public static WebApplication MapCreateCustomerEndpoint(this WebApplication app)
    {
        app.MapPost("/customers", async (
            [FromBody]CreateCustomerRequest request,
            ICustomerCreatorService service,
            IMapper mapper,
            IValidator<CreateCustomerRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) 
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var customer = await service.CreateAsync(request);
            
            return Results.CreatedAtRoute("GetCustomerById", new {id = customer.UniqueId} ,mapper.Map<Contracts.Customer>(customer));
        });
        return app;
    }
}