namespace stefc.itunes;

public class ChunkAdapter(ChunkRaw chunk)
{
    protected readonly ChunkRaw chunk = chunk;

    public uint ChunkSize => this.chunk.ChunkSize; // Chunklänge inkl. aller Kindelemente
    public long Position => this.chunk.Position; // Position im File

    public long Sibling => this.chunk.Sibling; // Position des nächsten Chunks

    public long Length => this.chunk.Length; // Länge des Chunks
    
    public ChunkType Id => this.chunk.Id;

    public Span<byte> Data => this.chunk.Data;
}

public static class ChunkFactory {

    public static ChunkAdapter Create(ChunkRaw current) => current.Id switch 
    {
        ChunkType.mhbd => new MhbdAdapter(current),
        ChunkType.mhsd => new MhsdAdapter(current),
        ChunkType.mhlt => new MhltAdapter(current),
        ChunkType.mhlp => new MhlpAdapter(current),
        ChunkType.mhla => new MhlaAdapter(current),
        ChunkType.mhit => new MhitAdapter(current),
        ChunkType.mhip => new MhipAdapter(current),
        ChunkType.mhyp => new MhypAdapter(current),
        ChunkType.mhia => new MhiaAdapter(current),
        ChunkType.mhod => new MhodAdapter(current),
        _ => new ChunkAdapter(current)
    };
}