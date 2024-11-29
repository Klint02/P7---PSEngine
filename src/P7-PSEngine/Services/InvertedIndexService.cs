using CloudFileIndexer;
using P7_PSEngine.Model;
using P7_PSEngine.Repositories;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace P7_PSEngine.Services
{
    public interface IInvertedIndexService
    {
        
    }

    public class InvertedIndexService: IInvertedIndexService
    {
        private readonly IInvertedIndexRepository invertedIndexRepository;
        public InvertedIndexService(IInvertedIndexRepository invertedIndexRepository)
        {
            this.invertedIndexRepository = invertedIndexRepository ?? throw new ArgumentNullException(nameof(invertedIndexRepository));
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

        public async void addOrUpdateWord(string word, string fileId, int userId)
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

        public void indexfile(string fileId, string content, int userId) 
        {
            var tokens = Regex.Split(content.ToLower(), @"\W+");
            foreach (var token in tokens)
            {
                addOrUpdateWord(token, fileId, userId);
            }
        }
    }
}
