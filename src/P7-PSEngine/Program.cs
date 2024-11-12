using CloudFileIndexer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P7_PSEngine.API;
using P7_PSEngine.Data;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TodoDb>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddSingleton<InvertedIndex>();
builder.Services.AddSingleton<IndexService>();
builder.Services.AddScoped<IndexController>();
//builder.Services.AddScoped<SearchController>();
builder.Services.AddSingleton<SearchController>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

var indexInitializer = app.Services.GetRequiredService<IndexService>();
indexInitializer.Initialize();

app.MapProductEndpoints();
app.MapFrontendEndpoints();


app.Run();


