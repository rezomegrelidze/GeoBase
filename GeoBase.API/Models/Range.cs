namespace GeoBase.API.Models;

[Serializable]
public struct Range
{
    public uint IpFrom { get; init; }
    public uint IpTo { get; init; }
    public uint LocationIndex { get; init; }
}