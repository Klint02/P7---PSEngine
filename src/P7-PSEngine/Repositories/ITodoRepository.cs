/* using P7_PSEngine.Model;

namespace P7_PSEngine.API
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAllTodosAsync();
        Task<Todo?> GetTodoByIdAsync(int id);
        Task<Todo?> GetFitterByName(Todo todo);
        Task AddTodoAsync(Todo todo);
        Task SaveDbChangesAsync();
        void UpdateTodoEntity(Todo existingTodo);
        void DeleteTodoEntity(Todo todo);
    }
}
 */