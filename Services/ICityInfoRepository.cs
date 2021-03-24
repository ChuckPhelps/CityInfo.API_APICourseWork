using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {

        IEnumerable<City> GetCities();
        //Get a CityID with a boolean of Y or N for the Points of Interest.
        City GetCity(int cityId, bool includePointsOfInterest);
        //Get all of the points of Interest for a city.
        IEnumerable<PointOfInterest> GetPointsOfInterestsForCity(int cityId);
        //Get a single point of Interest.
        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);
        //True of Flase that a city exists.
        bool CityExists(int cityId);

        //Add a method to add a Point Of Interest
        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        void UpdatePointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);


        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        //Save to the Database Method
        bool Save();

    }
}
