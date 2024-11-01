using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CloudFileIndexer;
using System.IO;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class IndexController 
{

        private readonly InvertedIndex _invertedIndex;

        public IndexController(InvertedIndex invertedIndex)
        {
            _invertedIndex = invertedIndex;
        }

        public string GetIndexData()
        {
        // Excerpts from file from Google Drive
        string jsonFilePath = "testgoogle.json";
        string jsonData = File.ReadAllText(jsonFilePath);

        FileList filelist = JsonConvert.DeserializeObject<FileList>(jsonData);

        // Creating dictionary to store inverted index of tokens and file IDs
        // Key: token, Value: list of file IDs (could probably be stored as an integer instead)
        var index = new CloudFileIndexer.InvertedIndex();

        // Loop through each file
        foreach (var file in filelist.Files)
        {
            // Tokenize the file name (split by spaces, punctuation, etc.)
            // The tokenization process should be more complex also using lemmatization and stemming
            string id = file.Id;
            string name = file.Name; 
            index.IndexDocument(id,name);
        }

        // Display the inverted index
        // index.DisplayIndex();
        
        return index.GetIndexData();
        }
}  

// Example file data class
public class FileList 
{
    public string NextPageToken {get; set;}
    public string Kind {get; set;}
    public bool IncompleteSearch {get; set;}
        
    public List<FileData> Files {get; set;}
}
public class FileData
{
    public string Kind {get; set;}
    public string MimeType {get; set;}
    public string Id { get; set; }
    public string Name { get; set; }
}
