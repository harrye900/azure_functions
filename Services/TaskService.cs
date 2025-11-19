using TaskManager.Models;

namespace TaskManager.Services
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetAllTasksAsync();
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<TaskItem> CreateTaskAsync(CreateTaskRequest request);
        Task<TaskItem?> UpdateTaskAsync(int id, UpdateTaskRequest request);
        Task<bool> DeleteTaskAsync(int id);
    }

    public class TaskService : ITaskService
    {
        private static readonly List<TaskItem> _tasks = new();
        private static int _nextId = 1;

        public Task<List<TaskItem>> GetAllTasksAsync()
        {
            return Task.FromResult(_tasks.ToList());
        }

        public Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(task);
        }

        public Task<TaskItem> CreateTaskAsync(CreateTaskRequest request)
        {
            var task = new TaskItem
            {
                Id = _nextId++,
                Title = request.Title,
                Description = request.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _tasks.Add(task);
            return Task.FromResult(task);
        }

        public Task<TaskItem?> UpdateTaskAsync(int id, UpdateTaskRequest request)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return Task.FromResult<TaskItem?>(null);

            if (!string.IsNullOrEmpty(request.Title))
                task.Title = request.Title;

            if (!string.IsNullOrEmpty(request.Description))
                task.Description = request.Description;

            if (request.IsCompleted.HasValue)
            {
                task.IsCompleted = request.IsCompleted.Value;
                task.CompletedAt = request.IsCompleted.Value ? DateTime.UtcNow : null;
            }

            return Task.FromResult<TaskItem?>(task);
        }

        public Task<bool> DeleteTaskAsync(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return Task.FromResult(false);

            _tasks.Remove(task);
            return Task.FromResult(true);
        }
    }
}