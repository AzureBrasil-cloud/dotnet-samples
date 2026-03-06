using OpenApi.WebApi.Extensions;
using OpenApi.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureCustomMiddlewareAndEndpoints();

// add custom services
builder.Services.AddScoped<BookService>();

var app = builder.Build();
app.UseCustomMiddlewareAndEndpoints();
app.Run();