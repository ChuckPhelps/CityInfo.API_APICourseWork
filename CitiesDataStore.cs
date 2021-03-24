using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {

        //Static Property can keep working on the same instance.
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
    
        //List of Citites
        public List<CityDto> Cities { get; set; } //List of cities.

        public CitiesDataStore()
        {
            Cities = new List<CityDto>() //Dummy Data
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one wit that big park.",

                    PointsOfInterest = new List<PointOfInterestDto>()
                     {
                         new PointOfInterestDto() {
                             Id = 1,
                             Name = "Central Park",
                             Description = "The most visited urban park in the United States." },
                          new PointOfInterestDto() {
                             Id = 2,
                             Name = "Empire State Building",
                             Description = "A 102-story skyscraper located in Midtown Manhattan." },
                     }

                },
                 new CityDto()
                {
                    Id = 2,
                    Name = "AntWerp",
                    Description = "The one with the cathedral that was really finished.",
                     PointsOfInterest = new List<PointOfInterestDto>()
                     {
                         new PointOfInterestDto() {
                             Id = 3,
                             Name = "Cathedral of Our Lady",
                             Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." },
                          new PointOfInterestDto() {
                             Id = 4,
                             Name = "Antwerp Central Station",
                             Description = "The the finest example of railway architecture in Belgium." },
                     }
                },
                 new CityDto()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "Moo Cows.",
                     PointsOfInterest = new List<PointOfInterestDto>()
                     {
                         new PointOfInterestDto() {
                             Id = 5,
                             Name = "Eiffel Tower",
                             Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel." },
                          new PointOfInterestDto() {
                             Id = 6,
                             Name = "The Louvre",
                             Description = "The world's largest museum." },
                     }
                }
            };
        }


    }
}
