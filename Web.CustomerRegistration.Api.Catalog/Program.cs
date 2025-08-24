using Core.CustomerRegistration.Application.Customers;
using Core.CustomerRegistration.Domain.Repositories;
using Infrastructure.CustomerRegistration.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CustomerRegistrationDbContext>(opt =>
    opt.UseOracle(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection(); 
}

app.UseAuthorization();

app.MapControllers();

app.Run();
