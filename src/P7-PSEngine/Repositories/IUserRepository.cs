using P7_PSEngine.Model;

namespace P7_PSEngine.API
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string Username);
        Task<User> GetUser(User user);
        Task AddUserAsync(User user);
        Task SaveDbChangesAsync();
        void UpdateUserEntity(User existingUser);
        void DeleteUserEntity(User user);
    }
}
