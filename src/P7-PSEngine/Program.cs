using CloudFileIndexer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P7_PSEngine.API;
using P7_PSEngine.Data;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PSengineDB>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddSingleton<InvertedIndex>();
builder.Services.AddSingleton<IndexService>();
builder.Services.AddScoped<IndexController>();
//builder.Services.AddScoped<SearchController>();
builder.Services.AddSingleton<SearchController>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

var indexInitializer = app.Services.GetRequiredService<IndexService>();
indexInitializer.Initialize();

app.MapProductEndpoints();

app.UseStaticFiles();

app.MapFrontendEndpoints();

app.MapServicesEndpoints();

app.Run();


