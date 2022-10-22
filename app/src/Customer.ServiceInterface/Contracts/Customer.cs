namespace Customer.ServiceInterface.Contracts;

public record Customer
{
    public Guid Id { get; init; } 
    public string Name { get; init; }
    public string Email { get; init; }
}