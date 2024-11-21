using System;
using System.IO;
using Newtonsoft.Json;
using CloudSearcher;


// See https://aka.ms/new-console-template for more information
        Console.WriteLine("Hello, World!");

        string jsonFilePath = "testgoogle.json";
        string jsonData = File.ReadAllText(jsonFilePath);

        FileList filelist = JsonConvert.DeserializeObject<FileList>(jsonData);

        // Creating dictionary to store inverted index of tokens and file IDs
        // Key: token, Value: list of file IDs (could probably be stored as an integer instead)
        var index = new CloudSearcher.InvertedIndex();

        // Loop through each file
        foreach (var file in filelist.Files)
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
        var term = "faktura";
        var result = index.GetTermData(term);
        Console.WriteLine(result);

        // Conduct a boolean search
        var booleanSearch = new BooleanSearch(index);
        var searchResults = booleanSearch.BoolSearch(term);
        Console.WriteLine(JsonConvert.SerializeObject(searchResults, Formatting.Indented));

        booleanSearch.DisplaySearchResults(searchResults);



        // Example file data class
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