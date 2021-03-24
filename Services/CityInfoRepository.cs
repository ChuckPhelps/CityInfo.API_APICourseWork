using CityInfo.API.Context;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {

        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            //Constructor Injection.
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<City> GetCities()
        {
            //Can return the cities ordered by name and executed at the time with ToList.
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            //If includePointsOfIntrest is true, we can pass in the collection of points of interest.
            if (includePointsOfInterest)
            {
                //To use the include statement we needed to add entity framework core to our using statement.
                return _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefault();
            }

            return _context.Cities
                .Where(c => c.Id == cityId).FirstOrDefault();

        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            //Filter on CityId and Point of Interest ID.
            return _context.PointOfInterest.
                Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestsForCity(int cityId)
        {
            return _context.PointOfInterest
                .Where(p => p.CityId == cityId).ToList();
        }

        //Method returns true if a city exists and false if it does not.
        public bool CityExists(int cityId)
        {
            //Uses the Any Method.
            return _context.Cities.Any(c => c.Id == cityId);
        }

        //Method to add a Point of Interest.
        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(pointOfInterest);
        }

        public void UpdatePointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {

        }

        //Pass through the point of interest we want to delete.
        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            //Remove the item from the context on dbset.
            _context.PointOfInterest.Remove(pointOfInterest);
        }

        //Save changes on the _context.
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);//Return true when zero or more entities have been saved.
        }

    }
}
