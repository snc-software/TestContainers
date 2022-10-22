using Customer.Business.Persistence;
using Customer.Business.Persistence.Interfaces;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Customers.Api.IntegrationTests.Infrastructure;

public class CustomersWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{ 
    public readonly TestcontainerDatabase Container;
    public const string DbPassword = "P4ssw0rd";
    public const string DbName = "CustomersDb";

    public CustomersWebApplicationFactory()
    {
        Container = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Password = DbPassword,
            })
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IDbConnectionFactory));
            services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(
                $"Server=localhost,{Container.Port};Database={DbName};User=SA;Password={DbPassword};Encrypt=False"));
        });
    }

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await Container.StopAsync();
    }
}