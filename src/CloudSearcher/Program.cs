using System;
using System.IO;
using Newtonsoft.Json;
using CloudSearcher;
using System.Text.Json.Serialization;

namespace CloudSearcher
{
class Program
{
    static async Task Main(string[] args)
    {

        Console.WriteLine("Hello, World!");

        string googleFilePath = "testgoogle.json";
        string jsonData = File.ReadAllText(googleFilePath);
        string dropboxFilePath = "filesInFolder.json";
        string jsonData2 = File.ReadAllText(dropboxFilePath);

        var loader = new CloudFileLoader();

        // Load Google Drive files
        var googleFiles = loader.LoadGoogleDriveFiles(jsonData);

        // Load Dropbox files
        var dropboxFiles = loader.LoadDropboxFiles(jsonData2);

        var cloudFiles = googleFiles.Concat(dropboxFiles);

        // Deserialize the JSON data from Google Drive into a FileList object
//        FileList filelist = JsonConvert.DeserializeObject<FileList>(jsonData);
        // Deserialize the JSON data from Dropbox into a FolderResponse object

        // Creating dictionary to store inverted index of tokens and file IDs
        // Key: token, Value: list of file IDs (could probably be stored as an integer instead)
        var index = new CloudSearcher.InvertedIndex();

        // Loop through each file
        foreach (var file in cloudFiles)
        {
            // Tokenize the file name (split by spaces, punctuation, etc.)
            // The tokenization process should be more complex also using lemmatization and stemming
            string id = file.Id;
            string name = file.Name; 
            index.IndexDocument(id,name);
        }

        // Print the inverted index
        index.DisplayIndex();

        // Return output for the term "Friskole"
        var term = "vestliv";
        var result = index.GetTermData(term);
        Console.WriteLine(result);

        // Conduct a boolean search
        var booleanSearch = new BooleanSearch(index);
        var searchResults = booleanSearch.BoolSearch(term);
        Console.WriteLine(JsonConvert.SerializeObject(searchResults, Formatting.Indented));

        booleanSearch.DisplaySearchResults(searchResults);

        // Example usage of DropboxGetter
//        var dropboxGetter = new DropboxGetter();
//        await dropboxGetter.ExampleUsage();

    }
}
}

public class FileList 
{
    public string NextPageToken {get; set;} = "";
    public string Kind {get; set;} = "";
    public bool IncompleteSearch {get; set;} = false;
        
    public List<FileData> Files {get; set;} = new List<FileData>();
}
public class FileData
{
    public string Kind {get; set;} = "";
    public string MimeType {get; set;} = "";
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
}

public class Entry
{
    [JsonPropertyName(".tag")]
    public string Tag { get; set; }
    public string Name { get; set; }
    public string PathLower { get; set; }
    public string PathDisplay { get; set; }
    public string Id { get; set; }
}

public class FolderResponse
{
    public List<CloudSearcher.Entry> Entries { get; set; }
}