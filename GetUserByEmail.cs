using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Auth;

public static class GetUserByEmail
{
    [FunctionName("GetUserByEmail")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "GetUser/{email}")] HttpRequest req,
        [CosmosDB(
            databaseName: "Restaurants",
            containerName: "Users",
            Connection = "CosmosDBConnectionString",
            Id = "{email}",
            PartitionKey = "{email}")] User user,
        ILogger log)
    {
        log.LogInformation("GetUserByEmail HTTP trigger function processed a request.");

        if (user == null)
      {
        log.LogInformation($"User not found");
        return new NotFoundResult();
      }
      else
      {
        log.LogInformation($"Found User, Name: {user.Username}");
        return new OkObjectResult(user);
      }
    }
}

