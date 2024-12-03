using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;

namespace P7_PSEngine.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<DocumentInformation>> SearchDocuments(IEnumerable<string> search);
        Task<IEnumerable<DocumentInformation>> GetALlDocumentsWithIndex();
    }

    public class SearchService : ISearchService
    {
        private readonly PSengineDB _db;

        public SearchService(PSengineDB db)
        {
            _db = db;
        }

        public async Task<IEnumerable<DocumentInformation>> SearchDocuments(IEnumerable<string> search)
        {
            List<DocumentInformation> documents = await _db.DocumentInformation.Include(p => p.TermDocuments.Where(p => search.Contains(p.Term))).Where(p => p.TermDocuments.Any(index => search.Contains(index.Term))).AsNoTracking().ToListAsync();

            return documents;
        }

        public async Task<IEnumerable<DocumentInformation>> GetALlDocumentsWithIndex()
        {
            List<DocumentInformation> documents = await _db.DocumentInformation.Include(p => p.TermDocuments).AsNoTracking().ToListAsync();

            return documents;
        }
    }
}
