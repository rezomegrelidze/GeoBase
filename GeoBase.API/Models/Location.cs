namespace GeoBase.API.Models;

public struct Location
{
    public string Country { get; init; }
    public string Region { get; init; }

    public string Postal { get; init; }
    public string City { get; init; }
    public string Organization { get; init; }
    public float Latitude { get; init; }
    public float Longitude { get; init; }
}