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
        Task<CloudService?> GetCloudService(User user);
    }
    public class InvertedIndexRepository : IInvertedIndexRepository
    {
        private readonly PSengineDB _db;
        public InvertedIndexRepository(PSengineDB db)
        {
            _db = db;
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task AddFileAsync(FileInformation file)
        {
            var existingFile = await _db.FileInformation
                .FirstOrDefaultAsync(d => d.FileId == file.FileId && d.UserId == file.UserId);

            if (existingFile == null)
            {
                _db.FileInformation.Add(file);
            }
        }
        public async Task<FileInformation?> FindFileAsync(string fileName, User user) 
        {
            return await _db.FileInformation.FirstOrDefaultAsync(p => p.FileName == fileName && p.User.UserId == user.UserId);
        }

        public async Task<InvertedIndex?> FindTerm(string term, User user) => await _db.InvertedIndex.FirstOrDefaultAsync(p => p.Term == term && p.User.UserId == user.UserId);

        public async Task AddInvertedIndexAsync(InvertedIndex invertedIndex)
        {
            await _db.InvertedIndex.AddAsync(invertedIndex);
        }
        public async Task<TermInformation?> FindExistingFileAsync(string term, string fileId, User user) => await _db.TermInformations.FirstOrDefaultAsync(p => p.Term == term && p.FileId == fileId && p.User.UserId == user.UserId);

        public async Task<User?> FindUserAsync(User user) => await _db.Users.FirstOrDefaultAsync(p => p.UserId == user.UserId);

        public async Task AddTermAsync(TermInformation term) => await _db.TermInformations.AddAsync(term);

        public async Task EnsureUserExistsOrCreateAsync(User user)
        {
            var existingUser = await _db.Users.FindAsync(user.UserId);
            if (existingUser == null)// Delete this in final product
            {
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<CloudService?> GetCloudService(User user)
        {
            return await _db.CloudService.FirstOrDefaultAsync(p => p.User.UserId == user.UserId);
        }
    }
}
