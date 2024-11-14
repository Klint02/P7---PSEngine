using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;

namespace P7_PSEngine.API
{

    public class TodoRepository : ITodoRepository
    {
        private readonly PSengineDB _db;

        public TodoRepository(PSengineDB db)
        {
            _db = db;
        }

        public async Task<List<Todo>> GetAllTodosAsync()
        {
            return await _db.Todos.ToListAsync();
        }

        // In this example, id must be a primary key for "FindAsync" method to work
        public async Task<Todo?> GetTodoByIdAsync(int id)
        {
            return await _db.Todos.FindAsync(id);
        }

        public async Task<Todo?> GetFitterByName(Todo todo)
        {
            return await _db.Todos.FirstOrDefaultAsync(p => p.Name == todo.Name);
        }

        public async Task AddTodoAsync(Todo todo)
        {
            await _db.Todos.AddAsync(todo);
        }
        // Always use after adding or updating or deleting an entity
        public async Task SaveDbChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void UpdateTodoEntity(Todo existingTodo)
        {
            _db.Todos.Update(existingTodo);
        }

        public void DeleteTodoEntity(Todo todo)
        {
            _db.Todos.Remove(todo);
        }
    }
}
