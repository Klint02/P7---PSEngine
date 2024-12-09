using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;

namespace P7_PSEngine.API
{

    public class CloudServiceRepository : ICloudServiceRepository
    {
        private readonly PSengineDB _db;

        public CloudServiceRepository(PSengineDB db)
        {
            _db = db;
        }
        public async Task<List<CloudService>> GetAllServiceAsync()
        {
            return await _db.CloudService.ToListAsync();
        }

        // In this example, id must be a primary key for "FindAsync" method to work
        public async Task<CloudService?> GetServiceByIdAsync(int id)
        {
            return await _db.CloudService.FindAsync(id);
        }

        public async Task<CloudService?> GetServiceByDefinedNameAsync(string UserDefinedServiceName)
        {
            return await _db.CloudService.FirstOrDefaultAsync(p => p.UserDefinedServiceName == UserDefinedServiceName);
        }

        public async Task AddServiceAsync(CloudService cloudService)
        {
            await _db.CloudService.AddAsync(cloudService);
        }
        // Always use after adding or updating or deleting an entity
        public async Task SaveDbChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void UpdateServiceEntity(CloudService existingCloudService)
        {
            _db.CloudService.Update(existingCloudService);
        }

        public void DeleteServiceEntity(CloudService cloudService)
        {
            _db.CloudService.Remove(cloudService);
        }
    }
}
