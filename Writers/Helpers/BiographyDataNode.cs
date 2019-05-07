using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Writers.Helpers
{
    public class BiographyDataNode
    {
        public ICollection<BiographyDataNode> Nodes { get; set; }

        public string Title { get; set; }

        public List<string> Paragraph { get; set; }

        public BiographyDataNode()
        {
            Nodes = new List<BiographyDataNode>();
            Title = string.Empty;
            Paragraph = new List<string>();
        }

    }
}