using CloudFileIndexer;
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

        if (!string.IsNullOrEmpty(jsonData))
        {
            FileList? filelist = JsonConvert.DeserializeObject<FileList>(jsonData);

            if (filelist != null && filelist.Files != null)
            {
                // Creating dictionary to store inverted index of tokens and file IDs
                // Key: token, Value: list of file IDs (could probably be stored as an integer instead)
                var index = new InvertedIndex();

                // Loop through each file
                foreach (var file in filelist.Files)
                {
                    // Tokenize the file name (split by spaces, punctuation, etc.)
                    // The tokenization process should be more complex also using lemmatization and stemming
                    string id = file.Id;
                    string name = file.Name;
                    index.IndexDocument(id, name);
                }

                // Display the inverted index
                // index.DisplayIndex();

                return index.GetIndexData();
            }

            throw new InvalidOperationException("Invalid file data");
        }

        throw new InvalidOperationException("File data is empty");
    }
}

// Example file data class
public class FileList
{
    public string NextPageToken { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public bool IncompleteSearch { get; set; } = false;

    public List<FileData> Files { get; set; } = new List<FileData>();
}
public class FileData
{
    public string Kind { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
