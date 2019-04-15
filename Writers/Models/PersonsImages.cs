using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Writers.Models
{
    public class PersonsImages
    {
        [Key]
        [ForeignKey("Person")]
        public string  PersonFullName { get; set; }
        public string ImagePath { get; set; }

        public virtual Person Person { get; set; }
    }
}