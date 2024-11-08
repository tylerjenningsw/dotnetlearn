namespace WebApplication1.Services;

public interface ITaskService
{
    Task<Todo?> GetTaskById(int id);
    Task<IEnumerable<Todo>> GetTasks();
    Task<Todo> CreateTask(Todo task);
    Task<Todo> DeleteTaskById(int id);
    Task<Todo> UpdateTask(Todo task);
} 