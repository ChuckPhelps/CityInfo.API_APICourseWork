using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API.Profiles
{
    public class CityProfile : Profile
    {
        //City Profiles need to be defined in the constructor.
        public CityProfile()
        {
            //Convention based will map from the source to destination object.
            //Map from a city entity to CityWithoutPointsOfInterestDto
            CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
            //Map from Etities City to Models CityDto
            CreateMap<Entities.City, Models.CityDto>();
        }
    }
}
