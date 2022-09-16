using System.Buffers.Binary;

namespace GeoBase.API.DataLayer;

public class FastBinaryReader
{
    public int Position;
    
    private readonly Memory<byte> memory;

    public FastBinaryReader(string path)
    {
        memory = File.ReadAllBytes(path).AsMemory();
    }

    public int MemoryLength => memory.Length;

    public int ReadInt32()
    {
        var result = BinaryPrimitives.ReadInt32LittleEndian(memory.Span.Slice(Position, 4));
        Position += 4;
        return result;
    }

    public byte[] ReadBytes(int count)
    {
        var result = memory.Span.Slice(Position, count).ToArray();
        Position += count;
        return result;
    }

    public ulong ReadUInt64()
    {
        var result = BinaryPrimitives.ReadUInt64LittleEndian(memory.Span.Slice(Position, 8));
        Position += 8;
        return result;
    }

    public uint ReadUInt32()
    {
        var result = BinaryPrimitives.ReadUInt32LittleEndian(memory.Span.Slice(Position, 4));
        Position += 4;
        return result;
    }

    public float ReadSingle()
    {
        var result = BinaryPrimitives.ReadSingleLittleEndian(memory.Span.Slice(Position, 4));
        Position += 4;
        return result;
    }
}