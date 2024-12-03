using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;

namespace P7_PSEngine.Repositories
{
    public interface IInvertedIndexRepository
    {
        Task AddDocumentAsync(DocumentInformation document);
        Task<DocumentInformation?> FindDocumentAsync(string documentName, int userId);
        Task Save();
        Task<InvertedIndex> FindTerm(string term, int userId);
        Task AddInvertedIndexAsync(InvertedIndex invertedIndex);
        Task<TermInformation?> FindExistingDocumentAsync(string term, string docId, int userId);
        Task EnsureUserExistsOrCreateAsync(int userId);
        Task<InvertedIndex?> FindInvertedTerm(string term, int userId);

        Task<User?> FindUserAsync(int userId);
        Task AddTermAsync(TermInformation term);
    }
    public class InvertedIndexRepository : IInvertedIndexRepository
    {
        private readonly PSengineDB db;
        public InvertedIndexRepository(PSengineDB db)
        {
            this.db = db;
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }

        public async Task AddDocumentAsync(DocumentInformation document)
        {
            var existingDocument = await db.DocumentInformation
                .FirstOrDefaultAsync(d => d.DocId == document.DocId && d.UserId == document.UserId);

            if (existingDocument == null)
            {
                Console.WriteLine($"Adding document: DocId={document.DocId}, UserId={document.UserId}");
                db.DocumentInformation.Add(document);
                await db.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine($"Document already exists: DocId={document.DocId}, UserId={document.UserId}");
            }
        }
        /*
        public async Task AddDocumentAsync(DocumentInformation document)
        {
            await db.DocumentInformation.AddAsync(document);
            await db.SaveChangesAsync();
        }*/
        public async Task<DocumentInformation?> FindDocumentAsync(string documentName, int userId) => await db.DocumentInformation.Include(p => p.TermDocuments).FirstOrDefaultAsync(p => p.DocumentName == documentName && p.UserId == userId);

        public async Task<InvertedIndex?> FindTerm(string term, int userId) => await db.InvertedIndex.FirstOrDefaultAsync(p => p.Term == term && p.UserId == userId);

        public async Task AddInvertedIndexAsync(InvertedIndex invertedIndex)
        {
            await db.InvertedIndex.AddAsync(invertedIndex);
        }
        public async Task<TermInformation?> FindExistingDocumentAsync(string term, string docID, int userId) => await db.TermInformations.FirstOrDefaultAsync(p => p.Term == term && p.DocID == docID && p.UserId == userId);

        public async Task<User?> FindUserAsync(int userId) => await db.Users.FirstOrDefaultAsync(p => p.Id == userId);

        public async Task<InvertedIndex?> FindInvertedTerm(string term, int userId) => await db.InvertedIndex.FirstOrDefaultAsync(p => p.Term == term && p.UserId == userId);

        public async Task AddTermAsync(TermInformation term) => await db.TermInformations.AddAsync(term);

        public async Task EnsureUserExistsOrCreateAsync(int userId)
        {
            var user = await db.Users.FindAsync(userId);
            if (user == null)// Delete this in final product
            {
                user = new User { Id = userId, Username = "Default Name", Password = "1234" }; // Adjust fields as needed
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
        }
    }
}
