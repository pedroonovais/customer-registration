using Core.CustomerRegistration.Application.Customers;
using Core.CustomerRegistration.Domain.Repositories;
using Infrastructure.CustomerRegistration.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen();

// EF Core
builder.Services.AddDbContext<CustomerRegistrationDbContext>(opt =>
    opt.UseOracle(builder.Configuration.GetConnectionString("Default")));

// DI
builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();