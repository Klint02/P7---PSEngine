using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Model;

namespace P7_PSEngine.Data
{
    public class PSengineDB : DbContext
    {
        public PSengineDB()
        {
            
        }

        public PSengineDB(DbContextOptions<PSengineDB> options)
        : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
        public DbSet<User> Users => Set<User>();
        public DbSet<FileInformation> FileInformations => Set<FileInformation>();
        public DbSet<IndexInformation> IndexInformations => Set<IndexInformation>();

    }
    
}
