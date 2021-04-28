using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Linq;
using System.Text;

namespace goodreads_readme
{
    public static class GetCurrentlyReading
    {
        [FunctionName("GetCurrentlyReading")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Goodreads request processed.");

            Book mostRecent = GetMostRecentStarted();
            string div = CreateSVG(mostRecent);
            log.LogInformation(div);
            return new FileContentResult(Encoding.ASCII.GetBytes(div),"image/svg+xml");
        }
        
        private static Book GetMostRecentStarted()
        {
            SyndicationFeed feed;
            using (XmlReader reader = XmlReader.Create(GOODREADS_RSS_URL))
            {
                feed = SyndicationFeed.Load(reader);
            }
            var mostRecent = feed.Items.Where(i=>i.Summary.Text.Contains("reading")).FirstOrDefault();

            return new Book(mostRecent);
        }

        private static string CreateSVG(Book book)
        {
            string svg = File.ReadAllText("./template.html");
            //Commenting out in case I decide to add images back
            //svg = svg.Replace("{{imageHTML}}",book.Image);
            svg = svg.Replace("{{book_name}}",book.Title);
            svg = svg.Replace("{{author}}",book.Author);
            return svg;
        }

        private static string GOODREADS_RSS_URL => Environment.GetEnvironmentVariable("GOODREADS_RSS_URL");
    }
}
