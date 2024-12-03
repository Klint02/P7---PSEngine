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
        public DbSet<DocumentInformation> DocumentInformation => Set<DocumentInformation>();
        public DbSet<InvertedIndex> InvertedIndex => Set<InvertedIndex>();
        public DbSet<TermInformation> TermInformations => Set<TermInformation>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentInformation>()
                .HasKey(td => new { td.DocId, td.UserId });

            modelBuilder.Entity<InvertedIndex>()
                .HasKey(td => new { td.Term, td.UserId });

            modelBuilder.Entity<TermInformation>()
                .HasKey(td => new { td.Term, td.UserId });

            modelBuilder.Entity<TermInformation>()
                .HasOne(td => td.InvertedIndex)
                .WithMany(ii => ii.TermDocuments)
                .HasForeignKey(td => new { td.Term, td.UserId});

            modelBuilder.Entity<TermInformation>()
                .HasOne(td => td.DocumentInformation)
                .WithMany(d => d.TermDocuments)
                .HasForeignKey(td => new { td.DocID, td.UserId });

            modelBuilder.Entity<DocumentInformation>()
                .HasOne(d => d.User)
                .WithMany(u => u.documentInformations)
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<InvertedIndex>()
                .HasOne(d => d.User)
                .WithMany(u => u.InvertedIndex)
                .HasForeignKey(d => d.UserId);

        }
    }
    
}
