using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Authorize]
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private readonly IWorldRepository _worldRepository;
        private readonly ILogger<TripsController> _logger;

        public TripsController(IWorldRepository worldRepository, ILogger<TripsController> logger)
        {
            _worldRepository = worldRepository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            var trips = _worldRepository.GetAllTripsWithStops(User.Identity.Name);
            return Json(Mapper.Map<IEnumerable<TripViewModel>>(trips));
        }

        [HttpPost("")]
        public IActionResult Post([FromBody] TripViewModel vm)
        {
            try
            { 
                if (ModelState.IsValid)
                {
                    var newTrip = Mapper.Map<Trip>(vm);
                    newTrip.UserName = User.Identity.Name;

                    // Save to the database
                    _logger.LogInformation("Attempting to save a new trip");
                    _worldRepository.AddTrip(newTrip);
                    if (_worldRepository.SaveAll())
                    {
                        Response.StatusCode = (int) HttpStatusCode.Created;
                        return Json(Mapper.Map<TripViewModel>(newTrip));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new trip", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new {Message = ex.Message });
            }

            Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return Json(new {Message = "Failed", ModelState = ModelState });
        }
    }
}
