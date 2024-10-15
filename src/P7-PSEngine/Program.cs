using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P7_PSEngine.API;
using P7_PSEngine.Data;
var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<TodoDb>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDbContext<TodoDb>(options => 
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Sorry, Your connectionstring is not found"));
});

var app = builder.Build();

app.MapProductEndpoints();


app.Run();

