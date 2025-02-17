﻿using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;

namespace P7_PSEngine.API
{

    public class FileInformationRepository : IFileInformationRepository
    {
        private readonly PSengineDB _db;

        public FileInformationRepository(PSengineDB db)
        {
            _db = db;
        }
        public async Task<List<FileInformation>> GetAllFileInformationAsync()
        {
            return await _db.FileInformation.ToListAsync();
        }

        // In this example, id must be a primary key for "FindAsync" method to work
        public async Task<FileInformation?> GetFileInformationByIdAsync(int id)
        {
            return await _db.FileInformation.FindAsync(id);
        }

        public async Task<FileInformation?> GetFileInformationByNameAsync(string FileInformationName)
        {
            return await _db.FileInformation.FirstOrDefaultAsync(p => p.FileName == FileInformationName);
        }

        public async Task RemoveUserCache(User user, CloudService SID)
        {
            _db.FileInformation.RemoveRange(_db.FileInformation.Where(p => p.User.UserName == user.UserName && p.SID.UserDefinedServiceName == SID.UserDefinedServiceName));
            await _db.SaveChangesAsync();
        }

        public async Task AddFileInformationAsync(FileInformation FileInformation)
        {
            await _db.FileInformation.AddAsync(FileInformation);
        }

        public async Task AddFileInformationRangeAsync(List<FileInformation> fileInformation)
        {
            var validFiles = fileInformation.Where(file => !string.IsNullOrEmpty(file.FileId)).ToList();

            foreach (var file in validFiles)
            {
                if (string.IsNullOrEmpty(file.FileId))
                {
                    throw new InvalidOperationException("FileId cannot be null or empty.");
                }
            }
            await _db.FileInformation.AddRangeAsync(validFiles);
        }
        // Always use after adding or updating or deleting an entity
        public async Task SaveDbChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void UpdateFileInformationEntity(FileInformation existingFileInformation)
        {
            _db.FileInformation.Update(existingFileInformation);
        }

        public void DeleteFileInformationEntity(FileInformation FileInformation)
        {
            _db.FileInformation.Remove(FileInformation);
        }
    }
}
