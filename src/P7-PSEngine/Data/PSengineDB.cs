using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Model;
using System.Runtime.InteropServices.ComTypes;

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
        public DbSet<InvertedIndexInformation> InvertedIndexInformations => Set<InvertedIndexInformation>();
        public DbSet<WordInformation> WordInformations => Set<WordInformation>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileInformation>()
                .HasKey(td => new { td.FileId, td.UserId });

            modelBuilder.Entity<InvertedIndexInformation>()
                .HasKey(td => new { td.Word, td.UserId });

            modelBuilder.Entity<WordInformation>()
                .HasKey(td => new { td.Word, td.UserId });

            modelBuilder.Entity<WordInformation>()
                .HasOne(td => td.InvertedIndex)
                .WithMany(ii => ii.WordInformations)
                .HasForeignKey(td => new { td.Word, td.UserId});

            modelBuilder.Entity<WordInformation>()
                .HasOne(td => td.FileInformation)
                .WithMany(d => d.WordInformations)
                .HasForeignKey(td => new { td.FileID, td.UserId });

            modelBuilder.Entity<FileInformation>()
                .HasOne(d => d.User)
                .WithMany(u => u.FileInformations)
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<InvertedIndexInformation>()
                .HasOne(d => d.User)
                .WithMany(u => u.InvertedIndexInformations)
                .HasForeignKey(d => d.UserId);

        }
    }
    
}
