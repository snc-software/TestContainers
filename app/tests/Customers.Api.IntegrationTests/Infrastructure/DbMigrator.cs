using DbUp;
using DbUp.Engine;

namespace Customers.Api.IntegrationTests.Infrastructure;

public class DbMigrator
{
    public void Migrate(int port, string dbPassword, string dbName)
    {
        var connectionString = $"Server=localhost,{port};Database={dbName};User=SA;Password={dbPassword};Encrypt=False";

        var upgrader =
            DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScript(new SqlScript("migrate.sql", @"CREATE TABLE dbo.Customers
        (
            Id INT NOT NULL IDENTITY(1,1),
            UniqueId UNIQUEIDENTIFIER NOT NULL,
            Name VARCHAR(100) NOT NULL,
            Email NVARCHAR(150) NULL,
            CONSTRAINT PK_Customers PRIMARY KEY CLUSTERED (Id)
        )"))
                .LogToConsole()
                .Build();
        
        EnsureDatabase.For.SqlDatabase(connectionString);
        upgrader.PerformUpgrade();
    }
}