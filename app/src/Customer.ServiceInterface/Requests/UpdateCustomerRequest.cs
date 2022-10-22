namespace Customer.ServiceInterface.Requests;

public record UpdateCustomerRequest
{
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
}