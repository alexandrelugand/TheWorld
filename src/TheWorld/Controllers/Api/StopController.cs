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
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Authorize]
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private readonly IWorldRepository _worldRepository;
        private readonly ILogger<StopController> _logger;
        private readonly CoordService _coordService;

        public StopController(IWorldRepository worldRepository, 
                              ILogger<StopController> logger,
                              CoordService coordService)
        {
            _worldRepository = worldRepository;
            _logger = logger;
            _coordService = coordService;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _worldRepository.GetTripByName(tripName, User.Identity.Name);
                if (trip == null)
                    return Json(null);

                return Json(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get stops for trip {tripName}", ex);
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json("Error occurred finding trip name");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody] StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Map to the entity
                    var newStop = Mapper.Map<Stop>(vm);

                    //Looking up Geocoordinates
                    var coordResult = await _coordService.Lookup(newStop.Name);
                    if(!coordResult.Success)
                    {
                        Response.StatusCode = (int) HttpStatusCode.BadRequest;
                        return Json(coordResult.Message);
                    }

                    newStop.Latitude = coordResult.Latitude;
                    newStop.Longitude = coordResult.Longitude;

                    // Save to the database
                    _worldRepository.AddStop(tripName, User.Identity.Name, newStop);

                    if (_worldRepository.SaveAll())
                    {
                        Response.StatusCode = (int) HttpStatusCode.Created;
                        return Json(Mapper.Map<StopViewModel>(newStop));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new stop", ex);
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json("Failed to save new stop");
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new stop");
        }

    }
}
