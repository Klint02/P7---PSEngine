using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;

namespace P7_PSEngine.Repositories
{
    public class InvertedIndexRepository
    {
        private readonly PSengineDB _db;

        public InvertedIndexRepository(PSengineDB db)
        {
            _db = db;
        }

        public InvertedIndexRepository()
        {

        }
        
        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
        public async Task AddDocumentAsync(FileInformation document)
        {
            await _db.FileInformations.AddAsync(document);
        }
        public async Task<FileInformation?> FindDocumentAsync(string fileName) => await _db.FileInformations.Include(p => p.IndexInformations).FirstOrDefaultAsync(p => p.FileName == fileName);
    }
}
