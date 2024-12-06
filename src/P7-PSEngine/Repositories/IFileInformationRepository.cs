using P7_PSEngine.Model;

namespace P7_PSEngine.API
{
    public interface IFileInformationRepository
    {
        Task<List<FileInformation>> GetAllFileInformationAsync();
        Task<FileInformation?> GetFileInformationByIdAsync(int id);
        Task<FileInformation?> GetFileInformationByNameAsync(string FileInformationName);
        Task RemoveUserCache(User UID, CloudService SID);
        Task AddFileInformationAsync(FileInformation FileInformation);
        Task AddFileInformationRangeAsync(List<FileInformation> FileInformation);
        Task SaveDbChangesAsync();
        void UpdateFileInformationEntity(FileInformation existingFileInformation);
        void DeleteFileInformationEntity(FileInformation File);
    }
}
