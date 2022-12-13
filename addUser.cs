using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;

namespace Auth;
public static class AddUser
{
    [FunctionName("AddUser")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        [CosmosDB(
            databaseName: "Restaurants",
            containerName: "Users",
            Connection = "CosmosDBConnectionString")]IAsyncCollector<dynamic> documentsOut,
        ILogger log)
    {
        log.LogInformation("AddUser function processed a request.");

        IActionResult returnValue = null;
        try
        {
            string requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            dynamic data = JsonConvert.DeserializeObject<User>(requestBody);

            User newUser = data;

            newUser.id = newUser.Email;
            newUser.partitionKey = newUser.Email;

            await documentsOut.AddAsync(newUser);

            returnValue = new OkObjectResult(new { id = newUser.id });
        }
        catch (Exception ex)
        {
            log.LogError($"Could not insert user. Exception thrown: {ex.Message}");
            returnValue = new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        return returnValue;
    }
}

