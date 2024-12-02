using CloudFileIndexer;
using P7_PSEngine.Model;
using P7_PSEngine.Repositories;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using InvertedIndex = CloudFileIndexer.InvertedIndex;

namespace P7_PSEngine.Services
{
    public interface IInvertedIndexService
    {
        Task InitializeUser();
    }

    public class InvertedIndexService: IInvertedIndexService
    {
        private readonly IInvertedIndexRepository invertedIndexRepository;
        public InvertedIndexService(IInvertedIndexRepository invertedIndexRepository)
        {
            this.invertedIndexRepository = invertedIndexRepository ?? throw new ArgumentNullException(nameof(invertedIndexRepository));
        }

        public async Task InitializeUser()
        {
            int userId = 1;
            string currentDirectory = "./Files";
            var files = Directory.GetFiles(currentDirectory);
            string jsonFilePath = "./Files/testgoogle.json";
            string jsonData = await File.ReadAllTextAsync(jsonFilePath);
            //FileList filelist = JsonConvert.DeserializeObject<FileList>(jsonData);
            Console.WriteLine("Running InitializeUser");
            Console.WriteLine(jsonData);
            await IndexFileAsync("1", "jsonData", userId);
            
            /*
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
                await IndexFileAsync("1", "test", userId);
            // Creating dictionary to store inverted index of tokens and file IDs
            // Key: token, Value: list of file IDs (could probably be stored as an integer instead)
            // var index = new InvertedIndexRepository();

            // Loop through each file
            foreach (var file in filelist.Files)
            {
                // Tokenize the file name (split by spaces, punctuation, etc.)
                // The tokenization process should be more complex also using lemmatization and stemming
                string id = file.Id;
                string name = file.Name;
                await IndexFileAsync(id, name, userId);
            }*/
        }
    

        //public async Task IndexFiles()
        //{
        //    string currentDirectory = "Files";
        //    var files = Directory.GetFiles(currentDirectory);
        //    foreach (var fileName in files)
        //    {
        //        FileInformation? document = await invertedIndexRepository.FindDocumentAsync(fileName);
        //        if (document == null)
        //        {
        //            await AddFileToDb(fileName, await File.ReadAllTextAsync(fileName));
        //        }
        //        else
        //        {
        //            UpdateFileInDb(document, await File.ReadAllTextAsync(fileName));
        //        }
        //    }
        //    await invertedIndexRepository.Save();
        //}

        //public void UpdateFileInDb(FileInformation document, string content)
        //{
        //    document.WordInformations.Clear();
        //    var tokens = Regex.Split(content.ToLower(), "\\W+");
        //    AddInvertedIndicies(document, tokens);
        //}

        //public async Task AddFileToDb(string fileName, string content)
        //{
        //    FileInformation? document = await invertedIndexRepository.FindDocumentAsync(fileName);
        //    if (document != null)
        //    {
        //        throw new Exception("document was not null");
        //    }
        //    document ??= new FileInformation() { FileName = fileName, WordInformations = [] };

        //    var tokens = Regex.Split(content.ToLower(), "\\W+");
        //    AddInvertedIndicies(document, tokens);
        //    await invertedIndexRepository.AddDocumentAsync(document);
        //}

        public async Task addOrUpdateWord(string word, string fileId, int userId)
        {
            word = word.ToLower();

            var invertedIndexEntry = await invertedIndexRepository.FindWord(word, userId);

            if (invertedIndexEntry == null)
            {
                invertedIndexEntry = new WordInformation
                {
                    Word = word,
                    UserId = userId,
                    WordFrequency = 1,
                    FileID = fileId,
                };

                await invertedIndexRepository.AddWordAsync(invertedIndexEntry);
            }

            var wordFileEntry = await invertedIndexRepository.FindExistingFileAsync(word, fileId, userId);

            if (wordFileEntry == null)
            {
                wordFileEntry = new WordInformation
                {
                    Word = word,
                    UserId = userId,
                    WordFrequency = 1,
                    FileID = fileId,
                };

            }
            else {
                wordFileEntry.WordFrequency++;
            }
            await invertedIndexRepository.Save();
        }

        public async Task IndexFileAsync(string fileId, string content, int userId) 
        {
            var tokens = Regex.Split(content.ToLower(), @"\W+");
            foreach (var token in tokens)
            {
                await addOrUpdateWord(token, fileId, userId);
            }
        }
    }
}
