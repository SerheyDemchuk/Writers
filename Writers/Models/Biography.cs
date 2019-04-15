using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Writers.Models
{
    public class Biography
    {
        [Key]
        [ForeignKey("Person")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [StringLength(50)]
        public string PersonFullName { get; set; }

        public string Titles { get; set; }

        public string BiographyText { get; set; }


        public virtual Person Person { get; set; }
    }
}