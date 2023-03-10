namespace stefc.itunes;

public class ChunkProxy
{
    protected readonly ChunkRaw chunk;

    public ChunkProxy(ChunkRaw chunk) => this.chunk = chunk;

    public uint ChunkSize => this.chunk.ChunkSize;
    public long Position => this.chunk.Position;
    public ChunkType Id => this.chunk.Id;

     public Span<byte> Data => this.chunk.Data;

}

public static class ChunkFactory {

    public static ChunkProxy Create(ChunkRaw current) => current.Id switch 
    {
        ChunkType.mhbd => new MhbdProxy(current),
        ChunkType.mhsd => new MhsdProxy(current),
        ChunkType.mhlt => new MhltProxy(current),
        ChunkType.mhlp => new MhlpProxy(current),
        ChunkType.mhla => new MhlaProxy(current),
        ChunkType.mhit => new MhitProxy(current),
        ChunkType.mhip => new MhipProxy(current),
        ChunkType.mhyp => new MhypProxy(current),
        ChunkType.mhia => new MhiaProxy(current),
        ChunkType.mhod => new MhodProxy(current),
        _ => new ChunkProxy(current)
    };
}