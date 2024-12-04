using P7_PSEngine.Model;

namespace P7_PSEngine.API
{
    public interface ICloudServiceRepository
    {
        Task<List<CloudService>> GetAllServiceAsync();
        Task<CloudService?> GetServiceByIdAsync(int id);
        Task<CloudService?> GetServiceByDefinedNameAsync(string UserDefinedServiceName);
        Task AddServiceAsync(CloudService cloudService);
        Task SaveDbChangesAsync();
        void UpdateServiceEntity(CloudService existingCloudService);
        void DeleteServiceEntity(CloudService cloudService);
    }
}
