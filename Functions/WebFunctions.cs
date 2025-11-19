using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TaskManager.Functions
{
    public class WebFunctions
    {
        private readonly ILogger<WebFunctions> _logger;

        public WebFunctions(ILogger<WebFunctions> logger)
        {
            _logger = logger;
        }

        [Function("Home")]
        public async Task<IActionResult> Home([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "")] HttpRequest req)
        {
            _logger.LogInformation("Serving home page");
            
            var html = await File.ReadAllTextAsync("wwwroot/index.html");
            return new ContentResult
            {
                Content = html,
                ContentType = "text/html",
                StatusCode = 200
            };
        }

        [Function("HealthCheck")]
        public IActionResult HealthCheck([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequest req)
        {
            _logger.LogInformation("Health check requested");
            
            return new OkObjectResult(new 
            { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                version = "1.0.0"
            });
        }
    }
}