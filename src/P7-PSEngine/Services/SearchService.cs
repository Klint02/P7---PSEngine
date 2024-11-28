using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;
using System.Reflection.Metadata;

namespace P7_PSEngine.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<FileInformation>> SearchDocuments(IEnumerable<string> search);
        Task<IEnumerable<FileInformation>> GetALlDocumentsWithIndex();
    }

    public class SearchService : ISearchService
    {
        private readonly PSengineDB _db;

        public SearchService(PSengineDB db)
        {
            _db = db;
        }

        public async Task<IEnumerable<FileInformation>> SearchDocuments(IEnumerable<string> search)
        {
            List<FileInformation> documents = await _db.FileInformations.Include(p => p.WordInformations.Where(p => search.Contains(p.Word))).Where(p => p.WordInformations.Any(index => search.Contains(index.Word))).AsNoTracking().ToListAsync();

            return documents;
        }

        public async Task<IEnumerable<FileInformation>> GetALlDocumentsWithIndex()
        {
            List<FileInformation> documents = await _db.FileInformations.Include(p => p.WordInformations).AsNoTracking().ToListAsync();

            return documents;
        }
    }
}
