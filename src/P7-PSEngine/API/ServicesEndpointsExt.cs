using Microsoft.EntityFrameworkCore;
using P7_PSEngine.DTO;
using P7_PSEngine.Data;
using P7_PSEngine.Model;
using System.Text; 
using System.Text.Json;
using System.Threading.Tasks;

namespace P7_PSEngine.API
{
    public static class ServicesEndpointsExt
    {


        static async Task GetToken(ServiceCreationDetailsDTO Details) {
            string client_id = "";
            string redirect_url = "http://localhost:8070/linkuser";
            //Needed for GDrive but not dropbox
            //string scope = "offline_access Files.Read.All";
            string client_secret = "";


            //Console.WriteLine($"{Details.code}, \n {Details.service_type}, \n {Details.user}, \n {Details.user_defined_service_name}");
            using (HttpClient client = new HttpClient())
            {
                var token_data = new FormUrlEncodedContent (new [] {
                    new KeyValuePair<string, string>("client_id", client_id),
                    new KeyValuePair<string, string>("client_secret", client_secret),
                    new KeyValuePair<string, string>("code", Details.code),
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("redirect_uri", redirect_url),
                });
//$"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token"
                //192.122.0.5
                HttpResponseMessage response = await client.PostAsync($"https://api.dropboxapi.com/oauth2/token", token_data);
                            // Check the response status

                Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Request sent successfully!");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"bab: {responseBody}");


                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"bab: {responseBody}");
                }
            }
        }
        
        public static void MapServicesEndpoints(this WebApplication app)
        {
            string static_path = "/app/wwwroot";

            app.MapGet("/linkuser", () => Results.Content(File.ReadAllText($"{static_path}/html/auth.html"), "text/html"));

            app.MapPost("/api/addservice", async (ServiceCreationDetailsDTO Details) => {
                await GetToken(Details);
            });
            
        }
    }
}
