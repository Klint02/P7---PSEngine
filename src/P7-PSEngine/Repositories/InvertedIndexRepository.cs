using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;

namespace P7_PSEngine.Repositories
{
    public interface IInvertedIndexRepository
    {
        Task AddFileAsync(FileInformation file);
        Task<FileInformation?> FindFileAsync(string fileName, User user);
        Task Save();
        Task<InvertedIndex> FindTerm(string term, User user);
        Task AddInvertedIndexAsync(InvertedIndex invertedIndex);
        Task<TermInformation?> FindExistingFileAsync(string term, string fileId, User user);
        Task EnsureUserExistsOrCreateAsync(User user);

        Task<User?> FindUserAsync(User user);
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

        public async Task AddFileAsync(FileInformation file)
        {
            var existingFile = await db.FileInformation
                .FirstOrDefaultAsync(d => d.FileId == file.FileId && d.UserId == file.UserId);

            if (existingFile == null)
            {
                Console.WriteLine($"Adding document: FileId={file.FileId}, UserId={file.UserId}");
                db.FileInformation.Add(file);
                await db.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine($"Document already exists: DocId={file.FileId}, UserId={file.UserId}");
            }
        }
        public async Task<FileInformation?> FindFileAsync(string fileName, User user) => await db.FileInformation.Include(p => p.TermFiles).FirstOrDefaultAsync(p => p.FileName == fileName && p.User.UserId == user.UserId);

        public async Task<InvertedIndex?> FindTerm(string term, User user) => await db.InvertedIndex.FirstOrDefaultAsync(p => p.Term == term && p.User.UserId == user.UserId);

        public async Task AddInvertedIndexAsync(InvertedIndex invertedIndex)
        {
            await db.InvertedIndex.AddAsync(invertedIndex);
        }
        public async Task<TermInformation?> FindExistingFileAsync(string term, string fileId, User user) => await db.TermInformations.FirstOrDefaultAsync(p => p.Term == term && p.FileId == fileId && p.User.UserId == user.UserId);

        public async Task<User?> FindUserAsync(User user) => await db.Users.FirstOrDefaultAsync(p => p.UserId == user.UserId);

        public async Task AddTermAsync(TermInformation term) => await db.TermInformations.AddAsync(term);

        public async Task EnsureUserExistsOrCreateAsync(User user)
        {
            var existingUser = await db.Users.FindAsync(user.UserId);
            if (existingUser == null)// Delete this in final product
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
        }
    }
}
