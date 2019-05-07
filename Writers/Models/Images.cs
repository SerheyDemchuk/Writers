using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Writers.Models
{
    public class Images
    {
        public int ImagesID { get; set; }   
        public string ImagePath { get; set; }

        public virtual Person Person { get; set; }
    }
}