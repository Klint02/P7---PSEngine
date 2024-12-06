using Newtonsoft.Json;
using P7_PSEngine.Model;
using P7_PSEngine.Repositories;
using System.Text.RegularExpressions;

namespace P7_PSEngine.Services
{
    public interface IInvertedIndexService
    {
        Task InitializeUser();
        Task IndexDocumentAsync(string fileId, string content, int userId);
        Task addOrUpdateTerm(string term, string docId, int userId);
    }

    public class InvertedIndexService : IInvertedIndexService
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
            FileList filelist = JsonConvert.DeserializeObject<FileList>(jsonData);
            Console.WriteLine("Running InitializeUser");
            Console.WriteLine(jsonData);
            //await IndexDocumentAsync("1", "jsonData", userId);


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
                await IndexDocumentAsync("10", "test", userId);
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
                    await IndexDocumentAsync(id, name, userId);
                }
            }
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

        public async Task addOrUpdateTerm(string term, string docId, int userId)
        {
            Console.WriteLine($"addOrUpdateTerm called with term: {term}, docId: {docId}, userId: {userId}");
            term = term.ToLower();

            // Ensure user exists or create a new user
            await invertedIndexRepository.EnsureUserExistsOrCreateAsync(userId);

            // Ensure the Document exists or create a new Document
            var documentexists = await invertedIndexRepository.FindDocumentAsync(docId, userId);
            Console.WriteLine(documentexists == null ? "Document not found" : "Document found");

            if (documentexists == null)
            {
                if (string.IsNullOrWhiteSpace(docId))
                    throw new ArgumentException("DocId cannot be null or empty.", nameof(docId));

                var document = new DocumentInformation
                {
                    DocumentName = term,
                    DocId = docId,
                    UserId = userId,
                    TermDocuments = new List<TermInformation>()
                };
                Console.WriteLine("Adding new document to repository");
                Console.WriteLine($"DocumentName: {document.DocumentName}, UserId: {document.UserId}, DocumentId: {document.DocId}");
                await invertedIndexRepository.AddDocumentAsync(document);
            }

            // Find term in InvertedIndex
            var invertedIndexEntry = await invertedIndexRepository.FindTerm(term, userId);
            Console.WriteLine(invertedIndexEntry == null ? "Term not found in InvertedIndex" : "Term found in InvertedIndex");

            if (invertedIndexEntry == null)
            {
                var termInformationEntry = new TermInformation
                {
                    DocID = docId,
                    TermFrequency = 1,
                };

                invertedIndexEntry = new InvertedIndex
                {
                    Term = term,
                    UserId = userId,
                    DocumentFrequency = 1,
                    TotalTermFrequency = 1,
                    TermInformations = new List<TermInformation> { termInformationEntry }
                };

                Console.WriteLine("Adding new term to InvertedIndex");
                await invertedIndexRepository.AddInvertedIndexAsync(invertedIndexEntry);
                await invertedIndexRepository.Save();
            }
            else
            {
                var wordFileEntry = await invertedIndexRepository.FindExistingDocumentAsync(term, docId, userId);
                Console.WriteLine(wordFileEntry == null ? "No existing TermInformation found" : "Existing TermInformation found");
                //var updateInvertedIndex = await invertedIndexRepository.FindInvertedTerm(term, userId);
                //if (updateInvertedIndex == null)
                //{
                //    throw new InvalidOperationException("InvertedIndex entry not found");
                //}

                if (wordFileEntry == null)
                {
                    wordFileEntry = new TermInformation
                    {
                        Term = term,
                        UserId = userId,
                        TermFrequency = 1,
                        DocID = docId,
                    };

                    invertedIndexEntry.DocumentFrequency++;
                    invertedIndexEntry.TotalTermFrequency++;

                    Console.WriteLine("Adding new TermInformation for document");
                    await invertedIndexRepository.AddTermAsync(wordFileEntry);
                    await invertedIndexRepository.Save();
                }
                else
                {
                    wordFileEntry.TermFrequency++;
                    invertedIndexEntry.DocumentFrequency++;
                    invertedIndexEntry.TotalTermFrequency++;
                    Console.WriteLine($"Incrementing TermFrequency for term: {term}");
                }
            }

            await invertedIndexRepository.Save();
            Console.WriteLine("Changes saved to repository");
        }

        public async Task IndexDocumentAsync(string fileId, string content, int userId)
        {
            var tokens = Regex.Split(content.ToLower(), @"\W+");
            foreach (var token in tokens)
            {
                await addOrUpdateTerm(token, fileId, userId);
            }
        }
    }
}
