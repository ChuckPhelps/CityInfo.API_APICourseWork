using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class PointOfInterestForUpdateDto
    {
        [Required(ErrorMessage = "You should provide a name value")]//Error Message Returned if name is too long.
        [MaxLength(50)] //Max length of 50 from this Attribute.
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
