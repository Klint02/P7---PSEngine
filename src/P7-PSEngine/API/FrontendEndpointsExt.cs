using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using P7_PSEngine.Data;
using P7_PSEngine.Model;

namespace P7_PSEngine.API
{
    public static class FrontendEndpointsExt
    {
        public struct ServiceCreationDetails 
        {
            public string searchwords {get; set;}
            public bool filenameOption {get; set;}
            public bool contentOption {get; set;}
            public bool mailOption {get; set;}
            public bool docOption {get; set;}
            public bool folderOption {get; set;}
            public bool imageOption {get; set;}
            public bool miscOption {get; set;}
            public DateTime? startDate {get; set;}
            public DateTime? endDate {get; set;}
        }

        public struct Command
        {
            public string Keyword {get; set;}
            public string Explanation {get; set;}
            public Command(string keyword, string explanation)
            {
                Keyword = keyword;
                Explanation = explanation;
            }
        }
        public static void MapFrontendEndpoints(this WebApplication app)
        {
            string static_path = "/app/wwwroot";

            app.MapGet("/linkuser", () => Results.Content(File.ReadAllText($"{static_path}/html/auth.html"), "text/html"));
            
            app.MapPost("/frontend/search", (ServiceCreationDetails Details) => {
                
                object[] response = [
                                    new {name = "test", path = "/test/", date = DateTime.Now}, 
                                    new {name = "test2", path = "/test2/", date = DateTime.Now}];
                
                return response;
            });
            
            Command[] Commands = [
                                    new Command("Contains:", "Search contains a certain keyword"), 
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                ];

            app.MapGet("/frontend/commands", () => { return Commands;});
        }
    }
}