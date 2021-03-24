using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; //Needed to be added for [Key] attribute
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    public class City
    {
        [Key] //Makes Entity classes more acceptable at first glance.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //New key is generated when a city is added.
        public int Id { get; set; }
        [Required] //Name is Required.
        [MaxLength(50)] //Set the Max Length to 50.
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }

        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
            = new List<PointOfInterest>();
        
    }
}
