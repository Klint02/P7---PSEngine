using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Model;

namespace P7_PSEngine.Data
{
    public class PSengineDB : DbContext
    {
        public PSengineDB(DbContextOptions options)
        : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
        public DbSet<User> Users => Set<User>();
        public DbSet<FileInformation> FileInformation => Set<FileInformation>();
        //public DbSet<IndexInformation> IndexInformations => Set<IndexInformation>();

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<FileInformation>(model =>
        //    {
        //        model.ToTable("file_information");
        //        model.HasMany(p => p.IndexInformations).WithOne(p => p.FileInformation);
        //    });
        //}
        public DbSet<CloudService> CloudService => Set<CloudService>();
    }
    
}
