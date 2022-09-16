using System.Collections.Concurrent;
using System.Diagnostics;
using GeoBase.API.Models;
using Range = GeoBase.API.Models.Range;

namespace GeoBase.API.DataLayer;

public class FasterDatabase
{
    private string path = "DB\\geobase.dat";

    long Position;

    public Range[] Ranges;
    public Location[] Locations;
    public ConcurrentDictionary<string, ConcurrentBag<LocationDto>?> CityIndexes = new();
    private FasterDatabase()
    {
    }

    public static readonly FasterDatabase Instance = new();

    public Header Header { get; private set; }


    public void Initialize()
    {
        var sw = Stopwatch.StartNew();
        FastBinaryReader binaryReader = new FastBinaryReader(path);

        Header = new Header
        {
            Version = binaryReader.ReadInt32(),
            DatabaseName = GetString(binaryReader, 32),
            Records = binaryReader.ReadInt32(),
            TimeStamp = binaryReader.ReadUInt64(),
            OffsetRanges = binaryReader.ReadUInt32(),
            OffsetCities = binaryReader.ReadUInt32(),
            OffsetLocations = binaryReader.ReadUInt32()
        };

        LoadRanges(binaryReader);
        LoadLocations(binaryReader);
        LoadCities(binaryReader);
        
        var ms = sw.ElapsedMilliseconds;
        Console.WriteLine($"Took {ms} ms");
    }
    

    private void LoadCities(FastBinaryReader binaryReader)
    {
        while (binaryReader.Position < binaryReader.MemoryLength)
        {
            var index = binaryReader.ReadUInt32();
            Task.Run(() =>
            {
                var location = Locations[index / 96]; // 96 is the size of Location struct
                if (CityIndexes.TryGetValue(GetString(location.City), out var list))
                {
                    list.Add(new LocationDto(location));
                }
                else
                {
                    var dto = new LocationDto(location);
                    CityIndexes.TryAdd(dto.City, new ConcurrentBag<LocationDto> {dto});
                }
            });
        }
    }

    private void LoadLocations(FastBinaryReader binaryReader)
    {
        long length = (Header.OffsetCities - Header.OffsetLocations) / 96; // 96 is the size of Location struct 
        Locations = new Location[length];
        for (int i = 0; i < length; i++)
        {
            var location = new Location
            {
                Country = binaryReader.ReadBytes(8),
                Region = binaryReader.ReadBytes(12),
                Postal = binaryReader.ReadBytes(12),
                City = binaryReader.ReadBytes(24),
                Organization = binaryReader.ReadBytes(32),
                Latitude = binaryReader.ReadSingle(),
                Longitude = binaryReader.ReadSingle(),
            };

            Locations[i] = location;

        }
    }

    private unsafe void LoadRanges(FastBinaryReader binaryReader)
    {
        var length = (Header.OffsetLocations - Header.OffsetRanges) / sizeof(Range);
        Ranges = new Range[length];
        for (int i = 0; i < length; i++)
        {
            var range = new Range
            {
                IpFrom = binaryReader.ReadUInt32(),
                IpTo = binaryReader.ReadUInt32(),
                LocationIndex = binaryReader.ReadUInt32()
            };
            Ranges[i] = range;
        }
    }

    static unsafe string GetString(byte[] bytes)
    {

        fixed (sbyte* ptr = (sbyte[])(Array)bytes)
            return new string(ptr);
    }

    static unsafe string GetString(FastBinaryReader reader, int count)
    {
        var bytes = (sbyte[])(Array)reader.ReadBytes(count);

        fixed (sbyte* ptr = bytes)
            return new string(ptr);
    }
}