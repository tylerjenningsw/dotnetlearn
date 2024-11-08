using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Services;

public class InMemoryTaskService : ITaskService
{
    private readonly List<Todo> _tasks = new();
    
    public Task<Todo?> GetTaskById(int id) => Task.FromResult(_tasks.Find(t => t.Id == id));
    public Task<IEnumerable<Todo>> GetTasks() => Task.FromResult(_tasks.AsEnumerable());
    public Task<Todo> CreateTask(Todo task)
    {
        _tasks.Add(task);
        return Task.FromResult(task);
    }
    public Task<Todo> DeleteTaskById(int id)
    {
        var task = _tasks.Find(t => t.Id == id) ?? throw new KeyNotFoundException($"Task with id {id} not found");
        _tasks.Remove(task);
        return Task.FromResult(task);
    }
    public Task<Todo> UpdateTask(Todo task)
    {
        var index = _tasks.FindIndex(t => t.Id == task.Id);
        _tasks[index] = task;
        return Task.FromResult(task);
    }
} 