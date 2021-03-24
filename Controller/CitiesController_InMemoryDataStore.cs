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

    //Added the ControllerBase class for functions for a controller.
    public class CitiesController : ControllerBase 
    {

        private readonly ICityInfoRepository _cityInfoRepository;

        //Defined in the CityInfo.API.Services
        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository ??
                throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        [HttpGet]
        //JSONified  data.
        public IActionResult GetCities()
        {
            //Repository and context work on. The Actions work off of DTOs.
            var cityEntities = _cityInfoRepository.GetCities();

            //Create a new list of City Without Points of Interest.
            var results = new List<CityWithoutPointsOfInterestDto>();

            foreach (var cityEntity in cityEntities)
            {
                results.Add(new CityWithoutPointsOfInterestDto
                {
                    Id = cityEntity.Id,
                    Description = cityEntity.Description,
                    Name = cityEntity.Name
                });
               
            }

            return Ok(results);

        }

        //{}are used for parameters do not need the full patch because we have the route above.
        [HttpGet("{id}")] //Attribute of an http get.
        //IActionResult is being used so that we can return the data in the format the consumer is looking for.
        public IActionResult GetCity(int id) //Pass the id to get the individudal city.
        {

            var cityToReturn = CitiesDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == id);

            if (cityToReturn == null)
            {
                //If the city is not found a 404 error will be delivered.
                return NotFound();
            }

            //If the City is found it will return a 200 status code and the city.
            return Ok(cityToReturn);

          }
    }
}
