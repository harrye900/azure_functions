using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Functions
{
    public class TaskFunctions
    {
        private readonly ILogger<TaskFunctions> _logger;
        private readonly ITaskService _taskService;

        public TaskFunctions(ILogger<TaskFunctions> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        [Function("GetTasks")]
        public async Task<IActionResult> GetTasks([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks")] HttpRequest req)
        {
            _logger.LogInformation("Getting all tasks");
            var tasks = await _taskService.GetAllTasksAsync();
            return new OkObjectResult(tasks);
        }

        [Function("GetTask")]
        public async Task<IActionResult> GetTask([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks/{id:int}")] HttpRequest req, int id)
        {
            _logger.LogInformation("Getting task with ID: {Id}", id);
            var task = await _taskService.GetTaskByIdAsync(id);
            
            if (task == null)
                return new NotFoundObjectResult(new { message = "Task not found" });
            
            return new OkObjectResult(task);
        }

        [Function("CreateTask")]
        public async Task<IActionResult> CreateTask([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tasks")] HttpRequest req)
        {
            _logger.LogInformation("Creating new task");
            
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var createRequest = JsonSerializer.Deserialize<CreateTaskRequest>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (createRequest == null || string.IsNullOrEmpty(createRequest.Title))
                    return new BadRequestObjectResult(new { message = "Title is required" });

                var task = await _taskService.CreateTaskAsync(createRequest);
                return new CreatedResult($"/api/tasks/{task.Id}", task);
            }
            catch (JsonException)
            {
                return new BadRequestObjectResult(new { message = "Invalid JSON format" });
            }
        }

        [Function("UpdateTask")]
        public async Task<IActionResult> UpdateTask([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "tasks/{id:int}")] HttpRequest req, int id)
        {
            _logger.LogInformation("Updating task with ID: {Id}", id);
            
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var updateRequest = JsonSerializer.Deserialize<UpdateTaskRequest>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (updateRequest == null)
                    return new BadRequestObjectResult(new { message = "Invalid request body" });

                var task = await _taskService.UpdateTaskAsync(id, updateRequest);
                
                if (task == null)
                    return new NotFoundObjectResult(new { message = "Task not found" });
                
                return new OkObjectResult(task);
            }
            catch (JsonException)
            {
                return new BadRequestObjectResult(new { message = "Invalid JSON format" });
            }
        }

        [Function("DeleteTask")]
        public async Task<IActionResult> DeleteTask([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "tasks/{id:int}")] HttpRequest req, int id)
        {
            _logger.LogInformation("Deleting task with ID: {Id}", id);
            
            var deleted = await _taskService.DeleteTaskAsync(id);
            
            if (!deleted)
                return new NotFoundObjectResult(new { message = "Task not found" });
            
            return new NoContentResult();
        }
    }
}