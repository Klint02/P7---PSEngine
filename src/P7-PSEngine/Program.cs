using Microsoft.Extensions.Configuration;
using CloudFileIndexer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P7_PSEngine.API;
using P7_PSEngine.Data;
using P7_PSEngine.Handlers;
using P7_PSEngine.Model;

using P7_PSEngine.Repositories;
using P7_PSEngine.Services;
var builder = WebApplication.CreateBuilder(args);

var env_file = Path.Combine(Directory.GetCurrentDirectory(), "env");
if (File.Exists(env_file))
{
    builder.Configuration.AddIniFile(env_file);
}

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PSengineDB>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
//builder.Services.AddMvcCore().AddNewtonsoftJson(options =>
//{
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
//    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
//});
builder.Services.AddSingleton<InvertedIndex>();
builder.Services.AddSingleton<IndexService>();
builder.Services.AddScoped<IndexController>();
builder.Services.AddScoped<SearchController>();
builder.Services.AddSingleton<SearchController>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFileInformationRepository, FileInformationRepository>();
builder.Services.AddScoped<ICloudServiceRepository, CloudServiceRepository>();
builder.Services.AddTransient<IInvertedIndexService, InvertedIndexService>();
builder.Services.AddTransient<ISearchService, SearchService>();
builder.Services.AddTransient<IInvertedIndexRepository, InvertedIndexRepository>();


var app = builder.Build();

//var indexInitializer = app.Services.GetRequiredService<IndexService>();
//indexInitializer.Initialize();

app.MapProductEndpoints();

app.UseStaticFiles();

app.MapFrontendEndpoints();

app.MapServicesEndpoints();

app.Run();



