using P7_PSEngine.Controllers;
using P7_PSEngine.Model;

namespace P7_PSEngine.API
{
    public static class FrontendEndpointsExt
    {
        public static void MapFrontendEndpoints(this WebApplication app)
        {
            string static_path = "/app/wwwroot";

            //Frontpage
            app.MapGet("/", () => Results.Content(File.ReadAllText($"{static_path}/index.html"), "text/html"));

            app.MapPost("/frontend/search", (SearchDetailsDTO Details) =>
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

            app.MapGet("/frontend/commands", () =>
            {
                return FrontendController.SendCommands();
            });

            app.MapPost("/frontend/signin", async (User user, IUserRepository repo) =>
            {
                return await FrontendController.HandleLogin(user, repo);
            });

            app.MapPost("/frontend/signup", async (User user, IUserRepository repo) =>
            {
                return await FrontendController.HandleSignUp(user, repo);
            });

            app.MapPost("/frontend/verifysession", async (SessionCookieDTO cookie, IUserRepository repo) =>
            {
                return await FrontendController.VerifySession(cookie.username, cookie.session_cookie, repo);
            });

            app.MapGet("/frontend/{service}/key", (string service) =>
            {
                return FrontendController.GetServiceKey(service, app);
            });
        }
    }
}