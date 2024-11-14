using CloudFileIndexer;
using Newtonsoft.Json;

public class IndexService
{
    private InvertedIndex _invertedIndex;

    public IndexService(InvertedIndex invertedIndex)
    {
        _invertedIndex = invertedIndex ?? throw new ArgumentNullException(nameof(invertedIndex));
    }

    public void Initialize()
    {
        // Excerpts from file from Google Drive
        _invertedIndex.tester = "Hello";
        string currentDirectory = Directory.GetCurrentDirectory();
        //   Console.WriteLine($"Current directory: {currentDirectory}");
        string[] files = Directory.GetFiles(currentDirectory);
        //    Console.WriteLine("Files in current directory:");
        foreach (string file in files)
        {
            //        Console.WriteLine(file);
        }
        string jsonFilePath = "Files/testgoogle.json";
        string jsonData = File.ReadAllText(jsonFilePath);
        //    Console.WriteLine($"File data: {jsonData}");
        FileList filelist = JsonConvert.DeserializeObject<FileList>(jsonData);
        //Console.WriteLine($"File data: {filelist}");

        if (filelist == null)
        {
            throw new InvalidOperationException("Invalid file data (filelist null)");
        }
        else if (filelist.Files == null)
        {
            throw new InvalidOperationException("Invalid file data (filelist.Files null)");
        }
        else if (filelist.Files.Count == 0)
        {
            throw new InvalidOperationException("Invalid file data (filelist.Files empty)");
        }
        else
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
            index.DisplayIndex();

        }
    }
    public InvertedIndex GetInvertedIndex()
    {
        return _invertedIndex;
    }

    public class FileList
    {
        public string NextPageToken { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;
        public bool IncompleteSearch { get; set; } = false;
        public List<FileData> Files { get; set; } = new List<FileData>();
    }

    public class FileData
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

}


