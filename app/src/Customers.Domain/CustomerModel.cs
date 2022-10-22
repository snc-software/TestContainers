namespace Customers.Domain;

public class CustomerModel
{
    public int Id { get; set; }
    
    public Guid UniqueId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
}