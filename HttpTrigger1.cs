using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Company.Function
{
    public class HttpTrigger1
    {
        // private readonly ILogger<HttpTrigger1> _logger;

        public HttpTrigger1(ILogger<HttpTrigger1> logger)
        {
            // _logger = logger;
        }

        [Function("HttpTrigger1")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            string userAssignedClientId = "52cb4501-d16d-4d06-b6b9-a23d456e9782";
            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = userAssignedClientId });

            var client = new SecretClient(new Uri("https://traxsvault.vault.azure.net/"), credential);

            KeyVaultSecret secret = await client.GetSecretAsync("InitialSecret");

            string responseMessage = $"Secret Value: { secret.Value }";



            return new OkObjectResult(responseMessage);
        }
    }
}
