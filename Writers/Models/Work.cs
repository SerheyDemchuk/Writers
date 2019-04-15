using System;
using System.Collections.Generic;

namespace Writers.Models
{
    public class Work
    {
        public int WorkID { get; set; }
        public string AuthorFullName { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string ReleasePlace { get; set; }

        public virtual Person Person { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
    }
}