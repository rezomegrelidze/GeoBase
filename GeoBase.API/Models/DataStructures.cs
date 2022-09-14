namespace GeoBase.API.Models;

public struct Range
{
    public uint IpFrom { get; init; }
    public uint IpTo { get; init; }
    public uint LocationIndex { get; init; }
}

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


public struct Header
{
    public int Version { get; init; }
    public string DatabaseName { get; init; }
    public uint OffsetLocations { get; init; }

    public uint OffsetCities { get; init; }

    public uint OffsetRanges { get; init; }

    public ulong TimeStamp { get; init; }
    public int Records { get; init; }
}
