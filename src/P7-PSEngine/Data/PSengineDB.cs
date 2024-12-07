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
        public DbSet<FileInformation> FileInformation => Set<FileInformation>();
        public DbSet<InvertedIndex> InvertedIndex => Set<InvertedIndex>();
        public DbSet<TermInformation> TermInformations => Set<TermInformation>();
        public DbSet<CloudService> CloudService => Set<CloudService>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // FileInformation entity

            modelBuilder.Entity<FileInformation>(entity =>
            {
                // Primary key (composity key of FileId and UserId)
                entity.HasKey(e => new { e.FileId, e.UserId });

                // Required fields
                entity.Property(e => e.FileId)
                    .IsRequired();

                entity.Property(e => e.FileName)
                    .IsRequired();
                
                entity.Property(e => e.UserId)
                    .IsRequired();
                
                entity.Property(e => e.FilePath)
                    .IsRequired();

                entity.Property(e => e.FileType)
                    .IsRequired();

                // Relationships
                
                entity.HasOne(d => d.User)
                    .WithMany(p => p.FileInformations)
                    .HasForeignKey(d => d.UserId)
                    .IsRequired();

                entity.HasMany(d => d.TermFiles)
                    .WithOne(p => p.FileInformation)
                    .HasForeignKey(d => new { d.FileId, d.UserId });
            });

            // InvertedIndex entity
            modelBuilder.Entity<InvertedIndex>(entity =>
            {
                // Primary key (composity key of Term and UserId)
                entity.HasKey(e => new { e.Term, e.UserId });

                // Required fields
                entity.Property(e => e.Term)
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .IsRequired();

                // Relationships
                entity.HasOne(d => d.User)
                    .WithMany(p => p.InvertedIndex)
                    .HasForeignKey(d => d.UserId)
                    .IsRequired();

                entity.HasMany(d => d.TermInformations)
                    .WithOne(p => p.InvertedIndex)
                    .HasForeignKey(d => new { d.Term, d.UserId });
            });

            // TermInformation entity
            modelBuilder.Entity<TermInformation>(entity =>
            {
                // Primary key (composity key of Term, FileId and UserId)
                entity.HasKey(e => new { e.Term, e.FileId, e.UserId });

                // Required fields
                entity.Property(e => e.Term)
                    .IsRequired();

                entity.Property(e => e.FileId)
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.TermFrequency)
                    .IsRequired();

                // Relationships
                entity.HasOne(d => d.InvertedIndex)
                    .WithMany(p => p.TermInformations)
                    .HasForeignKey(d => new { d.Term, d.UserId })
                    .IsRequired();

                entity.HasOne(d => d.FileInformation)
                    .WithMany(p => p.TermFiles)
                    .HasForeignKey(d => new { d.FileId, d.UserId })
                    .IsRequired();
            });

            // CloudService entity
            modelBuilder.Entity<CloudService>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.Id);

                // Required fields
                entity.Property(e => e.ServiceType)
                    .IsRequired();

                entity.Property(e => e.UserDefinedServiceName)
                    .IsRequired();

                entity.Property(e => e.refresh_token)
                    .IsRequired();

                // Relationships
                entity.HasOne(e => e.User)
                    .WithMany(p => p.CloudServices)
                    .HasForeignKey(e => e.UserId)
                    .IsRequired();
            });

            // User entity
            modelBuilder.Entity<User>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.UserId);

                // Required fields
                entity.Property(e => e.UserName)
                    .IsRequired();

                entity.Property(e => e.Password)
                    .IsRequired();
            });

        }

    }

}
