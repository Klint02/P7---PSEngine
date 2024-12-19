using P7_PSEngine.Model;

namespace P7_PSEngine.API
{
    public interface ICloudServiceRepository
    {
        Task<List<CloudService>> GetAllServiceAsync();
        Task<CloudService?> GetServiceByIdAsync(int id);
        Task<CloudService?> GetServiceByDefinedNameAsync(string UserDefinedServiceName);
        Task AddServiceAsync(CloudService cloudService);
        Task<CloudService?> GetServiceByUserAsync(User user);
        Task SaveDbChangesAsync();
        void UpdateServiceEntity(CloudService existingCloudService);
        void DeleteServiceEntity(CloudService cloudService);
    }
}
