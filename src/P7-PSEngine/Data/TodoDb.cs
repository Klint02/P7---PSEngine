using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Model;

namespace P7_PSEngine.Data
{
    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions options)
        : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
    }
}
