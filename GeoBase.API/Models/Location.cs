namespace GeoBase.API.Models;

public struct Location
{

    public byte[] Country;
    public byte[] Region { get; init; }

    public byte[] Postal { get; init; }
    public byte[] City { get; init; }
    public byte[] Organization { get; init; }
    public float Latitude { get; init; }
    public float Longitude { get; init; }
}