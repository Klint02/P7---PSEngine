using P7_PSEngine.DTO;
using P7_PSEngine.Data;
using P7_PSEngine.Model;
using P7_PSEngine.API;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace P7_PSEngine.Controllers;

public static class FrontendController {

    private static string HashData(string string_to_hash) {
        byte[] hash_bytes = Encoding.UTF8.GetBytes(string_to_hash);

        byte[] hash_value = SHA256.HashData(hash_bytes);

        return(Convert.ToHexString(hash_value));
    }
    
    public static async Task<DataErrorDTO> HandleLogin (User user, IUserRepository repo) {
        user.Password = HashData(user.Password);

        User? fetched_user = await repo.GetUserByUsernameAsync(user.Username);

        if (fetched_user == null) {
            return new DataErrorDTO { Error = "No user found", Data = "" };
        }

        if (fetched_user.Password != user.Password) {
            return new DataErrorDTO { Error = $"Password of user '{user.Username}' does not match", Data = "" };

        }

        return new DataErrorDTO { Error = "", Data = HashData(user.Username + DateTime.Now.ToString("MM/dd/yyyy")) };
    }

    public static async Task<DataErrorDTO> HandleSignUp (User user, IUserRepository repo) {
        user.Password = HashData(user.Password);

        User? fetched_user = await repo.GetUserByUsernameAsync(user.Username);

        if (fetched_user == null) {
            await repo.AddUserAsync(user);
            await repo.SaveDbChangesAsync();
            return new DataErrorDTO {Data = HashData(user.Username + DateTime.Now.ToString("MM/dd/yyyy")), Error = "" };
        }

        return new DataErrorDTO {Error = $"'{user.Username}' is already taken", Data = "" };
    }

    public static bool VerifySession (string username, string session_cookie) {
        return (HashData(username + DateTime.Now.ToString("MM/dd/yyyy")) == session_cookie);
    }

    public static FrontendSearchCommandDTO[] SendCommands () {

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
    public static object[] Search(SearchDetailsDTO details) {
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
}