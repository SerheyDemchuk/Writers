using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Writers.ViewModels
{
    public class PersonsDetailsInfo
    {
        public List<string> BiographyTitles { get; set; }

        public List<string> BiographyParagraphs { get; set; }

        public List<int> BiographyParagraphsCount { get; set; }
    }
}