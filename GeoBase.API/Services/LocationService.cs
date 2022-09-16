using System.Collections.Concurrent;
using System.Net;
using GeoBase.API.DataLayer;
using GeoBase.API.Models;

namespace GeoBase.API.Services;

public class LocationService
{
    public Location? GetLocation(string ipAddress)
    {
        // uses binary search to search through ranges
        var address = IPAddress.Parse(ipAddress).Address;
        var database = Database.Instance;
        int lo = 0;
        int hi = database.Ranges.Length - 1;
        while (lo <= hi)
        {
            var mid = (lo + hi) / 2;

            var range = database.Ranges[mid];
            if (address >= range.IpFrom && address <= range.IpTo)
                return database.Locations[range.LocationIndex];

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

    public ConcurrentBag<LocationDto>? GetLocations(string city)
    {
        var database = Database.Instance;
        return database.CityIndexes[city];
    }
}