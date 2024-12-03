//using Dropbox.Api;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Newtonsoft.Json;

namespace CloudSearcher
{
    /*public class Dropbox
    {
        private readonly string _accessToken;
        private DropboxClient _client;

        public DropboxGetter()
        {
            _accessToken = GetAccessToken();
            InitializeClient();
        }

        private void InitializeClient()
        {
            _client = new DropboxClient(_accessToken);
        }

        private string GetAccessToken()
        {
            var parentDirectory = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(parentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Personal.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            return configuration["Dropbox:AccessToken"];
        }

        // Add methods to interact with Dropbox here
        // Here is a method to get all the files in a folder
        public async Task<string> GetCurrentAccountAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await client.PostAsync("https://api.dropboxapi.com/2/users/get_current_account", null);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> GetFilesInFolderAsync(string folderPath, string rootNamespaceId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                client.DefaultRequestHeaders.Add("Dropbox-API-Path-Root", 
                    $"{{\".tag\": \"root\", \"root\": \"{rootNamespaceId}\"}}");

                var requestbody = new 
                {
                    path = folderPath,
                    limit = 2000,
                    recursive = false
                };

                var jsonRequestBody = System.Text.Json.JsonSerializer.Serialize(requestbody);

                var response = await client.PostAsync(
                    "https://api.dropboxapi.com/2/files/list_folder", 
                    new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json")
                    );
                
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }        

        public async Task ExampleUsage()
        {
            string accountInfo = await GetCurrentAccountAsync();
            Console.WriteLine(accountInfo);
            string filesInFolder = await GetFilesInFolderAsync("", "127332311");
            // Serialize the filesInFolder object to a JSON string and write it to a file
            File.WriteAllText("filesInFolder.json", filesInFolder);
            //Console.WriteLine(folderResponse);

            string jsonContent = File.ReadAllText("filesInFolder.json");

            // Deserialize the JSON into FolderResponse
            var folderResponse = System.Text.Json.JsonSerializer.Deserialize<FolderResponse>(jsonContent);
            // Console.WriteLine(folderResponse);
            // Create a dictionary linking path_display and id
            var pathIdDictionary = new Dictionary<string, string>();

            foreach (var entry in folderResponse.Entries)
            {
                pathIdDictionary[entry.PathDisplay] = entry.Id;
            }

            // Print the dictionary
            foreach (var kvp in pathIdDictionary)
            {
                Console.WriteLine($"Path: {kvp.Key}, ID: {kvp.Value}");
            }
        }

    }*/

    public class DropboxFile : ICloudFile
    {
        private readonly Entry _entry;

        public DropboxFile(Entry entry)
        {
            _entry = entry;
        }

        public string Id => _entry.Id;
        public string Name => _entry.Name;
    }

    public class Entry
    {
    [JsonProperty(".tag")]
    public string Tag { get; set; } 
    public string Name { get; set; }
    public string PathLower { get; set; }
    public string PathDisplay { get; set; }
    public string Id { get; set; }
    }

public class FolderResponse
    {
    [JsonProperty("entries")]
    public List<Entry> Entries { get; set; }
    }
}