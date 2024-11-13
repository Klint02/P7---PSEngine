using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;

namespace P7_PSEngine.API
{

    public class UserRepository : IUserRepository
    {
        private readonly PSengineDB _db;

        public UserRepository(PSengineDB db)
        {
            _db = db;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }

        // In this example, id must be a primary key for "FindAsync" method to work
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string Username)
        {
            return await _db.Users.FirstOrDefaultAsync(p => p.Username == Username);
        }

        public async Task AddUserAsync(User user)
        {
            await _db.Users.AddAsync(user);       
        }
        // Always use after adding or updating or deleting an entity
        public async Task SaveDbChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void UpdateUserEntity(User existingUser)
        {
            _db.Users.Update(existingUser);
        }

        public void DeleteUserEntity(User user)
        {
            _db.Users.Remove(user);
        }
    }
}
