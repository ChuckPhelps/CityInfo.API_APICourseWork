using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc; //Added in for the ControllerBase class.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API.Controller
{
    //Apply API controller to our API.
    [ApiController]
    [Route("api/cities")] //Will map to the proper controller
    public class CitiesController : ControllerBase //Added the ControllerBase class for functions for a controller.
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        //Defined in the CityInfo.API.Services
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ??
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        //JSONified  data.
        public IActionResult GetCities()
        {

            //Repository and context work on. The Actions work off of DTOs.
            var cityEntities = _cityInfoRepository.GetCities();

            ////Create a new list of City Without Points of Interest.
            //var results = new List<CityWithoutPointsOfInterestDto>();

            //foreach (var cityEntity in cityEntities)
            //{
            //    results.Add(new CityWithoutPointsOfInterestDto
            //    {
            //        Id = cityEntity.Id,
            //        Description = cityEntity.Description, //
            //        Name = cityEntity.Name
            //    });

            //}
            //Return the results.

            //Build out a Mapping Statement out to IEnumberable cityEntities.
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }

        //{}are used for parameters do not need the full patch because we have the route above.
        [HttpGet("{id}")] //Attribute of an http get.
        //IActionResult is being used so that we can return the data in the format the consumer is looking for.
        public IActionResult GetCity(int id, bool includePointsOfInterest = false) //Pass the id to get the individudal city.
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);
            //Check to see if the city is null. Then return a NotFound.
            if (city == null)
            {
                return NotFound();
            }

            //Map to a CityDto or the Point Of Interest.
            if (includePointsOfInterest)
            {
                //If Points of Interest might be included we can map to the City Dto.
                //var cityResult = _mapper.Map<CityDto>(city);

                //var cityResult = new CityDto()
                //{
                //    Id = city.Id,
                //    Name = city.Name,
                //    Description = city.Description
                //};
                ////Loop through the list if more than one.
                //foreach (var poi in city.PointsOfInterest)
                //{
                //    cityResult.PointsOfInterest.Add(
                //        new PointOfInterestDto()
                //        {
                //            Id = poi.Id,
                //            Name = poi.Name,
                //            Description = poi.Description
                //        });
                //}
                //No longer neeed the code above as the mapping is done above.
                return Ok(_mapper.Map<CityDto>(city));

            }

            //var cityWithoutPointsOfInterestResult =
            //    new CityWithoutPointsOfInterestDto()
            //    {
            //        Id = city.Id,
            //        Description = city.Description,
            //        Name = city.Name
            //    };

            //Return a 200 with the City Only. Mapping code above no longer required with the _mappper.
            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
          }
    }
}
