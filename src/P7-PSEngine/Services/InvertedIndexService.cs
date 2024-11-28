using CloudFileIndexer;
using P7_PSEngine.Model;
using P7_PSEngine.Repositories;
using System.Text.RegularExpressions;

namespace P7_PSEngine.Services
{
    public interface IInvertedIndexService
    {
        Task IndexFiles();
    }

    public class InvertedIndexService: IInvertedIndexService
    {
        private readonly IInvertedIndexRepository invertedIndexRepository;
        public InvertedIndexService(IInvertedIndexRepository invertedIndexRepository)
        {
            this.invertedIndexRepository = invertedIndexRepository ?? throw new ArgumentNullException(nameof(invertedIndexRepository));
        }

        public async Task IndexFiles()
        {
            if (invertedIndexRepository == null)
            {
                throw new InvalidOperationException("InvertedIndexRepository is not initialized.");
            }
            string currentDirectory = "Files";
            var files = Directory.GetFiles(currentDirectory);
            foreach (var fileName in files)
            {
                FileInformation? document = await invertedIndexRepository.FindDocumentAsync(fileName);
                if (document == null)
                {
                    await AddFileToDb(fileName, await File.ReadAllTextAsync(fileName));
                }
                else
                {
                    UpdateFileInDb(document, await File.ReadAllTextAsync(fileName));
                }
            }
            await invertedIndexRepository.Save();
        }

        public void UpdateFileInDb(FileInformation document, string content)
        {
            document.WordInformations.Clear();
            var tokens = Regex.Split(content.ToLower(), "\\W+");
            AddInvertedIndicies(document, tokens);
        }

        public async Task AddFileToDb(string fileName, string content)
        {
            FileInformation? document = await invertedIndexRepository.FindDocumentAsync(fileName);
            if (document != null)
            {
                throw new Exception("document was not null");
            }
            document ??= new FileInformation() { FileName = fileName, WordInformations = [] };

            var tokens = Regex.Split(content.ToLower(), "\\W+");
            AddInvertedIndicies(document, tokens);
            await invertedIndexRepository.AddDocumentAsync(document);
        }

        public void AddInvertedIndicies(FileInformation document, string[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                if (!string.IsNullOrEmpty(token))
                {
                    document.WordInformations.Add(new WordInformation(token));
                }
            }
        }
    }
}
