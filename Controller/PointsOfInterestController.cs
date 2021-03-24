using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API.Controller
{
    [ApiController]//API Controller Attribute
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase //Inherit Controller Base.
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        //Use constructor injections.
        //This is where we have added logging, IMailService, and ICityRepository.
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            //Ensure that the logger is not null. 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService)); //Null check on the Mail Service.
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));//Check the Repository.
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet] //Sample call http://localhost:1028/api/cities/1/
        public IActionResult GetPointsofInterest(int cityId)//Accept CityID as a parameter.
        {
            try
            {
                //throw new Exception("Exception Example.");

                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when " +
                        $"accessing points of interest"); //Log if the city does not exist.
                    return NotFound();//Return a 404 not found.
                }

                //Return the Points of Interest for a City
                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestsForCity(cityId);

                ////New list of Point of Interest DTO.
                //var pointsOfInterestForCityResults = new List<PointOfInterestDto>();
                //foreach (var poi in pointsOfInterestForCity)
                //{
                //    pointsOfInterestForCityResults.Add(new PointOfInterestDto()
                //    {
                //        Id = poi.Id,
                //        Name = poi.Name,
                //        Description = poi.Description
                //    });
                //}
                //Map the IEnumerable to the DTO returned from our repository.
                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));

            }
            catch (Exception ex)
            {
                //Log a Critical Error in Log.
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}", ex);
                return StatusCode(500, "A problem happened while handling your request"); //Return a Status Code of 500 with the exception.
                //Do not want to expose internal working here.
            }
        }
        //Added a Name to the Get Attribute so we can refer to it later.
        [HttpGet("{id}", Name="GetPointOfInterest")] //Sample call - http://localhost:1028/api/cities/1/pointsofinterest/1
        public IActionResult GetPointofInterest(int cityId, int id)
        {
            //Make sure that the City does exist.
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }
            //Gather the Point of Interest.
            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            //If it is not there return a null.
            if(pointOfInterest == null)
            {
                return NotFound();
            }

            //Map the Points Of Interest.
            //var pointOfInterestResult = new PointOfInterestDto()
            //{
            //    Id = pointOfInterest.Id,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));

        }


        [HttpPost]//Bad Request will automoatically send a 400 bad request. If this is not here, we would need to add some logic.
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody]PointofInterestForCreationDto pointOfInterest) //Complex Request Type
        {
            //API Controller does not require the code below checking the modelstate.
            
            //Validate that the description is different than the name.
            if(pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name");
            }

            //Need to check the model state is good.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);//Pass the model state to bad request to ensure it is deserialized. 
            }

            //check to see if a city exists before attetmping to add a point of interest to an unexisting city.
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            //=================================== Old Mapping Code=====================================================
            //Data Store code below replaced by repository code.
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if(city == null)
            //{
            //    return NotFound();
            //}
            //Grab the max ID. 
            //var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
            //    c => c.PointsOfInterest).Max(p => p.Id);
            ////Gearing up to add the information into the database or in memory data store.
            //var finalPointofInterest = new PointOfInterestDto()
            //{
            //    Id = ++maxPointOfInterestId,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description,
            //};
            //==================================OLD MAPPING CODE=========================================

            //Mapp the final points of Interest
            var finalPointofInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId, finalPointofInterest);
            _cityInfoRepository.Save();//If something goes wrong here a 500 server error will be returned. CityID and ID will be set here.

            //Mapping back to the models.
            var createPointOfInterestToReturn = _mapper
                .Map<Models.PointOfInterestDto>(finalPointofInterest);

            return CreatedAtRoute(
                "GetPointOfInterest", //Called from above with the name of Get Point of Interest.
                new { cityId, id = createPointOfInterestToReturn.Id }, //Passing in the Route Values. cityId is used as the variable name passed.
                createPointOfInterestToReturn //Final point of Interest.
                );

        }

        [HttpPut("{id}")]//Full updatea are done using the HttpPut method.
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            //=============================Old Code==================================
            ////Validate that the description is different than the name.
            //if (pointOfInterest.Description == pointOfInterest.Name)
            //{       ModelState.AddModelError(
            //        "Description",
            //        "The provided description should be different from the name");
            //}
            //check to see if a city exists before attetmping to add a point of interest to an unexisting city.
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}
            //Look for the Point of Interest.
            //var pointOfInterestFromStore = city.PointsOfInterest
            //    .FirstOrDefault(p => p.Id == id); //Check the point of interest ID.
            //if (pointOfInterest == null)
            //{
            //    return NotFound();
            //}
            //PUT should update all of the resource, except for the ID.
            //pointOfInterestFromStore.Name = pointOfInterest.Name;
            //pointOfInterestFromStore.Description = pointOfInterest.Description;
            //========================================================================

            //Need to check the model state is good.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);//Pass the model state to bad request to ensure it is deserialized. 
            }
            //Check to see if the city exists.
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            //Over ride the elements in the destination object from the source object.
            _mapper.Map(pointOfInterest, pointOfInterestEntity);
            //Added for a best practice.
            _cityInfoRepository.UpdatePointOfInterestForCity(cityId, pointOfInterestEntity);

            //Save the changes to the database.
            _cityInfoRepository.Save();
           
            return NoContent(); //Return a No Content as the update was given by the consumer.

        }

        [HttpPatch("{id}")] //Http Patch attribute
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, [FromBody] 
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc) //JSON Patch Document need to include Microsoft.AspNetCore.JsonPatch
        {
            //========================Old Code================================
            ////Check to see if the City Exists.
            //var city = CitiesDataStore.Current.Cities
            //    .FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            ////Check to see if the point of interest exists.
            //var pointOfInterestFromStore = city.PointsOfInterest
            //    .FirstOrDefault(c => c.Id == id);
            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterestToPatch =
            //    new PointOfInterestForUpdateDto()
            //    {
            //        Name = pointOfInterestFromStore.Name,
            //        Description = pointOfInterestFromStore.Description
            //    };

            //PUT should update all of the resource, except for the ID.
            //pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            //pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            //=================================================================

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            //Map to a Point of Interest Update DTO from the mapper.
            var pointOfInterestToPatch = _mapper
                .Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);
            

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            //Checking the Patch Document.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Needto check the data in the document before applying it.

            //Validate that the description is different than the name.
            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError(
                "Description",
                "The provided description should be different from the name");
            }

            //Validate that the model is good. If it is not return a bad request.
            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState); //Check the model state again after the patch document has been applied.
            }


            //Over ride the elements in the destination object from the source object.
            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            //Added for a best practice.
            _cityInfoRepository.UpdatePointOfInterestForCity(cityId, pointOfInterestEntity);

            //Save the changes to the database.
            _cityInfoRepository.Save();

           
            return NoContent(); //Return a No Content as the update was given by the consumer.

        }

        [HttpDelete("{id}")] //Delete Attribute.
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            //=============================Old Code==============================
            ////Check to see if the City Exists.
            //var city = CitiesDataStore.Current.Cities
            //    .FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            ////Check to see if the point of interest exists.
            //var pointOfInterestFromStore = city.PointsOfInterest
            //    .FirstOrDefault(c => c.Id == id);
            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}

            //If the city and Point of Interest are found we can then remove the point of interest.
            //city.PointsOfInterest.Remove(pointOfInterestFromStore);

            //=====================================================================

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            //Call the delete method.
            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            //Save the changes.
            _cityInfoRepository.Save();

            //Send our message using our mail service. First string is the subject and the second string is the body.
            _mailService.Send("Point of interest deleted",
                $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

            //Return a 204 No Content Header.
            return NoContent();

        }


    }
}
