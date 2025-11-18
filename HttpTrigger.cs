using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AzureFunctions
{
    public class HttpTrigger
    {
        private readonly ILogger<HttpTrigger> _logger;

        public HttpTrigger(ILogger<HttpTrigger> logger)
        {
            _logger = logger;
        }

        [Function("HttpTrigger")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            if (string.IsNullOrEmpty(name))
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                if (!string.IsNullOrEmpty(requestBody))
                {
                    try
                    {
                        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(requestBody);
                        data?.TryGetValue("name", out name);
                    }
                    catch (JsonException)
                    {
                        // Invalid JSON, ignore
                    }
                }
            }

            if (!string.IsNullOrEmpty(name))
            {
                var response = new { message = $"Hello, {name}!" };
                return new OkObjectResult(response);
            }
            else
            {
                var response = new { message = "Please provide a name parameter" };
                return new BadRequestObjectResult(response);
            }
        }
    }
}