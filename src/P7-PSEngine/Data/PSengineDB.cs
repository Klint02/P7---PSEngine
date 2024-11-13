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
    }
    
}
