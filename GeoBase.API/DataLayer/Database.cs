using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using GeoBase.API.Models;
using Microsoft.VisualBasic.CompilerServices;
using Range = GeoBase.API.Models.Range;

namespace GeoBase.API.DataLayer;

public class Database
{
    private string path = "DB\\geobase.dat";

    public Range[] Ranges;
    public Location[] Locations;
    public uint[] Cities; // Indexes of locations sorted by citites
    public ConcurrentDictionary<string, List<Location>?> CityIndexes = new();
    private Database()
    {
    }

    public static readonly Database Instance = new ();

    public Header Header { get; private set; }


    public void Initialize()
    {
        var sw = Stopwatch.StartNew();
        var bytes = File.ReadAllBytes(path);
        using var binaryReader = new BinaryReader(new MemoryStream(bytes));
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

        BuildCityIndex();
        var ms = sw.ElapsedMilliseconds;
        Debug.WriteLine($"Took {ms} ms");
    }

    private void BuildCityIndex()
    {
        foreach (var index in Cities)
        {
            var location = Locations[index/96]; // 96 is the size of Location struct
            if(CityIndexes.TryGetValue(location.City,out var list))
            {
                list.Add(location);
            }
            else
            {
                CityIndexes.TryAdd(location.City, new List<Location> {location});
            }
        }
    }

    private void LoadCities(BinaryReader binaryReader)
    {
        var cities = new List<uint>();
        while(binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
        {
            cities.Add(binaryReader.ReadUInt32());
        }

        Cities = cities.ToArray();
    }

    private void LoadLocations(BinaryReader binaryReader)
    {
        long length = (Header.OffsetCities - Header.OffsetLocations) / 96; // 96 is the size of Location struct 
        Locations = new Location[length];
        for (int i = 0; i < length; i++)
        {
            var location = new Location
            {
                Country = GetString(binaryReader, 8),
                Region = GetString(binaryReader,12),
                Postal = GetString(binaryReader,12),
                City = GetString(binaryReader,24),
                Organization = GetString(binaryReader,32),
                Latitude = binaryReader.ReadSingle(),
                Longitude = binaryReader.ReadSingle(),
            };
            Locations[i] = location;
        }
    }

    private unsafe void LoadRanges(BinaryReader binaryReader)
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

    static unsafe string GetString(BinaryReader reader,int count)
    {
        var bytes = (sbyte[]) (Array) reader.ReadBytes(count);

        fixed (sbyte* ptr = bytes)
            return new string(ptr);
    }

}