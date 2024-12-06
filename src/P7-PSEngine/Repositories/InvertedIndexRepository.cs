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
            await db.FileInformation.AddAsync(document);
        }
        public async Task<FileInformation?> FindDocumentAsync(string fileName) => await db.FileInformation.Include(p => p.IndexInformations).FirstOrDefaultAsync(p => p.FileName == fileName);
    }
}
