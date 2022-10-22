using Customer.Business.Persistence;
using Customer.Business.Persistence.Interfaces;
using Customer.Business.PersistenceServices;
using Customer.Business.PersistenceServices.Interfaces;
using Customer.Business.Services;
using Customer.ServiceInterface.Mappings;
using Customer.ServiceInterface.Validation;
using Customers.Api.Endpoints;
using Customers.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("CustomerDb");
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
builder.Services.AddScoped<IPersistenceCustomerRetriever, PersistenceCustomerRetriever>();
builder.Services.AddScoped<IPersistenceCustomerCreator, PersistenceCustomerCreator>();
builder.Services.AddScoped<IPersistenceCustomerUpdater, PersistenceCustomerUpdater>();
builder.Services.AddScoped<IPersistenceCustomerDeleter, PersistenceCustomerDeleter>();


builder.Services.AddScoped<ICustomerRetrieverService, CustomerRetrieverService>();
builder.Services.AddScoped<ICustomerCreatorService, CustomerCreatorService>();
builder.Services.AddScoped<ICustomerUpdaterService, CustomerUpdaterService>();
builder.Services.AddScoped<ICustomerDeleterService, CustomerDeleterService>();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<CustomersProfile>());
builder.Services.AddValidatorsFromAssembly(typeof(CreateCustomerRequestValidator).Assembly);
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapEndpoints();

app.Run();

public partial class Program { }