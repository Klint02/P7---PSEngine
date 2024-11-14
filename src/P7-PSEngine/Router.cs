using CloudFileIndexer;
namespace Routing;

public class Router
{

    private IndexController _indexcontroller;

    public Router(WebApplication app)
    {
        string static_path = "/app/wwwroot";
        var invertedIndex = new InvertedIndex();
        _indexcontroller = new IndexController(invertedIndex);

        //Lets you send files from wwwroot folder
        app.UseStaticFiles();

        //examples on how you can send data or pages
        //Send html page from wwwroot folder
        app.MapGet("/", () => Results.Content(File.ReadAllText($"{static_path}/index.html"), "text/html"));

        //Send a JSON object
        app.MapGet("/api/test", () => new { hmm = "wow", bab = 12345 });

        //Send a string
        app.MapGet("/api/test2", () => "test2");

        //Use variables from URL
        app.MapGet("/api/repeat/{message}", (string message) => $"{message}");

        app.MapGet("/api/index", () => Results.Json(_indexcontroller.GetIndexData()));

        app.Run();
    }
}