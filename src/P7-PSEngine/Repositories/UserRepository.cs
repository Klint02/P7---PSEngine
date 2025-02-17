﻿using Microsoft.EntityFrameworkCore;
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
            return await _db.Users.FirstOrDefaultAsync(p => p.UserName == Username);
        }

        public async Task RemoveAllUsersAsync()
        {
            _db.Users.RemoveRange(_db.Users);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> AddUserAsync(User user)
        {
            User? user2 = await GetUserByUsernameAsync(user.UserName);
            if (user2 == null) {
                await _db.Users.AddAsync(user);
                await SaveDbChangesAsync();    
                return true;
            } else {
                return false;
            }
        }
        // Always use after adding or updating or deleting an entity
        public async Task SaveDbChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<User?> GetUser(User user)
        {
            return await _db.Users.FindAsync(user);
        }

        public void UpdateUserEntity(User existingUser)
        {
            _db.Users.Update(existingUser);
        }

        public void DeleteUserEntity(User user)
        {
            _db.Users.Remove(user);
        }

        public async Task EnsureUserExistsAsync(int userId)
        {
            var userExists = await _db.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                throw new InvalidOperationException($"User with ID {userId} does not exist.");
            }
        }
    }
}
