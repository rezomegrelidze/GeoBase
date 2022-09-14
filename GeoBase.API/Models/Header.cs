namespace GeoBase.API.Models;

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
