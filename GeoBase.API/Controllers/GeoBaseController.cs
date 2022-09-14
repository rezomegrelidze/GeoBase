using System.Net;
using GeoBase.API.DataLayer;
using GeoBase.API.Models;
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

    public class LocationService
    {
        private readonly Database _db;

        public LocationService(Database db)
        {
            _db = db;
        }

        public Location? GetLocation(string ipAddress)
        {
            // uses binary search to search through ips
            var address = IPAddress.Parse(ipAddress).Address;
            
            int lo = 0;
            int hi = _db.Ranges.Length - 1;
            while (lo <= hi)
            {
                var mid = (lo + hi) / 2;


                var range = _db.Ranges[mid];
                if(address >= range.IpFrom && address <= range.IpTo)
                    return _db.Locations[range.LocationIndex];

                if (address < range.IpFrom)
                {
                    hi = mid - 1;
                }

                if (address > range.IpTo)
                {
                    lo = mid + 1;
                }
            }

            return null;
        }

        public List<Location>? GetLocations(string city)
        {
            return _db.CityIndexes[city];
        }
    }
}
