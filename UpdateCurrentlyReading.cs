using System;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace goodreads_readme
{
    public static class UpdateCurrentlyReading
    {
        [FunctionName("UpdateCurrentlyReading")]
        public static void Run(
            [TimerTrigger("0 0 0 * * 0")]TimerInfo myTimer, 
            [Blob("books/currentlyReading.json")] out byte[] outputBlob,
            ExecutionContext context, 
            ILogger log)
        {
            log.LogInformation($"Updating goodreads status: {DateTime.Now}");
            
            var path = Path.Combine(context.FunctionAppDirectory, "template.html");
            var feed = GetRss();
            var book = GetMostRecent(feed);
            var svg = CreateSvg(book, path);
            var encodedSvg = Encoding.ASCII.GetBytes(svg);
            
            outputBlob = encodedSvg;
        }

        private static string CreateSvg(Book book, string path)
        {
            string svg = File.ReadAllText(path)
                .Replace("{{book_name}}",book.Title)
                .Replace("{{author}}",book.Author);
            return svg;
        }
        private static Book GetMostRecent(SyndicationFeed feed)
        {
            var mostRecent = feed.Items.Where(i=>i.Summary.Text.Contains("reading")).FirstOrDefault();

            return new Book(mostRecent);
        }
        private static SyndicationFeed GetRss()
        {
            SyndicationFeed feed;
            using (XmlReader reader = XmlReader.Create(GOODREADS_RSS_URL))
            {
                feed = SyndicationFeed.Load(reader);
            }
            return feed;
        }
        private static string GOODREADS_RSS_URL => Environment.GetEnvironmentVariable("GOODREADS_RSS_URL");
    }
}
