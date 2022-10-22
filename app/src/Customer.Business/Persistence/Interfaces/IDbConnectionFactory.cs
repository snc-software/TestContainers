namespace Customer.Business.Persistence.Interfaces;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
}