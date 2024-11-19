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
        private readonly InvertedIndexRepository _invertedIndexRepository;
        public InvertedIndexService()
        {

        }
        public InvertedIndexService(InvertedIndexRepository invertedIndexRepository)
        {
            _invertedIndexRepository = invertedIndexRepository ?? throw new ArgumentNullException(nameof(invertedIndexRepository));
        }

        public async Task IndexFiles()
        {
            string currentDirectory = "C:\\Users\\DJBer\\OneDrive\\Desktop\\Uni\\P7\\P7-PSEngine\\src\\P7-PSEngine\\Files\\";
            var files = Directory.GetFiles(currentDirectory);
            foreach (var fileName in files)
            {
                FileInformation? document = await _invertedIndexRepository.FindDocumentAsync(fileName);
                if (document == null)
                {
                    await AddFileToDb(fileName, await File.ReadAllTextAsync(fileName));
                }
                else
                {
                    UpdateFileInDb(document, await File.ReadAllTextAsync(fileName));
                }
            }
            await _invertedIndexRepository.Save();
        }

        public void UpdateFileInDb(FileInformation document, string content)
        {
            document.IndexInformations.Clear();
            var tokens = Regex.Split(content.ToLower(), "\\W+");
            AddInvertedIndicies(document, tokens);
        }

        public async Task AddFileToDb(string fileName, string content)
        {
            FileInformation? document = await _invertedIndexRepository.FindDocumentAsync(fileName);
            if (document != null)
            {
                throw new Exception("document was not null");
            }
            document ??= new FileInformation() { FileName = fileName, IndexInformations = [] };

            var tokens = Regex.Split(content.ToLower(), "\\W+");
            AddInvertedIndicies(document, tokens);
            await _invertedIndexRepository.AddDocumentAsync(document);
        }

        public void AddInvertedIndicies(FileInformation document, string[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                if (!string.IsNullOrEmpty(token))
                {
                    document.IndexInformations.Add(new IndexInformation(token, i));
                }
            }
        }
    }
}
