using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P7_PSEngine.API;
using P7_PSEngine.Data;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PSengineDB>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.MapFrontendEndpoints();

app.MapServicesEndpoints();

app.Run();


