using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;

namespace goodreads_readme
{
    public static class GetCurrentlyReading
    {
        [FunctionName("GetCurrentlyReading")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,[Blob("books/currentlyReading.json")] byte[] storageBlob, ILogger log)
        {
            log.LogInformation("Goodreads request processed.");
            try
            {    
                log.LogInformation(Encoding.ASCII.GetString(storageBlob));
                return new FileContentResult(storageBlob,"image/svg+xml");
            }
            catch
            {
                return new NoContentResult();
            }
        }
    }
}
