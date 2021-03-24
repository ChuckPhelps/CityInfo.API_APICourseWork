using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    //Setup the model for PointsofInterest Model.
    public class PointOfInterestDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }



}
