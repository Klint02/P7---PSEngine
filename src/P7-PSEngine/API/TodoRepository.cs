using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;

namespace P7_PSEngine.API
{
    public class TodoRepository
    {
        private readonly TodoDb _db;

        public TodoRepository(TodoDb db)
        {
            _db = db;
        }

        public async Task<List<Todo>> GetAllTodosAsync()
        {
            return await _db.Todos.ToListAsync();
        }

        public async Task<Todo?> GetTodoByIdAsync(int id)
        {
            return await _db.Todos.FindAsync(id);
        }
    }
}
