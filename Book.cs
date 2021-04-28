using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;

namespace goodreads_readme
{
    internal class Book
    {
        internal Book(SyndicationItem item)
        {
            MatchCollection match = Regex.Matches(item.Summary.Text, anchorReg);
            //Image = ParseImage(match[0].Value);
            Title = ParseValue(match[1].Value);
            Author = ParseValue(match[2].Value);
        }

        private static string ParseValue(string input)=>Regex.Match(input, valueReg).Value.Replace(">","").Replace("<","");

        const string anchorReg = "<a.*?/a>";
        const string valueReg = ">.*?<";
        const string imgLinkReg = "<img.*?/>";

        internal string Title {get;private set;}
        internal string Author {get;private set;}
        
        //Commenting these in case I decide to add images back
        //internal string Image {get;private set;}
        //private static string ParseImage(string input)=>Regex.Match(input,imgLinkReg).Value;
    }
}
