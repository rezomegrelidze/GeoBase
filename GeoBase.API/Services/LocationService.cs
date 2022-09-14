using System.Net;
using GeoBase.API.DataLayer;
using GeoBase.API.Models;

namespace GeoBase.API.Services;

public class LocationService
{
    private readonly Database _database;

    public LocationService(Database database)
    {
        _database = database;
    }

    public Location? GetLocation(string ipAddress)
    {
        // uses binary search to search through ranges
        var address = IPAddress.Parse(ipAddress).Address;

        int lo = 0;
        int hi = _database.Ranges.Length - 1;
        while (lo <= hi)
        {
            var mid = (lo + hi) / 2;

            var range = _database.Ranges[mid];
            if (address >= range.IpFrom && address <= range.IpTo)
                return _database.Locations[range.LocationIndex];

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
        return _database.CityIndexes[city];
    }
}