namespace GeoBase.API.Models;

public class LocationDto
{
    public string Country;
    public string Region { get; init; }

    public string Postal { get; init; }
    public string City { get; init; }
    public string Organization { get; init; }
    public float Latitude { get; init; }
    public float Longitude { get; init; }

    public LocationDto(Location location)
    {
        Country = GetString(location.Country);
        Region = GetString(location.Region);
        Postal = GetString(location.Postal);
        City = GetString(location.City);
        Organization = GetString(location.Organization);
        Latitude = location.Latitude;
        Longitude = location.Longitude;
    }

    static unsafe string GetString(byte[] bytes)
    {

        fixed (sbyte* ptr = (sbyte[])(Array)bytes)
            return new string(ptr);
    }
}