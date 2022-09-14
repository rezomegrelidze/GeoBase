using GeoBase.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoBase.API.Controllers
{
    public class GeoBaseController : Controller
    {
        private readonly LocationService _service;

        public GeoBaseController(LocationService service)
        {
            _service = service;
        }

        [HttpGet("/ip")]
        public IActionResult LocationByIp(string ipAddress)
        {
            var location = _service.GetLocation(ipAddress);
            if(location != null)
                return Json(location);
            else
                return NotFound();
        }

        [HttpGet("/city")]
        public IActionResult LocationsByCity(string city)
        {
            var locations = _service.GetLocations(city);
            if (locations != null)
                return Json(locations);
            else
                return NotFound();
        }
    }
}
