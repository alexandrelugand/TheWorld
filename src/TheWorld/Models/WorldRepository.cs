using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private readonly WorldContext _worldContext;
        private readonly ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext worldContext, ILogger<WorldRepository> logger)
        {
            _worldContext = worldContext;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try
            {
                return _worldContext.Trips.OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips from database", ex);
                return null;
            }
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return _worldContext.Trips
                   .Include(t => t.Stops)
                   .OrderBy(t => t.Name)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get stops from database", ex);
                return null;
            }               
        }

        public void AddTrip(Trip newTrip)
        {
            _worldContext.Trips.Add(newTrip);
        }

        public bool SaveAll()
        {
            return _worldContext.SaveChanges() > 0;
        }

        public Trip GetTripByName(string tripName, string userName)
        {
            return _worldContext.Trips
                 .Include(t => t.Stops)
                 .FirstOrDefault(t => t.Name == tripName && t.UserName == userName);
        }

        public void AddStop(string tripName, string userName, Stop newStop)
        {
            var trip = GetTripByName(tripName, userName);
            newStop.Order = trip.Stops.Select(s => s.Order).DefaultIfEmpty(0).Max() + 1;
            trip.Stops.Add(newStop);
            _worldContext.Stops.Add(newStop);
        }

        public IEnumerable<Trip> GetAllTripsWithStops(string userName)
        {
            try
            {
                return _worldContext.Trips
                   .Include(t => t.Stops)
                   .OrderBy(t => t.Name)
                   .Where(t => t.UserName == userName)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get stops from database", ex);
                return null;
            }
        }  
    }
}
