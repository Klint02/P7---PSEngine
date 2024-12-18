using Newtonsoft.Json;
using P7_PSEngine.Model;
using P7_PSEngine.Repositories;
using System.Text.RegularExpressions;

namespace P7_PSEngine.Services
{
    public interface IInvertedIndexService
    {
        //Task InitializeUser(User user);
        Task IndexFileAsync(string fileId, string content, User user);
        Task addOrUpdateTerm(string term, string docId, User user);
    }


    //TODO (djb) Remove whitespaces from indexing tokens
    //TODO (djb) Remove redundant code
    public class InvertedIndexService : IInvertedIndexService
    {
        private readonly IInvertedIndexRepository invertedIndexRepository;
        public InvertedIndexService(IInvertedIndexRepository invertedIndexRepository)
        {
            this.invertedIndexRepository = invertedIndexRepository ?? throw new ArgumentNullException(nameof(invertedIndexRepository));
        }

        //public async Task InitializeUser(User user)
        //{
        //    string currentDirectory = "./Files";
        //    var files = Directory.GetFiles(currentDirectory);
        //    string jsonFilePath = "./Files/testgoogle.json";
        //    string jsonData = await File.ReadAllTextAsync(jsonFilePath);
        //    FileList filelist = JsonConvert.DeserializeObject<FileList>(jsonData);

        //    if (filelist == null)
        //    {
        //        throw new InvalidOperationException("Invalid file data (filelist null)");
        //    }
        //    else if (filelist.Files == null)
        //    {
        //        throw new InvalidOperationException("Invalid file data (filelist.Files null)");
        //    }
        //    else if (filelist.Files.Count == 0)
        //    {
        //        throw new InvalidOperationException("Invalid file data (filelist.Files empty)");
        //    }
        //    else
        //    {
        //        // Creating dictionary to store inverted index of tokens and file IDs
        //        // Key: token, Value: list of file IDs (could probably be stored as an integer instead)
        //        // var index = new InvertedIndexRepository();

        //        // Loop through each file
        //        foreach (var file in filelist.Files)
        //        {
        //            // Tokenize the file name (split by spaces, punctuation, etc.)
        //            // The tokenization process should be more complex also using lemmatization and stemming
        //            string id = file.Id;
        //            string name = file.Name;
        //            await IndexFileAsync(id, name, user);
        //        }
        //    }
        //}

        public async Task addOrUpdateTerm(string term, string fileId, User user)
        {
            //Console.WriteLine($"addOrUpdateTerm called with term: {term}, fileId: {fileId}, userId: {user.UserId}");
            //term = term.ToLower();

            // Ensure user exists or create a new user
            //await invertedIndexRepository.EnsureUserExistsOrCreateAsync(user);

            // Ensure the Document exists or create a new Document
            var fileexists = await invertedIndexRepository.FindFileAsync(fileId, user);
            Console.WriteLine(fileexists == null ? "File not found" : "File found");
            var cloudService = await invertedIndexRepository.GetCloudService(user);
            if (cloudService == null)
            {
                throw new Exception("Cloudservice could not be found");
            }

            if (fileexists == null)
            {
                if (string.IsNullOrWhiteSpace(fileId))
                    throw new ArgumentException("DocId cannot be null or empty.", nameof(fileId));

                var file = new FileInformation
                {
                    FileName = term, // Remember to change this to the actual file name
                    FileId = fileId,
                    UserId = user.UserId,
                    FilePath = "Suck my ass",
                    FileType = "json",
                    SID = cloudService,
                    //TermFiles = new List<TermInformation>()
                };
                //Console.WriteLine("Adding new document to repository");
                //Console.WriteLine($"DocumentName: {file.FileName}, UserId: {file.UserId}, DocumentId: {file.FileId}");
                await invertedIndexRepository.AddFileAsync(file);
            }

            // Find term in InvertedIndex
            var invertedIndexEntry = await invertedIndexRepository.FindTerm(term, user);
            //Console.WriteLine(invertedIndexEntry == null ? "Term not found in InvertedIndex" : "Term found in InvertedIndex");

            if (invertedIndexEntry == null)
            {
                var termInformationEntry = new TermInformation
                {
                    FileId = fileId,
                    TermFrequency = 1,
                };

                invertedIndexEntry = new InvertedIndex
                {
                    Term = term,
                    UserId = user.UserId,
                    FileFrequency = 1,
                    TotalTermFrequency = 1,
                    TermInformations = new List<TermInformation> { termInformationEntry }
                };

                //Console.WriteLine("Adding new term to InvertedIndex");
                await invertedIndexRepository.AddInvertedIndexAsync(invertedIndexEntry);
            }
            else
            {
                var wordFileEntry = await invertedIndexRepository.FindExistingFileAsync(term, fileId, user);
                //Console.WriteLine(wordFileEntry == null ? "No existing TermInformation found" : "Existing TermInformation found");

                if (wordFileEntry == null)
                {
                    wordFileEntry = new TermInformation
                    {
                        Term = term,
                        UserId = user.UserId,
                        TermFrequency = 1,
                        FileId = fileId,
                    };

                    invertedIndexEntry.FileFrequency++;
                    invertedIndexEntry.TotalTermFrequency++;

                    //Console.WriteLine("Adding new TermInformation for document");
                    await invertedIndexRepository.AddTermAsync(wordFileEntry);
                }
                else
                {
                    wordFileEntry.TermFrequency++;
                    invertedIndexEntry.FileFrequency++;
                    invertedIndexEntry.TotalTermFrequency++;
                    //Console.WriteLine($"Incrementing TermFrequency for term: {term}");
                }
            }

            await invertedIndexRepository.Save();
            //Console.WriteLine("Changes saved to repository");
        }

        public async Task IndexFileAsync(string fileId, string content, User user)
        {
            var tokens = Regex.Split(content.ToLower(), @"[^a-zA-ZæøåÆØÅ]+");
            foreach (var token in tokens)
            {
                await addOrUpdateTerm(token, fileId, user);
            }
        }
    }
}
