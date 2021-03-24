using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; //Required Library for Annotations.
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class PointofInterestForCreationDto
    {
        //Data Annotations that help us control Input. 
        //Can send an error message back if the field is left blank.
        [Required(ErrorMessage = "You should provide a name value")] //Set a field that is required.
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
