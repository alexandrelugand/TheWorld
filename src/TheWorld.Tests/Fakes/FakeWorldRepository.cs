using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Models;

namespace TheWorld.Tests.Fakes
{
    public class FakeWorldRepository : IWorldRepository
    {
        public void AddStop(string tripName, string userName, Stop newStop)
        {
        }

        public void AddTrip(Trip newTrip)
        {
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return new List<Trip>()
            {
                new Trip()
                {
                    Id = 1,
                    Name = "Trip Test 1",
                    Created = DateTime.Now,
                    UserName = "User Test"
                },
                new Trip()
                {
                    Id = 2,
                    Name = "Trip Test 2",
                    Created = DateTime.Now,
                    UserName =  "User Test"
                }
            };
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            return new List<Trip>()
            {
                new Trip()
                {
                    Id = 1,
                    Name = "Trip Test 1",
                    Created = DateTime.Now,
                    UserName = "User Test",
                    Stops = new List<Stop>()
                    {
                        new Stop()
                        {
                            Id = 1,
                            Name = "Stop Test 1",
                            Longitude = 100,
                            Latitude = 10,
                            Arrival = DateTime.Now,
                            Order = 1
                        }
                    }
                },
                new Trip()
                {
                    Id = 2,
                    Name = "Trip Test 2",
                    Created = DateTime.Now,
                    UserName =  "User Test",
                    Stops = new List<Stop>()
                    {
                        new Stop()
                        {
                            Id = 2,
                            Name = "Stop Test 1",
                            Longitude = 100,
                            Latitude = 10,
                            Arrival = DateTime.Now,
                            Order = 1
                        }
                    }
                }
            };
        }

        public IEnumerable<Trip> GetAllTripsWithStops(string userName)
        {
            return new List<Trip>()
            {
                new Trip()
                {
                    Id = 1,
                    Name = "Trip Test 1",
                    Created = DateTime.Now,
                    UserName = userName,
                    Stops = new List<Stop>()
                    {
                        new Stop()
                        {
                            Id = 1,
                            Name = "Stop Test 1",
                            Longitude = 100,
                            Latitude = 10,
                            Arrival = DateTime.Now,
                            Order = 1
                        }
                    }
                },
                new Trip()
                {
                    Id = 2,
                    Name = "Trip Test 2",
                    Created = DateTime.Now,
                    UserName = userName,
                    Stops = new List<Stop>()
                    {
                        new Stop()
                        {
                            Id = 2,
                            Name = "Stop Test 1",
                            Longitude = 100,
                            Latitude = 10,
                            Arrival = DateTime.Now,
                            Order = 1
                        }
                    }
                }
            };
        }

        public Trip GetTripByName(string tripName, string userName)
        {
            return new Trip()
            {
                Id = 1,
                Name = "Trip Test",
                Created = DateTime.Now,
                UserName = "User Test",
                Stops = new List<Stop>()
                {
                    new Stop()
                    {
                        Id = 1,
                        Name = "Stop Test",
                        Longitude = 100,
                        Latitude = 10,
                        Arrival = DateTime.Now,
                        Order = 1
                    }
                }
            };
        }

        public bool SaveAll()
        {
            return true;
        }
    }
}
