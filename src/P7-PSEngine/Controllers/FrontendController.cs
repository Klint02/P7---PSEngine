using P7_PSEngine.API;
using P7_PSEngine.DTO;
using P7_PSEngine.Model;
using System.Security.Cryptography;
using System.Text;

namespace P7_PSEngine.Controllers;

public static class FrontendController
{

    private static string HashData(string string_to_hash)
    {
        byte[] hash_bytes = Encoding.UTF8.GetBytes(string_to_hash);

        byte[] hash_value = SHA256.HashData(hash_bytes);

        return (Convert.ToHexString(hash_value));
    }

    public static async Task<DataErrorDTO> HandleLogin(User user, IUserRepository repo)
    {
        user.Password = HashData(user.Password);

        User? fetched_user = await repo.GetUserByUsernameAsync(user.UserName);

        if (fetched_user == null)
        {
            return new DataErrorDTO { Error = "No user found", Data = "" };
        }

        if (fetched_user.Password != user.Password)
        {
            return new DataErrorDTO { Error = $"Password of user '{user.UserName}' does not match", Data = "" };

        }

        return new DataErrorDTO { Error = "", Data = HashData(user.UserName + DateTime.Now.ToString("MM/dd/yyyy")) };
    }

    public static async Task<DataErrorDTO> HandleSignUp(User user, IUserRepository repo)
    {
        user.Password = HashData(user.Password);

        if (await repo.AddUserAsync(user) == true) {
            return new DataErrorDTO {Data = HashData(user.UserName + DateTime.Now.ToString("MM/dd/yyyy")), Error = "" };
        }

        return new DataErrorDTO { Error = $"'{user.UserName}' is already taken", Data = "" };
    }

    public static async Task<DataErrorDTO> VerifySession(string username, string session_cookie, IUserRepository repo)
    {
        string hash_data = HashData(username + DateTime.Now.ToString("MM/dd/yyyy"));

        User? user = await repo.GetUserByUsernameAsync(username);

        if (hash_data == session_cookie && user != null)
        {
            return new DataErrorDTO { Error = "", Data = hash_data };
        }
        else
        {
            return new DataErrorDTO { Error = "Session cookie mismatch", Data = "" };

        }
    }

    public static FrontendSearchCommandDTO[] SendCommands()
    {

        return [
                    new FrontendSearchCommandDTO("Contains:", "Search contains a certain keyword"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
                new FrontendSearchCommandDTO("Test", "A command for testing whether this works or not"),
            ];

    }

    //TODO: (nkc) PLACEHOLDER. This should be part of the searchcontroller later
    public static object[] Search(SearchDetailsDTO details)
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
    }

    public static DataErrorDTO GetServiceKey(string service, WebApplication app)
    {
        switch (service)
        {
            case "dropbox":
                return new DataErrorDTO { Error = "", Data = app.Configuration["DBOX_ID"] };

            case "google_drive":
                return new DataErrorDTO { Error = "", Data = app.Configuration["GDRIVE_ID"] };

            default:
                return new DataErrorDTO { Error = "Service requested is not implemented", Data = "" };

        }
    }
}