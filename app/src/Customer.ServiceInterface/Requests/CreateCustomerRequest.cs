namespace Customer.ServiceInterface.Requests;

public record CreateCustomerRequest
{
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
}