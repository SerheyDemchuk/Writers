using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Writers.Models
{
    public class Genre
    {
        public int GenreID { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
        public virtual ICollection<Work> Works { get; set; }
    }
}