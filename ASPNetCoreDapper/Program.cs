using ASPNetCoreDapper.Context;
using ASPNetCoreDapper.Contracts;
using ASPNetCoreDapper.Repository;
using ASPNetCoreDapper.Services;
using ASPNetCoreDapper.Validators;
using ASPNetCoreDapper.Filters;
using ASPNetCoreDapper.Entities;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddControllers(options => options.Filters
    .Add(typeof(ValidationFilter)));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add FluentValidation
builder.Services.AddScoped<IValidator<User>, UserValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


