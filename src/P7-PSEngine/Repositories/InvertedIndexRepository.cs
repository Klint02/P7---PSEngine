using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;
using System.Reflection.Metadata;

namespace P7_PSEngine.Repositories
{
    public interface IInvertedIndexRepository
    {
        Task AddDocumentAsync(FileInformation document);
        Task<FileInformation?> FindDocumentAsync(string fileName);
        Task Save();
        Task<WordInformation> FindWord(string word, int userId);
        Task AddWordAsync(WordInformation word);
        Task<WordInformation?> FindExistingFileAsync(string word, string fileId, int userId);
    }
    public class InvertedIndexRepository : IInvertedIndexRepository
    {
        private readonly PSengineDB db;
        public InvertedIndexRepository(PSengineDB db)
        {
            this.db = db;
        }

        public InvertedIndexRepository()
        {

        }
        
        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
        public async Task AddDocumentAsync(FileInformation document)
        {
            await db.FileInformations.AddAsync(document);
        }
        public async Task<FileInformation?> FindDocumentAsync(string fileName) => await db.FileInformations.Include(p => p.WordInformations).FirstOrDefaultAsync(p => p.FileName == fileName);

        public async Task<WordInformation?> FindWord(string word, int userId) => await db.WordInformations.FindAsync(word, userId);

        public async Task AddWordAsync(WordInformation word)
        {
            await db.WordInformations.AddAsync(word);
        }
        public async Task<WordInformation?> FindExistingFileAsync(string word, string fileId, int userId) => await db.WordInformations.FirstOrDefaultAsync(p => p.Word == word && p.FileID == fileId && p.UserId == userId);

    }
}
