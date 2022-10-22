using Customers.Api.IntegrationTests.Infrastructure;

namespace Customers.Api.IntegrationTests;

public class IntegrationTestBase : IClassFixture<CustomersWebApplicationFactory>
{
    readonly CustomersWebApplicationFactory Factory;

    protected IntegrationTestBase(CustomersWebApplicationFactory factory)
    {
        Factory = factory;
        var migrator = new DbMigrator();
        migrator.Migrate(factory.Container.Port, CustomersWebApplicationFactory.DbPassword, CustomersWebApplicationFactory.DbName);
    }
}