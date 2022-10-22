namespace Customers.Api.Endpoints;

public static class UpdateCustomerEndpoint
{
    public static WebApplication MapUpdateCustomerEndpoint(this WebApplication app)
    {
        app.MapPut("/customers/{id}", async (
            Guid id,
            [FromBody]UpdateCustomerRequest request,
            ICustomerUpdaterService service,
            IMapper mapper,
            IValidator<UpdateCustomerRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) 
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var customer = await service.UpdateAsync(id, request);
            
            return Results.Ok(mapper.Map<Contracts.Customer>(customer));
        });
        return app;
    }
}