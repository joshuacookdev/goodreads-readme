using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;

namespace goodreads_readme
{
    internal class Book
    {
        internal Book(SyndicationItem item)
        {
            MatchCollection match = Regex.Matches(item.Summary.Text, anchorReg);
            (ImageLink, TitleLink, AuthorLink) = (match[0].Value, match[1].Value, match[2].Value);
        }

        const string anchorReg = "<a.*?/a>";

        internal string TitleLink {get;private set;}
        internal string AuthorLink {get;private set;}
        internal string ImageLink {get;private set;}
    }
}
