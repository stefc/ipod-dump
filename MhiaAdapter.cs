namespace stefc.itunes;

public class MhiaAdapter(ChunkRaw chunk) : ChunkAdapter(chunk)
{
    public uint TextCount => this.chunk.N2;
    public uint ArtistID => this.chunk.N3;

    
}