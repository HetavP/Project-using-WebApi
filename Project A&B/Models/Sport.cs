using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Project_A_B.Models
{
    [ModelMetadataType(typeof(SportMetaData))]

    public class Sport : Auditable
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public Byte[] RowVersion { get; set; }


        // Navigation property to Athletes
        public ICollection<Athlete> Athletes { get; set; }

    }
}
