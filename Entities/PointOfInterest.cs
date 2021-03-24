using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key] //Key as an attribute for the Point of Interest
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] //Name is Required.
        [MaxLength(50)] //Set the Max Length to 50.
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [ForeignKey("CityId")]//Annotate the CityId as the FK to the Points Of Interest.
        public City City { get; set; } //Will target the primary key of the principal entity. The FK will be the City ID.
        public int CityId { get; set;}

    }
}
