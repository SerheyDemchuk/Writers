using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Writers.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Display(Name = "Born")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime BirthDate { get; set; }


        [StringLength(50)]
        public string BirthPlace { get; set; }

        [Display(Name = "Died")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd:MM:yyyy}")]
        public DateTime DieDate { get; set; }


        [StringLength(50)]
        public string DiePlace { get; set; }


        [RegularExpression(@"^([0-9]{1,2}(th)?-?){1,2}(\sB\.C\.)?$")]
        [StringLength(15)]
        public string Century { get; set; }


        [Display(Name = "Citizenship")]
        [RegularExpression(@"^[A-Z][a-zA-Z\s]*$")]
        [StringLength(31)]
        public string Country { get; set; }


        [StringLength(100)]
        public string Occupation { get; set; }


        [StringLength(100)]
        public string Education { get; set; }


        public string Biography { get; set; }


        public virtual ICollection<Genre> Genres { get; set; }
        public virtual PersonsImages Image { get; set; }
    }
}