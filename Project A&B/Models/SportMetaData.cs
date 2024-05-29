using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Project_A_B.Models
{
    public class SportMetaData : Auditable
    {
        //public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the sport code blank.")]
        [RegularExpression("^[A-Z]{3}$", ErrorMessage = "The sport code must be exactly 3 capital letters.")]
        [StringLength(3)]
        public string Code { get; set; }

        [Required(ErrorMessage = "You cannot leave the sport name blank.")]
        [StringLength(50, ErrorMessage = "Sport name cannot be more than 50 characters long.")]
        public string Name { get; set; }

        [Timestamp]
        public Byte[] RowVersion { get; set; }


        //// Navigation property to Athletes
        //public ICollection<Athlete> Athletes { get; set; }

    }
}
