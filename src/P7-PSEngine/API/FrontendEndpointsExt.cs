namespace P7_PSEngine.API
{
    public static class FrontendEndpointsExt
    {
        public struct ServiceCreationDetails
        {
            public string searchwords { get; set; }
            public bool filenameOption { get; set; }
            public bool contentOption { get; set; }
            public bool mailOption { get; set; }
            public bool docOption { get; set; }
            public bool folderOption { get; set; }
            public bool imageOption { get; set; }
            public bool miscOption { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
        }

        public struct Command
        {
            public string Keyword { get; set; }
            public string Explanation { get; set; }
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

            app.MapPost("/frontend/search", (ServiceCreationDetails Details) =>
            {

                object[] response = [
                                    new {name = "Afkrydsning - SIMON", path = "/google/drive/", date = DateTime.Now},
                                    new {name = "Afkrydsning - KASPER", path = "/google/drive/", date = DateTime.Now},
                                    new {name = "Tilmelding til Aalborg Friskoles Musik- og Kulturskole (Efterår 2024) (svar)", path = "/google/drive/", date = DateTime.Now},
                                    new {name = "Lærer Kasper", path = "/google/drive/", date = DateTime.Now},
                                    new {name = "Afkrydsning - ANDREAS", path = "/google/drive/", date = DateTime.Now},
                                    new {name = "Afkrydsning - DAPHNE", path = "/google/drive/", date = DateTime.Now},
                                    new {name = "Every WFRP Adventure", path = "/google/drive/", date = DateTime.Now},
                                    new {name = "Tilmelding (Efterår 2024) (optælling) ENDELIG", path = "/google/drive/", date = DateTime.Now},
                                    new {name = "Simon – bruges ikke", path = "/google/drive/", date = DateTime.Now},
                                    new {name = "Undervisere Musik- og kulturskolen 2024-2025", path = "/google/drive/", date = DateTime.Now},
                                    new {name = "Ramme for undervisningen og forventninger til undervisere", path = "/google/drive/", date = DateTime.Now},
                                    new {name = "Mail til forældre - trommer", path = "/google/drive/", date = DateTime.Now},
                                    ];

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
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                    new Command("Test", "A command for testing whether this works or not"),
                                ];

            app.MapGet("/frontend/commands", () => { return Commands; });
        }
    }
}