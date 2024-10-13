using P7_PSEngine.Data;
using System.Security.Cryptography.X509Certificates;

namespace Routing;
public class Router
{
    private readonly TodoDb _db;
    public Router(WebApplication app, TodoDb db) 
    {
        this._db = db;

        string static_path = "/app/wwwroot";

        //Lets you send files from wwwroot folder
        app.UseStaticFiles();

        //examples on how you can send data or pages
        //Send html page from wwwroot folder
        app.MapGet("/", () => Results.Content(File.ReadAllText($"{static_path}/index.html"), "text/html"));

        //Send a JSON object
        app.MapGet("/api/test", () => new {hmm = "wow", bab = 12345});

        //Send a string
        app.MapGet("/api/test2", () => "test2");

        //Use variables from URL
        app.MapGet("/api/repeat/{message}", (string message) => $"{message}");

        app.Run();
    }
}